using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherLogic.Configuration
{
	public class FileSystemWatcherSection : ConfigurationSection
	{
		[ConfigurationProperty("culture", DefaultValue = "en-US")]
		public CultureInfo Culture => (CultureInfo)base["culture"];

		[ConfigurationCollection(typeof(DirectoryElement), AddItemName = "directory")]
		[ConfigurationProperty("directories")]
		public DirectoryElementCollection Directories => (DirectoryElementCollection)base["directories"];

		[ConfigurationCollection(typeof(DestinationElement), AddItemName = "destination")]
		[ConfigurationProperty("destinations")]
		public DestinationElementCollection Destinations => (DestinationElementCollection)base["destinations"];
	}
}
