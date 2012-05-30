namespace LogglyCollector.Configuration
{
    public class EventLogElementCollection : ConfigurationElementCollection<EventLogElement>
    {
        protected override string ElementName
        {
            get
            {
                return "log";
            }
        }

       
    }
}
