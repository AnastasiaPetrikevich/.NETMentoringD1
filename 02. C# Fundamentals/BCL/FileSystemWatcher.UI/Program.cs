using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using FileSystemWatcher.Interfaces;
using FileSystemWatcher.Models;
using FileSystemWatcher.Services;
using FileSystemWatcher.UI.Configuration;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher.UI
{
	class Program
	{
		private static Services.FileSystemWatcher watcher;

		static void Main(string[] args)
		{
			ILogger logger = new Logger();
			var configuration = ConfigurationManager.GetSection("fileSystemSection") as FileSystemWatcherSection;

			ValidateConfiguration(configuration, logger);

			var directories = new List<string>(configuration.Directories.Count);
			var destinations = new List<Destination>();
			
			configuration.Directories.Cast<DirectoryElement>().ToList().ForEach(d => directories.Add(d.DirectoryPath));
			configuration.Destinations.Cast<DestinationElement>().ToList().ForEach(d => destinations.Add(new Destination
			{
				SearchTemplate = d.SearchTemplate,
				DestinationDirectory = d.DestinationDirectory,
				IsDateRequired = d.IsDateRequired,
				IsNumberRequired = d.IsNumberRequired
			}));

			CultureInfo.DefaultThreadCurrentCulture = configuration.Culture;
			CultureInfo.DefaultThreadCurrentUICulture = configuration.Culture;

			logger.Log(ResourcesString.CultureInfo);

			watcher = new Services.FileSystemWatcher(directories, destinations, configuration.Destinations.DefaultDirectory, logger);

			watcher.FileCreated += OnCreated;

			Console.ReadKey();
		}

		private static void ValidateConfiguration(FileSystemWatcherSection configuration, ILogger logger)
		{
			if (configuration == null)
			{
				logger.Log(ResourcesString.ConfigurationNotFound);
			}
		}

		private static void OnCreated(object sender, FileCreatedEventArgs<FileInfo> args)
		{
			watcher.MoveItem(args.CreatedItem);
		}
	}
}
