using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemVisitorLogic.Models;

namespace FileSystemVisitorLogic.Services
{
	public class SearchService : ISearchService
	{
		public IEnumerable<string> SearchFiles(string path)
		{
			var files = Directory.GetFiles(path);

			foreach (var file in files)
			{
				yield return file;
			}
		}

		public IEnumerable<string> SearchDirectories(string path)
		{
			var directories = Directory.GetDirectories(path);

			foreach (var directory in directories)
			{
				yield return directory;
			}
		}

		public bool IsDirectoryExists(string path) => Directory.Exists(path);

	}
}
