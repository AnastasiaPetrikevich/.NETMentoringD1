using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherLogic.Models
{
	public class FileCreateEventArgs<T> : EventArgs
	{
		public T CreatedItem { get; set; }
	}
}
