using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;

namespace LogglyCollector.Configuration
{
    public class EventLogConfiguration : ConfigurationSection
    {

        [ConfigurationProperty("logs", IsDefaultCollection = false)]
        public EventLogElementCollection EventLogs
        {
            get
            {
                var eventLogs = (EventLogElementCollection)base["logs"];
                return eventLogs;
            }
        }

        [Export("EventLogConfiguration", typeof(EventLogConfiguration))]
        public static EventLogConfiguration Instance
        {
            get
            {
                return (EventLogConfiguration)ConfigurationManager.GetSection("eventLogs");
            }
        }

     
    }
}
