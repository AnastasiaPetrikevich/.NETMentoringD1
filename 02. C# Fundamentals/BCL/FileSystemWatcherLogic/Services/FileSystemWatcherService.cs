using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemWatcherLogic.Interfaces;
using FileSystemWatcherLogic.Models;
using ResourceString = FileSystemWatcherLogic.Resources.Resource;

namespace FileSystemWatcherLogic.Services
{
	public class FileSystemWatcherService
	{
		private readonly List<System.IO.FileSystemWatcher> fileSystemWatchers;
		private readonly ILogger logger;
		public event EventHandler<FileCreateEventArgs<FileInfo>> FileCreated;

		public FileSystemWatcherService(IEnumerable<string> dir, ILogger logger)
		{
			this.logger = logger;
			this.fileSystemWatchers = dir.Select(CreateWatcher).ToList();
		}

		private FileSystemWatcher CreateWatcher(string directory)
		{
			if (string.IsNullOrEmpty(directory))
			{
				throw new ArgumentNullException($"Directory {nameof(directory)} is null or empty.");
			}

			var fileSystemWatcher = new FileSystemWatcher(directory)
			{
				NotifyFilter = NotifyFilters.FileName,
				EnableRaisingEvents = true
			};

			fileSystemWatcher.Created += (s, e) =>
			{
				OnCreated(new FileInfo(e.FullPath));
				logger.Log(string.Format(ResourceString.FileFound, e.Name, File.GetCreationTime(e.FullPath)));
				
			};

			return fileSystemWatcher;
		}

		private void OnCreated(FileInfo file)
		{
			FileCreated?.Invoke(this, new FileCreateEventArgs<FileInfo> { CreatedItem = file });
		}
	}
}
