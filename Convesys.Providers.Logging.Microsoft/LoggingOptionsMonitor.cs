using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Convesys.Providers.Logging.Microsoft
{
    internal class LoggingOptionsMonitor : IOptionsMonitor<LoggerFilterOptions>
    {
        public LoggingOptionsMonitor(LoggerFilterOptions currentValue)
        {
            this.CurrentValue = currentValue;
        }

        public LoggerFilterOptions CurrentValue { get; }

        public LoggerFilterOptions Get(string name)
        {
            return this.CurrentValue;
        }

        public IDisposable OnChange(Action<LoggerFilterOptions, string> listener)
        {
            return (IDisposable)null;
        }
    }
}