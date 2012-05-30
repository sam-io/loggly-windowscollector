namespace LogglyCollector.Configuration
{
    public class PerformanceCounterInstanceElement : NamedConfigurationElement
    {
        public PerformanceCounterInstanceElement()
        {
        }

        public PerformanceCounterInstanceElement(string name)
            : base(name)
        {
        }
    }
}
