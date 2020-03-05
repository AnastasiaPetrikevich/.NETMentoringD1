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
        bool IsStopOnException { get; set; }
        IEnumerable<FileSystemInfo> SearchDirectories(DirectoryInfo path);
		IEnumerable<FileSystemInfo> SearchFiles(DirectoryInfo path);
		bool IsDirectoryExists(DirectoryInfo path);
	}
}
