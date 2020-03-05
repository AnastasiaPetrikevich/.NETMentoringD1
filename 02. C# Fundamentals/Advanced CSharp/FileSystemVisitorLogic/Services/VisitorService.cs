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
		public IEnumerable<FileSystemInfo> SearchFiles(DirectoryInfo directoryInfo) => directoryInfo.EnumerateFiles();

		public IEnumerable<FileSystemInfo> SearchDirectories(DirectoryInfo directoryInfo) => directoryInfo.EnumerateDirectories();

		public bool IsDirectoryExists(string directoryPath) => Directory.Exists(directoryPath);

	}
}
