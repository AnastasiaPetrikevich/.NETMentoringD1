using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherLogic.Configuration
{
	public class DestinationElement : ConfigurationElement
	{
		[ConfigurationProperty("searchTemplate", IsRequired = true, IsKey = true)]
		public string SearchTemplate => (string)base["searchTemplate"];

		[ConfigurationProperty("destinationFolder", IsRequired = true)]
		public string DestinationFolder => (string)base["destinationFolder"];

		[ConfigurationProperty("isDateRequired", IsRequired = false, DefaultValue = false)]
		public bool IsDateRequired => (bool)base["isDateRequired"];

		[ConfigurationProperty("isNumberRequired", IsRequired = false, DefaultValue = false)]
		public bool IsNumberRequired => (bool)base["isNumberRequired"];
	}
}
