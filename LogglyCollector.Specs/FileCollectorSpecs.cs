using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using LogglyCollector.Configuration;
using LogglyCollector.Files;
using SubSpec;

namespace LogglyCollector.Specs
{
    public class FileCollectorSpecs
    {

        [Specification]
        public void CanCollectLogFiles()
        {
            var fileCollector = default(FileCollector);
            var logger = default(TestLogger);
            var filePath = default(string);

            "Given I am collecting performance counter data"
                .Context(() =>
                        {
                            logger = new TestLogger();

                            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFile.txt");

                            var fileElement = new FileElement();
                            fileElement.Name = filePath;

                            var fileConfiguration = new LogFileConfiguration();
                            fileConfiguration.Files.Add(fileElement);

                            fileCollector = new FileCollector(fileConfiguration);
                        });
            "When I begin collecting".Do(() =>
            {
                fileCollector.Collect(logger);

                File.WriteAllLines(filePath, new string[] { "This is a test"});

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                File.Delete(filePath);
            });
            "The file content will be collected".Observation(() => logger.LogItems[0].Message.Should().Be("This is a test"));
        }

    }
}
