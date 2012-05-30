namespace LogglyCollector.Configuration
{
    public class DirectoryElementCollection : ConfigurationElementCollection<DirectoryElement>
    {
        protected override string ElementName
        {
            get
            {
                return "directory";
            }
        }

       
    }
}
