using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using LogglyCollector.Configuration;
using LogglyCollector.PerfomanceCounters;
using SubSpec;

namespace LogglyCollector.Specs
{
    
    public class PerformanceCounterCollectorSpecs
    {
        [Specification]
        public void CanLogPerformanceData()
        {
          
            var counterCollector = default(PerformanceCounterCollector);
            var logger = default(TestLogger);
            var totalOperations = default(PerformanceCounter);

            "Given I am collecting performance counter data"
                .Context(() =>
                        {
                            if(PerformanceCounterCategory.Exists("TestCategory"))
                                PerformanceCounterCategory.Delete("TestCategory");

                            var counters = new CounterCreationDataCollection();
                            var totalOps = new CounterCreationData();
                            totalOps.CounterName = "# operations executed";
                            totalOps.CounterHelp = "Total number of operations executed";
                            totalOps.CounterType = PerformanceCounterType.NumberOfItems32;
                            counters.Add(totalOps);
                            PerformanceCounterCategory.Create("TestCategory", "Test category", PerformanceCounterCategoryType.SingleInstance, counters);

                            totalOperations = new PerformanceCounter();
                            totalOperations.CategoryName = "TestCategory";
                            totalOperations.CounterName = "# operations executed";
                            totalOperations.MachineName = ".";
                            totalOperations.ReadOnly = false;

                            logger = new TestLogger();

                            var counter = new PerformanceCounterElement();
                            counter.Name = "# operations executed";

                            var category = new PerformanceCounterCategoryElement();
                            category.Name = "TestCategory";
                            category.PerformanceCounters.Add(counter);

                            var collectorConfiguration = new CounterConfiguration();
                            collectorConfiguration.Categories.Add(category);
                            collectorConfiguration.CollectionFrequency = "1";

                            counterCollector = new PerformanceCounterCollector(collectorConfiguration);
                        });
            "When I begin collecting".Do(() =>
                        {
                            counterCollector.Collect(logger);
                            totalOperations.IncrementBy(20);
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                            counterCollector.Dispose();
                            PerformanceCounterCategory.Delete("TestCategory");
                        });
            "Performance data will be collected".Observation(() => logger.LogItems[0].Data["Value"].ToString().Should().Be("20"));

            "The performance category will be collected".Observation(() => logger.LogItems[0].Data["CategoryName"].Should().Be("TestCategory"));

            "The performance counter name will be collected".Observation(() => logger.LogItems[0].Data["CounterName"].Should().Be("# operations executed"));
        }
    }
}
