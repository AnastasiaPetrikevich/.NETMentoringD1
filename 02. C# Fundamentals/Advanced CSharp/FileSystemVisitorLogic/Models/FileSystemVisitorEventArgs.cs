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

		public FileSystemInfo FileSystemInfo { get; }
		public string Message { get; }

		public FileSystemVisitorEventArgs(FileSystemInfo fileSystemInfo, string Message = null)
		{
			FileSystemInfo = fileSystemInfo;
			this.Message = Message;
		}


	}
}
