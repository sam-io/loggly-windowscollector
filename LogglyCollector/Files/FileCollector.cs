using System.Collections.Generic;
using System.ComponentModel.Composition;
using Loggly;
using LogglyCollector.Configuration;

namespace LogglyCollector.Files
{
    [Export(typeof(ICollector))]
    public class FileCollector : ICollector
    {
        private readonly LogFileConfiguration _logFileConfiguration;

        private ILogger _logger;
        private LogWatcher _watcher;

        [ImportingConstructor]
        public FileCollector([Import("LogFileConfiguration")]LogFileConfiguration logFileConfiguration)
        {
            _logFileConfiguration = logFileConfiguration;
        }

        public void Collect(ILogger logger)
        {
            _logger = logger;
            var watcherFctory = new WatchedFileFactory(LogFileChange);
            _watcher = new LogWatcher(watcherFctory);

            if (_logFileConfiguration!=null)
            {
                foreach (var directory in _logFileConfiguration.Directories)
                    _watcher.WatchDirectory(directory.Name, directory.Filter);

                foreach (var file in _logFileConfiguration.Files)
                    _watcher.WatchFile(file.Name);
            }
        }

        private void LogFileChange(WatchedFile file, string line)
        {
            var data = new Dictionary<string, object>();
            data["FileName"] = file.FileName;
            data["LogKind"] = "Log File";
            _logger.LogInfo(line, data);
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
