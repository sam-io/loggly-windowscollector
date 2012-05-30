namespace LogglyCollector.Configuration
{
    public class PerformanceCountersCollection : ConfigurationElementCollection<PerformanceCounterElement>
    {

        protected override string ElementName
        {
            get { return "performanceCounter"; }
        }

    }

}
