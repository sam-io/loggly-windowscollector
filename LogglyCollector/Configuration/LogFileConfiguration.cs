using System.ComponentModel.Composition;
using System.Configuration;

namespace LogglyCollector.Configuration
{
    public class LogFileConfiguration : ConfigurationSection
    {

        [ConfigurationProperty("directories", IsDefaultCollection = false)]
        public DirectoryElementCollection Directories
        {
            get
            {
                var directoriesCollection = (DirectoryElementCollection)base["directories"];
                return directoriesCollection;
            }
        }

        [ConfigurationProperty("files", IsDefaultCollection = false)]
        public FileElementCollection Files
        {
            get
            {
                var filesCollection = (FileElementCollection)base["files"];
                return filesCollection;
            }
        }


        [Export("LogFileConfiguration", typeof(LogFileConfiguration))]
        public static LogFileConfiguration Instance
        {
            get
            {
                return (LogFileConfiguration)ConfigurationManager.GetSection("logFiles");
            }
        }

       

    }
}
