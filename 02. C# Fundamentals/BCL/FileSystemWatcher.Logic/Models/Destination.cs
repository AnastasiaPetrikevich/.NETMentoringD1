using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcher.Models
{
	public class Destination
	{
		public string SearchTemplate { get; set; }

		public string DestinationDirectory { get; set; }

		public bool IsNumberRequired { get; set; }
	
		public bool IsDateRequired { get; set; }
	}
}
