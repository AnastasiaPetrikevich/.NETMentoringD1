using System.Configuration;

namespace FileSystemWatcher.Configuration
{
    public class DirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new DirectoryElement();

        protected override object GetElementKey(ConfigurationElement element) =>
            ((DirectoryElement)element).DirectoryPath;
    }
}
