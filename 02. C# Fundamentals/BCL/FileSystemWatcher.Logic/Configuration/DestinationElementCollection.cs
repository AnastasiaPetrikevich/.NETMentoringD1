using System.Configuration;

namespace FileSystemWatcher.Configuration
{
    public class DestinationElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultDirectory")]
        public string DefaultDirectory => (string)this["defaultDirectory"];

        protected override ConfigurationElement CreateNewElement() => new DestinationElement();

        protected override object GetElementKey(ConfigurationElement element) =>
            ((DestinationElement)element).SearchTemplate;
    }
}
