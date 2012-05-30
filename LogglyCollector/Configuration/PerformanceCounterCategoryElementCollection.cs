namespace LogglyCollector.Configuration
{
    public class PerformanceCounterCategoryElementCollection : ConfigurationElementCollection<PerformanceCounterCategoryElement>
    {
        protected override string ElementName
        {
            get
            {
                return "category";
            }
        }

       
    }
}
