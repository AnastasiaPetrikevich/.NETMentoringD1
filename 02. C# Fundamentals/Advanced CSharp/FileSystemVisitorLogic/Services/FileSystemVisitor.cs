using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemVisitorLogic.Models;

namespace FileSystemVisitorLogic.Services
{
	public class FileSystemVisitor
	{
		private readonly ISearchService searchService;
		private readonly Predicate<string> searchFilter;

		public event EventHandler<EventArgs> OnStart;
		public event EventHandler<EventArgs> OnFinish;
		public event EventHandler<FileSystemVisitorEventArgs> OnFileFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnDirectoryFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnFilteredFileFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnFilteredDirectoryFinded;

		public FileSystemVisitor(ISearchService searchService)
		{
			this.searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
		}

		public FileSystemVisitor(ISearchService searchService, Predicate<string> searchFilter)
		{
			this.searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
			this.searchFilter = searchFilter;
		}

		public void Search(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}

			if (!searchService.IsDirectoryExists(path))
			{
				throw new ArgumentException($"This directory path: {path} doesn't exist");
			}

			OnStart?.Invoke(this, new EventArgs());
			SearchFiles(path);
			SearchChildDirectories(path);
			OnFinish?.Invoke(this, new EventArgs());
		}

		private void SearchChildDirectories(string path)
		{
			var directories = searchService.SearchDirectories(path);

			foreach (var directory in directories)
			{
				if (searchFilter != null && searchFilter(directory))
				{
					var filterArgs = new FileSystemVisitorEventArgs(directory);
					OnFilteredDirectoryFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stopped)
						break;

					if (filterArgs.Status == SearchStatus.Exclude)
						continue;
				}

				var findArgs = new FileSystemVisitorEventArgs(directory);
				OnDirectoryFinded?.Invoke(this, findArgs);

				SearchFiles(directory);
				SearchChildDirectories(directory);

				if (findArgs.Status == SearchStatus.Stopped)
					break;
			}
		}

		private void SearchFiles(string path)
		{
			var files = searchService.SearchFiles(path);

			foreach (var file in files)
			{
				if (searchFilter != null && searchFilter(file))
				{
					var filterArgs = new FileSystemVisitorEventArgs(file);
					OnFilteredFileFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stopped)
						break;
				
					if (filterArgs.Status == SearchStatus.Exclude)
						continue;
				}

				var findArgs = new FileSystemVisitorEventArgs(file);
				OnFileFinded?.Invoke(this, findArgs);

				if (findArgs.Status == SearchStatus.Stopped)
					break;
			}
		}
	}
}
