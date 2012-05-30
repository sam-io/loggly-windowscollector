using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class PerformanceCounterCategoryElement : NamedConfigurationElement
    {

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PerformanceCountersCollection PerformanceCounters
        {
            get
            {
                PerformanceCountersCollection countersCollection = (PerformanceCountersCollection)base[""];
                return countersCollection;
            }
        }

    }
}
