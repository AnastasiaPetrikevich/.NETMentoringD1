using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherLogic.Configuration
{
	public class DestinationElementCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty("defaultDirectory", IsRequired = true)]
		public string DefaultDirectory => (string)base["defaultDirectory"];

		protected override ConfigurationElement CreateNewElement() => new DestinationElement();

		protected override object GetElementKey(ConfigurationElement element) => ((DestinationElement)element).SearchTemplate;

	}
}
