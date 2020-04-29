using System;
using System.IO;
using System.Linq;
using WGETAnalog.Logic.Interfaces;

namespace WGETAnalog.Logic.InterfacesImplementation
{
    public class Saver : ISaver
    {
        private readonly DirectoryInfo directory;

        public Saver(DirectoryInfo directory)
        {
            this.directory = directory;
        }

        public void SaveHtml(Uri url, string name, Stream stream)
        {
            string directoryPath = BuildLocation(directory, url);

            Directory.CreateDirectory(directoryPath);
            name = ExcludeInvalidSymbols(name);

            string fileFullPath = Path.Combine(directoryPath, name);

            SaveContent(stream, fileFullPath);
            stream.Close();
        }

        public void SaveFile(Uri url, Stream stream)
        {
            string filePath = BuildLocation(directory, url);
            var directoryPath = Path.GetDirectoryName(filePath);

            Directory.CreateDirectory(directoryPath);

            if (Directory.Exists(filePath))
            {
                filePath = Path.Combine(filePath, Guid.NewGuid().ToString());
            }

            SaveContent(stream, filePath);
            stream.Close();
        }

        private void SaveContent(Stream stream, string filePath)
        {
            var fileStream = File.Create(filePath);

            stream.CopyTo(fileStream);
            fileStream.Close();
        }

        private string BuildLocation(DirectoryInfo dir, Uri url)
        {
            return Path.Combine(dir.FullName, url.Host) + url.LocalPath.Replace("/", @"\");
        }

        private string ExcludeInvalidSymbols(string fileName)
        {
            var invalidSymbols = Path.GetInvalidFileNameChars();

            return new string(fileName.Where(c => !invalidSymbols.Contains(c)).ToArray());
        }
    }
}