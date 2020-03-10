using System.Configuration;
using System.Globalization;

namespace FileSystemWatcher.UI.Configuration
{
    public class FileSystemWatcherSection : ConfigurationSection
    {
        [ConfigurationProperty("culture", DefaultValue = "en-US")]
        public CultureInfo Culture => (CultureInfo)this["culture"];

        [ConfigurationCollection(typeof(DirectoryElement), AddItemName = "directory")]
        [ConfigurationProperty("directories")]
        public DirectoryElementCollection Directories => (DirectoryElementCollection)this["directories"];

        [ConfigurationCollection(typeof(DestinationElement), AddItemName = "rule")]
        [ConfigurationProperty("rules")]
        public DestinationElementCollection Destinations => (DestinationElementCollection)this["rules"];
    }
}
