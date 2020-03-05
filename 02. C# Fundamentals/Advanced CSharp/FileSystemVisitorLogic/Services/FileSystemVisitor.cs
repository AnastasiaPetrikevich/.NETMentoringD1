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
			ValidateSourcePath(sourcePath);

			DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
			OnStart?.Invoke(this, new FileSystemVisitorEventArgs(sourceDirectory, $"Started iterating through ${sourceDirectory.FullName}"));

			foreach (var file in SearchFiles(sourceDirectory))
				yield return file;

			foreach (var directory in SearchChildDirectories(sourceDirectory))
				yield return directory;

			OnFinish?.Invoke(this, new FileSystemVisitorEventArgs(sourceDirectory, $"Finishing iterating through ${sourceDirectory.FullName}"));
		}

		private void ValidateSourcePath(string sourcePath)
		{
			if (string.IsNullOrWhiteSpace(sourcePath))
			{
				throw new ArgumentNullException(nameof(sourcePath));
			}

			if (!_visitorService.IsDirectoryExists(sourcePath))
			{
				throw new ArgumentException($"This directory path: {sourcePath} doesn't exist");
			}
		}

		private IEnumerable<FileSystemInfo> SearchChildDirectories(DirectoryInfo path)
		{
			foreach (var directory in _visitorService.SearchDirectories(path))
			{
				if (searchFilter != null && searchFilter(directory))
				{
					var filterArgs = new FileSystemVisitorEventArgs(directory, "Filtered Directory Found");
					OnFilteredDirectoryFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stop)
						yield break;

					if (filterArgs.Status != SearchStatus.Exclude)
						yield return directory;
				}

				var findArgs = new FileSystemVisitorEventArgs(directory, "Directory Found");
				OnDirectoryFinded?.Invoke(this, findArgs);

				if (findArgs.Status == SearchStatus.Stop)
					yield break;

				if (findArgs.Status != SearchStatus.Exclude)
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
					var filterArgs = new FileSystemVisitorEventArgs(file, "Filtered File Found");
					OnFilteredFileFinded?.Invoke(this, filterArgs);

					if (filterArgs.Status == SearchStatus.Stop)
						yield break;

					if (filterArgs.Status == SearchStatus.Exclude)
						yield return file;
				}

				var findArgs = new FileSystemVisitorEventArgs(file, "File Found");
				OnFileFinded?.Invoke(this, findArgs);

				if (findArgs.Status == SearchStatus.Stop)
					yield break;

				if (findArgs.Status != SearchStatus.Exclude)
					yield return file;
			}
		}
	}
}
