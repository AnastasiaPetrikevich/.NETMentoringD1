using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorLogic.Models
{
	public class FileSystemVisitorEventArgs : EventArgs
	{
		public SearchStatus Status { get; set; }

		public string Path { get; }
		
		public FileSystemVisitorEventArgs(string path)
		{
			Path = path;
		}
	}
}
