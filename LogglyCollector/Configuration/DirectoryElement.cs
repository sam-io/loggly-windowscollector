using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class DirectoryElement : NamedConfigurationElement
    {
        [ConfigurationProperty("filter", IsRequired = true)]
        public string Filter
        {
            get
            {
                return (string)this["filter"];
            }
            set
            {
                this["filter"] = value;
            }
        }
    }
}
