using System.Configuration;

namespace FileSystemWatcher.UI.Configuration
{
    public class DestinationElement : ConfigurationElement
    {
        [ConfigurationProperty("searchTemplate", IsKey = true)]
        public string SearchTemplate => (string)base["searchTemplate"];

        [ConfigurationProperty("destinationDirectory")]
        public string DestinationDirectory => (string)base["destinationDirectory"];

        [ConfigurationProperty("isNumberRequired")]
        public bool IsNumberRequired => (bool)base["isNumberRequired"];

        [ConfigurationProperty("isDateRequired")]
        public bool IsDateRequired => (bool)base["isDateRequired"];
    }
}
