using System.Configuration;

namespace FileSystemWatcher.UI.Configuration
{
    public class DirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string DirectoryPath => (string)base["path"];
    }
}
