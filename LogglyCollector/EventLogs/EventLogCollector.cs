using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Loggly;
using LogglyCollector.Configuration;

namespace LogglyCollector.EventLogs
{

    [Export(typeof(ICollector))]
    public class EventLogCollector : ICollector
    {

        private readonly List<EventLog> _eventLogs = new List<EventLog>();
        private ILogger _logger;

        [ImportingConstructor]
        public EventLogCollector([Import("EventLogConfiguration")]EventLogConfiguration eventLogConfiguration)
        {
            if (eventLogConfiguration!=null)
            {
                _eventLogs.AddRange(eventLogConfiguration.EventLogs.Select(l => new EventLog(l.Name)));

                foreach (var eventLog in _eventLogs)
                {
                    EventLog log = eventLog;
                    eventLog.EnableRaisingEvents = true;
                    eventLog.EntryWritten += (s, e) => LogEntryWritten(log.LogDisplayName, e);
                }
            }
        }

        public void Collect(ILogger logger)
        {
            _logger = logger;
        }

        private void LogEntryWritten(string logDisplayName, EntryWrittenEventArgs e)
        {            
            var data = new Dictionary<string, object>();
            data["LogName"] = logDisplayName;
            data["LogKind"] = "EventLog";
            _logger.Log(e.Entry.Message, e.Entry.EntryType.ToString(), data);
        }

        public void Dispose()
        {
            foreach (var eventLog in _eventLogs)
                eventLog.Dispose();
        }
    }
}
