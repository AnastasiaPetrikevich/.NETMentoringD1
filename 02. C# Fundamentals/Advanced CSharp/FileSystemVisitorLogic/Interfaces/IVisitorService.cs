using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorLogic.Models
{
	public interface IVisitorService
	{
		IEnumerable<FileSystemInfo> SearchDirectories(DirectoryInfo directoryInfo);
		IEnumerable<FileSystemInfo> SearchFiles(DirectoryInfo directoryInfo);
		bool IsDirectoryExists(string directoryPath);
	}
}
