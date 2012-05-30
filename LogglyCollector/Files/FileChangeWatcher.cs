using System.Collections.Generic;
using System.IO;

namespace LogglyCollector.Files
{
    public class FileChangeWatcher : IChangeWatcher
    {

        private readonly string _path;
        private readonly WatchedFileFactory _watchedFileFactory;
        private readonly FileSystemWatcher _watcher;
        private readonly Dictionary<string, WatchedFile> _watchedFiles;

        public FileChangeWatcher(string path, WatchedFileFactory watchedFileFactory)
        {
            _path = path;
            _watchedFileFactory = watchedFileFactory;
            _watchedFiles = new Dictionary<string, WatchedFile>();
            _watcher = new FileSystemWatcher(path);
            _watcher.EnableRaisingEvents = true;

            _watcher.Deleted += WatcherChanged;
            _watcher.Renamed += WatcherChanged;
            _watcher.Created += WatcherChanged;
            _watcher.Changed += WatcherChanged;
        }

        private void WatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                string newName = Path.GetFileName(e.FullPath);
                HandleChange(e.Name, WatcherChangeTypes.Deleted);
                HandleChange(newName, WatcherChangeTypes.Created);
            }
            else
            {
                HandleChange(e.Name, e.ChangeType);
            }
        }

        private void HandleChange(string name, WatcherChangeTypes changeType)
        {
            WatchedFile file;
            if (_watchedFiles.TryGetValue(name.ToLower(), out file))
            {
                if (changeType == WatcherChangeTypes.Changed || changeType == WatcherChangeTypes.Created)
                    file.FileChanged();

                if (changeType == WatcherChangeTypes.Deleted)
                    file.Reset();
            }
        }

        public void CreateWatchedFile(string fileName)
        {
            var filePath = Path.Combine(_path, fileName);
            var watchedFile = _watchedFileFactory.Create(filePath);
            _watchedFiles.Add(fileName.ToLower(), watchedFile);
            
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
