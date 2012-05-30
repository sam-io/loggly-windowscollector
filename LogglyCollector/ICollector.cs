using System;
using Loggly;

namespace LogglyCollector
{
    public interface ICollector : IDisposable
    {
        void Collect(ILogger logger);
    }
}