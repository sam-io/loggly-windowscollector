using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
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
            var container = new CompositionContainer(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            _collectors = container.GetExportedValues<ICollector>().ToList();

            var logger = new Loggly.Logger("ea0b7c89-8f9d-4ef1-9d8e-18bbce79f260");
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
