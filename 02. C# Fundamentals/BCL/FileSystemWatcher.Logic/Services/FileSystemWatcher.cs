using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using FileSystemWatcher.Configuration;
using FileSystemWatcher.Interfaces;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher.Services
{
    public class FileSystemWatcher
    {
        private readonly ILogger logger;
        private readonly List<DestinationElement> destinations;
        private readonly string defaultDirectory;

        public FileSystemWatcher(List<DestinationElement> destinations, string defaultDirectory, ILogger logger)
        {
            this.destinations = destinations;
            this.defaultDirectory = defaultDirectory;
            this.logger = logger;
        }

        public void MoveItem(FileInfo file)
        {
            int count = 0;
            string itemPath = file.FullName;

            foreach (var destination in destinations)
            {
                var template = new Regex(destination.SearchTemplate);
                var isMatch = template.IsMatch(file.Name);

                if (isMatch)
                {
                    count++;
                    logger.Log(ResourcesString.TemplateMatched);
                    string destinationPath = CreateDestinationPath(file, destination, count);
                    Move(itemPath, destinationPath);
                    logger.Log(string.Format(ResourcesString.FileMoved, file.FullName, destinationPath));
                    return;
                }
            }

            string defaultPath = Path.Combine(defaultDirectory, file.Name);
            logger.Log(ResourcesString.TemplateNotMatched);
            Move(itemPath, defaultPath);
            logger.Log(string.Format(ResourcesString.FileMoved, file.FullName, defaultPath));
        }
	    private void Move(string itemPath, string destinationPath)
	    {
		    string directoryName = Path.GetDirectoryName(destinationPath);
		    bool fileAccess = true;

		    Directory.CreateDirectory(directoryName ?? throw new InvalidOperationException());
		    do
		    {
			    try
			    {
				    if (File.Exists(destinationPath))
				    {
					    File.Delete(destinationPath);
				    }
				    File.Move(itemPath, destinationPath);
				    fileAccess = false;
			    }
			    catch (FileNotFoundException)
			    {
				    logger.Log(ResourcesString.FileNotFound);
				    break;
			    }
			    catch (IOException)
			    {
					logger.Log(ResourcesString.FileLocked);
				    break;
				}
		    } while (fileAccess);
	    }
		private string CreateDestinationPath(FileInfo file, DestinationElement destination, int count)
        {
            string extension = Path.GetExtension(file.Name);
            string filename = Path.GetFileNameWithoutExtension(file.Name);

	        var destinationPath = new StringBuilder();
	        destinationPath.Append(Path.Combine(destination.DestinationDirectory, filename));

            if (destination.IsDateRequired)
            {
                var format = CultureInfo.CurrentCulture.DateTimeFormat;
                format.DateSeparator = ".";
                destinationPath.Append($" {DateTime.Now.ToLocalTime().ToString(format.ShortDatePattern)}");
            }

            if (destination.IsNumberRequired)
            {
                destinationPath.Append($" {count}");
            }

            return destinationPath.Append(extension).ToString();
        }
      
    }
}
