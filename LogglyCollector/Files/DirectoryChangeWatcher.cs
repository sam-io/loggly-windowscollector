using System.Collections.Generic;
using System.IO;

namespace LogglyCollector.Files
{
    public class DirectoryChangeWatcher : IChangeWatcher
    {

        private readonly string _path;
        private readonly WatchedFileFactory _watchedFileFactory;
        private readonly FileSystemWatcher _watcher;
        private readonly Dictionary<string, WatchedFile> _watchedFiles;

        public DirectoryChangeWatcher(string path, string filter, WatchedFileFactory watchedFileFactory)
        {
            _path = path;
            _watchedFileFactory = watchedFileFactory;
            _watchedFiles = new Dictionary<string, WatchedFile>();

            foreach (var file in Directory.GetFiles(path, filter))
            {
                _watchedFiles.Add(Path.GetFileName(file).ToLower(), _watchedFileFactory.Create(file));
            }

            _watcher = new FileSystemWatcher(path, filter);
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
                if (changeType == WatcherChangeTypes.Created || changeType == WatcherChangeTypes.Changed)
                    file.FileChanged();

                if (changeType == WatcherChangeTypes.Deleted)
                    _watchedFiles.Remove(name.ToLower());
            }
            else
            {
                if (changeType == WatcherChangeTypes.Created || changeType == WatcherChangeTypes.Changed)
                    _watchedFiles.Add(name.ToLower(), _watchedFileFactory.Create(Path.Combine(_path, name)));
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
