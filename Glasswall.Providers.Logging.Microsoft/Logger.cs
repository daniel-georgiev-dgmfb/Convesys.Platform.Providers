using System;
using Glasswall.Kernel.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using MsLogging = Microsoft.Extensions.Logging;

namespace Glasswall.Providers.Logging.Microsoft
{
    public class Logger<T> : IGWLogger<T>
    {
        private readonly ILogger _logger;

        public Logger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            this._logger = loggerFactory.CreateLogger(TypeNameHelper.GetTypeDisplayName(typeof(T)));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this._logger.BeginScope<TState>(state);
        }

        public bool IsEnabled(Kernel.Logging.LogLevel logLevel)
        {
            var logLevelInner = this.GetLogLevel(logLevel);
            return this.IsEnabledInternal(logLevelInner);
        }

        public void Log<TState>(Kernel.Logging.LogLevel logLevel, Kernel.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logLevelInner = this.GetLogLevel(logLevel);
            if (!this.IsEnabledInternal(logLevelInner))
                return;
            var innerEventId = new MsLogging.EventId(eventId.Id, eventId.Name);
            this._logger.Log<TState>(logLevelInner, innerEventId, state, exception, formatter);
        }

        private bool IsEnabledInternal(MsLogging.LogLevel logLevel)
        {
            return this._logger.IsEnabled(logLevel);
        }

        private MsLogging.LogLevel GetLogLevel(Kernel.Logging.LogLevel logLevel)
        {
            switch(logLevel)
            {
                case Kernel.Logging.LogLevel.Critical:
                    return MsLogging.LogLevel.Critical;
                case Kernel.Logging.LogLevel.Debug:
                    return MsLogging.LogLevel.Debug;
                case Kernel.Logging.LogLevel.Error:
                    return MsLogging.LogLevel.Error;
                case Kernel.Logging.LogLevel.Information:
                    return MsLogging.LogLevel.Information;
                case Kernel.Logging.LogLevel.Trace:
                    return MsLogging.LogLevel.Trace;
                case Kernel.Logging.LogLevel.Warning:
                    return MsLogging.LogLevel.Warning;
                default:
                    return MsLogging.LogLevel.None;
            }
        }
    }
}