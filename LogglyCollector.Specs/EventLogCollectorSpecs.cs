using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using LogglyCollector.Configuration;
using LogglyCollector.EventLogs;
using SubSpec;

namespace LogglyCollector.Specs
{
    public class EventLogCollectorSpecs
    {
        [Specification]
        public void CanLogEventLogs()
        {
            var eventLogCollector = default(EventLogCollector);
            var logger = default(TestLogger);
            var testlog = default(EventLog);

            "Given I am collecting performance counter data"
                .Context(() =>
                {
                    if (EventLog.SourceExists("TestEventSource"))
                        EventLog.DeleteEventSource("TestEventSource");

                    EventLog.CreateEventSource("TestEventSource", "TestEventLog");
                    testlog = new EventLog("TestEventLog", ".", "TestEventSource");
                    logger = new TestLogger();

                    var logElement = new EventLogElement();
                    logElement.Name = "TestEventLog";

                    var eventLogConfiguration = new EventLogConfiguration();
                    eventLogConfiguration.EventLogs.Add(logElement);

                    eventLogCollector = new EventLogCollector(eventLogConfiguration);
                });
            "When I begin collecting".Do(() =>
            {
                eventLogCollector.Collect(logger);
                testlog.WriteEntry("This is a test", EventLogEntryType.Information);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                eventLogCollector.Dispose();
                EventLog.DeleteEventSource("TestEventSource");
            });

            "The event log item will be collected".Observation(() => logger.LogItems[0].Message.Should().Be("This is a test"));
        }
    }
}
