using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FileSystemWatcherLogic.Configuration;
using FileSystemWatcherLogic.Interfaces;
using ResourceString = FileSystemWatcherLogic.Resources.Resource;

namespace FileSystemWatcherLogic.Services
{
	public class FileWatcher
	{
		private readonly ILogger logger;
		private readonly List<DestinationElement> destinations;
		private readonly string defaultDirectory;

		public FileWatcher(List<DestinationElement> destinations, string defaultDirectory, ILogger logger)
		{
			this.destinations = destinations;
			this.defaultDirectory = defaultDirectory;
			this.logger = logger;
		}

		public void MoveItem(FileInfo file)
		{
			int count = 0;
			string filePath = file.FullName;

			foreach (var rule in destinations)
			{
				var template = new Regex(rule.SearchTemplate);
				var isMatch = template.IsMatch(file.Name);

				if (isMatch)
				{
					count++;
					logger.Log(ResourceString.TemplateMatched);
					string destinationPath = CreateDestinationPath(file, rule, count);
					Move(filePath, destinationPath);
					logger.Log(string.Format(ResourceString.FileMoved, file.FullName, destinationPath));
					return;
				}
			}

			string defaultPath = Path.Combine(defaultDirectory, file.Name);
			logger.Log(ResourceString.TemplateNotMatched);
			Move(filePath, defaultPath);
			logger.Log(string.Format(ResourceString.FileMoved, file.FullName, defaultPath));
		}

		private void Move(string itemPath, string destinationPath)
		{
			string directoryName = Path.GetDirectoryName(destinationPath);
			bool fileAcess = true;

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
					fileAcess = false;
				}
				catch (FileNotFoundException)
				{
					logger.Log(ResourceString.FileNotFound);
					break;
				}
				catch (IOException)
				{
					Thread.Sleep(2000);
				}
			} while (fileAcess);
		}

		private string CreateDestinationPath(FileInfo file, DestinationElement destination, int count)
		{
			string extension = Path.GetExtension(file.Name);
			string filename = Path.GetFileNameWithoutExtension(file.Name);

			var destinationPath = new StringBuilder()
				.Append(Path.Combine(destination.DestinationFolder, filename));

			if (destination.IsNumberRequired)
			{
				destinationPath.Append($" {count}");
			}

			if (destination.IsDateRequired)
			{
				var format = CultureInfo.CurrentCulture.DateTimeFormat;
				format.DateSeparator = ".";
				destinationPath.Append($" {DateTime.Now.ToLocalTime().ToString(format.ShortDatePattern)}");
			}

			return destinationPath.Append(extension).ToString();
		}
	}
}
