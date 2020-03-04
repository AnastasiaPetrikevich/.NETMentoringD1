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
		private readonly IVisitorService _visitorService;
		private readonly Predicate<FileSystemInfo> searchFilter;

		public event EventHandler<EventArgs> OnStart;
		public event EventHandler<EventArgs> OnFinish;
		public event EventHandler<FileSystemVisitorEventArgs> OnFileFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnDirectoryFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnFilteredFileFinded;
		public event EventHandler<FileSystemVisitorEventArgs> OnFilteredDirectoryFinded;

		public FileSystemVisitor(IVisitorService visitorService)
		{
			this._visitorService = visitorService ?? throw new ArgumentNullException(nameof(visitorService));
		}

		public FileSystemVisitor(IVisitorService visitorService, Predicate<FileSystemInfo> searchFilter)
		{
			this._visitorService = visitorService ?? throw new ArgumentNullException(nameof(visitorService));
			this.searchFilter = searchFilter;
		}

		public IEnumerable<FileSystemInfo> Search(string sourcePath)
		{
			if (string.IsNullOrWhiteSpace(sourcePath))
			{
				throw new ArgumentNullException(nameof(sourcePath));
			}

			DirectoryInfo path = new DirectoryInfo(sourcePath);

			if (!_visitorService.IsDirectoryExists(path))
			{
				throw new ArgumentException($"This directory path: {path} doesn't exist");
			}

			OnStart?.Invoke(this, new EventArgs());

			foreach (var file in SearchFiles(path))
				yield return file;

			foreach (var directory in SearchChildDirectories(path))
				yield return directory;

			//var allFiles = SearchFiles(path).ToList();
			//allFiles.AddRange(SearchChildDirectories(path));
			
			OnFinish?.Invoke(this, new EventArgs());
		}

		private IEnumerable<FileSystemInfo> SearchChildDirectories(DirectoryInfo path)
		{
			var directories = _visitorService.SearchDirectories(path);

			foreach (var directory in directories)
			{
				if (searchFilter != null && searchFilter(directory))
				{
					var filterArgs = new FileSystemVisitorEventArgs(directory);
					OnFilteredDirectoryFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stopped)
						yield break;

					if (filterArgs.Status == SearchStatus.Exclude)
						yield return directory;
				}

				var findArgs = new FileSystemVisitorEventArgs(directory);
				OnDirectoryFinded?.Invoke(this, findArgs);

				if (findArgs.Status == SearchStatus.Stopped)
					yield break;

				if (findArgs.Status == SearchStatus.Exclude)
					yield return directory;

				foreach (var file in SearchFiles(directory as DirectoryInfo))
					yield return file;

				foreach (var childDirectory in SearchChildDirectories(directory as DirectoryInfo))
					yield return childDirectory;
				
			}
		}

		private IEnumerable<FileSystemInfo> SearchFiles(DirectoryInfo path)
		{
			var files = _visitorService.SearchFiles(path);

			foreach (var file in files)
			{
				if (searchFilter != null && searchFilter(file))
				{
					var filterArgs = new FileSystemVisitorEventArgs(file);
					OnFilteredFileFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stopped)
						yield break;

					if (filterArgs.Status == SearchStatus.Exclude)
						yield return file;
				}

				var findArgs = new FileSystemVisitorEventArgs(file);
				OnFileFinded?.Invoke(this, findArgs);

				if (findArgs.Status == SearchStatus.Stopped)
					yield break;

				if (findArgs.Status == SearchStatus.Exclude)
					yield return file;
			}
		}
	}
}
