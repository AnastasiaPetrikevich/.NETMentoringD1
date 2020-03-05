using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherLogic.Configuration
{
	public class DirectoryElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new DirectoryElement();

		protected override object GetElementKey(ConfigurationElement element) =>
			((DirectoryElement)element).DirectoryPath;
	}
}
