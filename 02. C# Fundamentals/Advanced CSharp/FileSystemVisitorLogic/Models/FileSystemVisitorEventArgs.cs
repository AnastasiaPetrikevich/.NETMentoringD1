using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorLogic.Models
{
	public class FileSystemVisitorEventArgs : EventArgs
	{
		public SearchStatus Status { get; set; }

		public FileSystemInfo Path { get; }
		
		public FileSystemVisitorEventArgs(FileSystemInfo path)
		{
			Path = path;
		}
	}
}
