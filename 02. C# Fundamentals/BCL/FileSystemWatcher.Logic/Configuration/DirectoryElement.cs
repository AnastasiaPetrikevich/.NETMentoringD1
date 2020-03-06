using System.Configuration;

namespace FileSystemWatcher.Configuration
{
    public class DirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string DirectoryPath => (string)base["path"];
    }
}
