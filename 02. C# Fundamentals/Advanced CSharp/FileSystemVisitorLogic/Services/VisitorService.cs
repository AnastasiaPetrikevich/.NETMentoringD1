using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemVisitorLogic.Models;

namespace FileSystemVisitorLogic.Services
{
	public class VisitorService : IVisitorService
	{
        public bool IsStopOnException { get; set; }

        public VisitorService(bool isStop = false)
        {
            IsStopOnException = isStop;
        }

        public IEnumerable<FileSystemInfo> SearchFiles(DirectoryInfo filePath)
		{
			IEnumerable<FileInfo> files = null;
			try
			{
				files = filePath.EnumerateFiles();
			}
			catch (UnauthorizedAccessException)
			{
                if (IsStopOnException)
                {
                    throw new UnauthorizedAccessException($"You don't have access to the {filePath.FullName}.");
                }
            }

			if (files != null)
			{
				foreach (var file in files)
				{
					yield return file;
				}
			}
		}

		public IEnumerable<FileSystemInfo> SearchDirectories(DirectoryInfo directoryPath)
		{
			IEnumerable<DirectoryInfo> directories = null;
			try
			{
				directories = directoryPath.EnumerateDirectories();
			}
			catch (UnauthorizedAccessException)
			{
                if (IsStopOnException)
                {
                    throw new UnauthorizedAccessException($"You don't have access to the {directoryPath.FullName}.");
                }				
			}

			if (directories != null)
			{
				foreach (var directory in directories)
				{
					yield return directory;
				}
			}
		}

		public bool IsDirectoryExists(DirectoryInfo path) => path.Exists;               
	}
}
