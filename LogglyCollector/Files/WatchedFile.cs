using System;
using System.IO;

namespace LogglyCollector.Files
{
    public class WatchedFile
    {
        private readonly string _path;
        private readonly Action<WatchedFile, string> _log;
        private long _filePos = 0;

        public WatchedFile(string path, Action<WatchedFile, string> log)
        {
            _path = path;
            _log = log;
            ReadToEnd();
        }

        public string FileName
        {
            get { return Path.GetFileName(_path); }
        }

        private void ReadToEnd()
        {
            if (File.Exists(_path))
            {
                using (var reader = new StreamReader(
                    new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)))
                {
                    reader.BaseStream.Seek(_filePos, SeekOrigin.Begin);

                    while (reader.Peek() != -1)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                            _log(this, line);
                    }
                    _filePos = reader.BaseStream.Position;
                }
            }
        }

        public void Reset()
        {
            _filePos = 0;
        }

        public void FileChanged()
        {
            ReadToEnd();
        }

    }
}
