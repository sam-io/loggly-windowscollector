using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class NamedConfigurationElement : ConfigurationElement
    {

        public NamedConfigurationElement()
        {
        }

        public NamedConfigurationElement(string name)
        {
            this.Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}
