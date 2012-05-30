namespace LogglyCollector.Configuration
{
    public class PerformanceCounterInstanceElementCollection : ConfigurationElementCollection<PerformanceCounterInstanceElement>
    {
        protected override string ElementName
        {
            get
            {
                return "instance";
            }
        }
    }
}
