using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Loggly;
using LogglyCollector.Configuration;

namespace LogglyCollector.PerfomanceCounters
{
    [Export(typeof(ICollector))]
    public class PerformanceCounterCollector : ICollector
    {

        private readonly int _collectionFrequency;
        private readonly List<PerformanceCounter> _counters = new List<PerformanceCounter>();
        private Timer _collectionTimer;

        [ImportingConstructor]
        public PerformanceCounterCollector([Import("CounterConfiguration")]CounterConfiguration counterConfiguration)
        {
            if (counterConfiguration!=null)
            {
                if (!int.TryParse(counterConfiguration.CollectionFrequency, out _collectionFrequency))
                    _collectionFrequency = 30;

                var counters = counterConfiguration.Categories
                    .SelectMany(
                        c => c.PerformanceCounters.Select(p => new { Category = c.Name, Counter = p }))
                    .SelectMany(
                        p =>
                        {
                            if (p.Counter.Instances.Any())
                                return p.Counter.Instances.Select(i => new { p.Category, Counter = p.Counter.Name, Instance = i.Name });
                            else
                                return new[] { new { p.Category, Counter = p.Counter.Name, p.Counter.Instance } };
                        });


                _counters.AddRange(
                    counters.Select(c => new PerformanceCounter(c.Category, c.Counter, c.Instance, true)));
            }
        }

        public void Collect(ILogger logger)
        {
            _collectionTimer = new Timer(CollectAll, logger, TimeSpan.FromSeconds(_collectionFrequency), TimeSpan.FromSeconds(10));   
        }

        private void CollectAll(object state)
        {
            var logger = (ILogger) state;
            foreach (var performanceCounter in _counters)
            {
                var data = new Dictionary<string, object>();
                data["LogKind"] = "Performance Counter";
                data["CategoryName"] = performanceCounter.CategoryName;
                data["CounterName"] = performanceCounter.CounterName;
                data["Value"] = performanceCounter.NextValue();

                logger.LogInfo("Performance Data", data);
            }
        }

        public void Dispose()
        {
            _collectionTimer.Dispose();
            foreach (var performanceCounter in _counters)
                performanceCounter.Dispose();
        }
    }
}
