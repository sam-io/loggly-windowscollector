using System;

namespace LogglyCollector.Files
{
    public class WatchedFileFactory
    {
        private readonly Action<WatchedFile, string> _log;

        public WatchedFileFactory(Action<WatchedFile, string> log)
        {
            _log = log;
        }

        public WatchedFile Create(string path)
        {
            return new WatchedFile(path, _log);
        }
    }
}
