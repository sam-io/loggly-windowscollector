using System;
using System.Collections.Generic;
using System.IO;

namespace LogglyCollector.Files
{
    public class LogWatcher : IDisposable
    {
        private readonly WatchedFileFactory _watchedFileFactory;
        private readonly Dictionary<string, IChangeWatcher> _watchers = new Dictionary<string, IChangeWatcher>();

        public LogWatcher(WatchedFileFactory watchedFileFactory)
        {
            _watchedFileFactory = watchedFileFactory;
        }

        public void WatchFile(string path)
        {
            IChangeWatcher changeWatcher;
            string watchPath = Path.GetDirectoryName(path).ToLower();

            if (!_watchers.TryGetValue(watchPath, out changeWatcher))
            {
                changeWatcher = new FileChangeWatcher(watchPath, _watchedFileFactory);
                _watchers.Add(watchPath, changeWatcher);
            }

            var fileChangeWatcher = changeWatcher as FileChangeWatcher;
            if (fileChangeWatcher!=null)
                fileChangeWatcher.CreateWatchedFile(Path.GetFileName(path));
        }

        public void WatchDirectory(string path, string filter)
        {
            IChangeWatcher changeWatcher;
            string watchPath = path.ToLower();

            if (!_watchers.TryGetValue(watchPath, out changeWatcher))
            {
                changeWatcher = new DirectoryChangeWatcher(watchPath, filter, _watchedFileFactory);
                _watchers.Add(watchPath, changeWatcher);
            }

            var directoryChangeWatcher = changeWatcher as DirectoryChangeWatcher;
            if (directoryChangeWatcher == null)
            {
                if(changeWatcher!=null)
                    changeWatcher.Dispose();

                _watchers[watchPath] = new DirectoryChangeWatcher(watchPath, filter, _watchedFileFactory);
            }
        }

        public void Dispose()
        {
            foreach (var changeWatcher in _watchers.Values)
            {
                changeWatcher.Dispose();
            }
        }
    }
}
