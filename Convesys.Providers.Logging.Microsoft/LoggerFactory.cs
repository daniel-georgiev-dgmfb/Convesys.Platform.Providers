using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convesys.Providers.MicrosoftCore.Logging
{
    internal class LoggerFactory : Microsoft.Extensions.Logging.ILoggerFactory
    {
        public void AddProvider(Microsoft.Extensions.Logging.ILoggerProvider provider)
        {

        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new Microsoft.Extensions.Logging.Logger<LoggerFactory>(new LoggerFactory());
        }

        public void Dispose()
        {

        }
    }
}
