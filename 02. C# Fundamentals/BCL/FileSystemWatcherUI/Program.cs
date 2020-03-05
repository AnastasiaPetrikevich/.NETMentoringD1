using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using FileSystemWatcherLogic.Configuration;
using FileSystemWatcherLogic.Interfaces;
using FileSystemWatcherLogic.Models;
using FileSystemWatcherLogic.Services;
using ResourceString = FileSystemWatcherLogic.Resources.Resource;

namespace FileSystemWatcherUI
{
	class Program
	{
		private static FileWatcher watcher;

		static void Main(string[] args)
		{
			ILogger logger = new Logger();
			FileSystemWatcherSection configuration = ConfigurationManager.GetSection("fileSystemSection") as FileSystemWatcherSection;

			if (configuration != null)
			{
				var directories = new List<string>(configuration.Directories.Count);
				var destinations = new List<DestinationElement>();

				foreach (DirectoryElement directory in configuration.Directories)
				{
					directories.Add(directory.DirectoryPath);
				}

				foreach (DestinationElement rule in configuration.Destinations)
				{
					destinations.Add(rule);
				}

				CultureInfo.DefaultThreadCurrentCulture = configuration.Culture;
				CultureInfo.DefaultThreadCurrentUICulture = configuration.Culture;
			

				watcher = new FileWatcher(destinations, configuration.Destinations.DefaultDirectory, logger);

				logger.Log(ResourceString.CultureInfo);

				var service = new FileSystemWatcherService(directories, logger);
				service.FileCreated += OnCreated;
			}
		}

		private static void OnCreated(object sender, FileCreateEventArgs<FileInfo> args)
		{
			watcher.MoveItem(args.CreatedItem);
		}
	}
}
