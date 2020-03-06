using FileSystemWatcher.Configuration;
using FileSystemWatcher.Interfaces;
using FileSystemWatcher.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using FileSystemWatcher.Services;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher
{
	class Program
	{
		private static Services.FileSystemWatcher watcher;

		static void Main(string[] args)
		{
			ILogger logger = new Logger();
			var configuration = ConfigurationManager.GetSection("fileSystemSection") as FileSystemWatcherSection;

			if (configuration == null)
			{
				logger.Log(ResourcesString.ConfigurationNotFound);
				return;
			}

			var directories = new List<string>(configuration.Directories.Count);
			var destinations = new List<DestinationElement>();

			foreach (DirectoryElement directory in configuration.Directories)
			{
				directories.Add(directory.DirectoryPath);
			}

			foreach (DestinationElement destination in configuration.Destinations)
			{
				destinations.Add(destination);
			}

			CultureInfo.DefaultThreadCurrentCulture = configuration.Culture;
			CultureInfo.DefaultThreadCurrentUICulture = configuration.Culture;

			logger.Log(ResourcesString.CultureInfo);

			watcher = new Services.FileSystemWatcher(destinations, configuration.Destinations.DefaultDirectory, logger);
			var service = new FileSystemWatcherService(directories, logger);

			service.FileCreated += OnCreated;

			Console.ReadKey();
		}

		private static void OnCreated(object sender, FileCreatedEventArgs<FileInfo> args)
		{
			watcher.MoveItem(args.CreatedItem);
		}
	}
}
