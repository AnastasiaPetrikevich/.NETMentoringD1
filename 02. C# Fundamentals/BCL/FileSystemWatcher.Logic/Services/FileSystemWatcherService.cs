using FileSystemWatcher.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSystemWatcher.Models;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher.Services
{
    public class FileSystemWatcherService
    {
        private readonly List<System.IO.FileSystemWatcher> fileSystemWatchers;
        private readonly ILogger logger;
        public event EventHandler<FileCreatedEventArgs<FileInfo>> FileCreated;

        public FileSystemWatcherService(IEnumerable<string> dir, ILogger logger)
        {
            this.logger = logger;
            this.fileSystemWatchers = dir.Select(CreateWatcher).ToList();
        }

        private System.IO.FileSystemWatcher CreateWatcher(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException($"Directory {nameof(directory)} is null or empty.");
            }

            var fileSystemWatcher = new System.IO.FileSystemWatcher(directory)
            {
                NotifyFilter = NotifyFilters.FileName,
                EnableRaisingEvents = true
            };

            fileSystemWatcher.Created += (s, e) =>
            {
                logger.Log(string.Format(ResourcesString.FileFound, e.Name, File.GetCreationTime(e.FullPath)));
                OnCreated(new FileInfo(e.FullPath));
            };

            return fileSystemWatcher;
        }

        private void OnCreated(FileInfo file)
        {
            FileCreated?.Invoke(this, new FileCreatedEventArgs<FileInfo> { CreatedItem = file });
        }
    }
}
