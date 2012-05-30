namespace LogglyCollector.Configuration
{
    public class FileElementCollection : ConfigurationElementCollection<FileElement>
    {
        protected override string ElementName
        {
            get
            {
                return "file";
            }
        }

       
    }
}
