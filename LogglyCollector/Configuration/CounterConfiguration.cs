using System.ComponentModel.Composition;
using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class CounterConfiguration : ConfigurationSection
    {

        [ConfigurationProperty("collectionFrequency", IsRequired = false)]
        public string CollectionFrequency
        {
            get
            {
                return (string)this["collectionFrequency"];
            }
            set
            {
                this["collectionFrequency"] = value;
            }
        }

        [ConfigurationProperty("categories", IsDefaultCollection = false)]
        public PerformanceCounterCategoryElementCollection Categories
        {
            get
            {
                var countersCollection = (PerformanceCounterCategoryElementCollection)base["categories"];
                return countersCollection;
            }
        }

        [Export("CounterConfiguration", typeof(CounterConfiguration))]
        public static CounterConfiguration Instance
        {
            get
            {
                return (CounterConfiguration)ConfigurationManager.GetSection("performanceCounters");
            }
        }

       
       

    }
}
