using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Loggly;
using Loggly.Responses;

namespace LogglyCollector.Specs
{

    public class LogItem
    {
        public string Message { get; set; } 
        public string Category { get; set; } 
        public IDictionary<string, object> Data { get; set; }
    }

    public class TestLogger : ILogger
    {

        private readonly List<LogItem> _logItems = new List<LogItem>();

        public ReadOnlyCollection<LogItem> LogItems
        {
            get { return new ReadOnlyCollection<LogItem>(_logItems); }
        }

        public LogResponse LogSync(string message)
        {
            LogResponse response = null;
            Log(message, (r)=>response = r);
            return response;
        }

        public void Log(string message)
        {
            Log(message, null, null);
        }

        public void Log(string message, Action<LogResponse> callback)
        {
            Log(message, null, null);
            callback(new LogResponse());
        }

        public void Log(string message, string category)
        {
            Log(message, category, null);
        }

        public void Log(string message, string category, IDictionary<string, object> data)
        {
            _logItems.Add(new LogItem()
                {
                    Category = category,
                    Message = message,
                    Data = data
                });
        }

        public void LogInfo(string message)
        {
            LogInfo(message, null);
        }

        public void LogInfo(string message, IDictionary<string, object> data)
        {
            Log(message, "Info", data);
        }

        public void LogVerbose(string message)
        {
            LogVerbose(message, null);
        }

        public void LogVerbose(string message, IDictionary<string, object> data)
        {
            Log(message, "Verbose", data);
        }

        public void LogWarning(string message)
        {
            LogWarning(message, null);
        }

        public void LogWarning(string message, IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message, Exception ex)
        {
            LogError(message, ex, null);
        }

        public void LogError(string message, Exception ex, IDictionary<string, object> data)
        {
            data["Exception"] = ex;
            Log(message, "Error", data);
        }
    }
}
