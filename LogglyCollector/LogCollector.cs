using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace LogglyCollector
{
    public partial class LogCollector : ServiceBase
    {
        
        private List<ICollector> _collectors;
 
        public LogCollector()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var inputKey = ConfigurationManager.AppSettings["inputKey"];
            var container = new CompositionContainer(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            _collectors = container.GetExportedValues<ICollector>().ToList();

            var logger = new Loggly.Logger(inputKey);
            foreach (var collector in _collectors)
                collector.Collect(logger);
        }

        protected override void OnStop()
        {
            foreach (var collector in _collectors)
                collector.Dispose();
        }

    }
}
