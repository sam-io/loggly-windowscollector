using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class PerformanceCounterElement : NamedConfigurationElement
    {

        public PerformanceCounterElement()
        {
        }

        public PerformanceCounterElement(string name)
        {
            this.Name = name;
        }


        [ConfigurationProperty("instanceName", IsRequired = false)]
        public string Instance
        {
            get
            {
                return (string)this["instanceName"];
            }
            set
            {
                this["instanceName"] = value;
            }
        }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection=true)]
        public PerformanceCounterInstanceElementCollection Instances
        {
            get
            {
                return (PerformanceCounterInstanceElementCollection)this[""];
            }
        }

      

    }
}
