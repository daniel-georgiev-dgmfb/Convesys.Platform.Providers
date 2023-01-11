using Convesys.Kernel.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MsLogging = Microsoft.Extensions.Logging;

namespace Convesys.Providers.Logging.Microsoft
{
    public class Logger<T> : IEventLogger<T>, ILogger
    {
        private readonly ILogger _logger;

        public Logger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));
            
            this._logger = loggerFactory.CreateLogger(typeof(T).FullName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this._logger.BeginScope<TState>(state);
        }

        public bool IsEnabled(Kernel.Logging.SeverityLevel logLevel)
        {
            var logLevelInner = this.GetLogLevel(logLevel);
            return this.IsEnabledInternal(logLevelInner);
        }

        public void Log<TState>(Kernel.Logging.SeverityLevel logLevel, Kernel.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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

        private MsLogging.LogLevel GetLogLevel(Kernel.Logging.SeverityLevel logLevel)
        {
            switch(logLevel)
            {
                case Kernel.Logging.SeverityLevel.Critical:
                    return MsLogging.LogLevel.Critical;
                case Kernel.Logging.SeverityLevel.Debug:
                    return MsLogging.LogLevel.Debug;
                case Kernel.Logging.SeverityLevel.Error:
                    return MsLogging.LogLevel.Error;
                case Kernel.Logging.SeverityLevel.Info:
                    return MsLogging.LogLevel.Information;
                case Kernel.Logging.SeverityLevel.Trace:
                    return MsLogging.LogLevel.Trace;
                case Kernel.Logging.SeverityLevel.Warning:
                    return MsLogging.LogLevel.Warning;
                default:
                    return MsLogging.LogLevel.None;
            }
        }

        public async Task Log(SeverityLevel level, Kernel.Logging.EventId eventId, Type eventSource, Guid transactionId, string message)
        {
            
        }

        public async Task Log(SeverityLevel level, Kernel.Logging.EventId eventId, Type eventSource, string message)
        {
            
        }

        public async Task Log(SeverityLevel level, Kernel.Logging.EventId eventId, Type eventSource, Guid transactionId, Exception exception)
        {
            
        }

        public async Task Log(SeverityLevel level, Kernel.Logging.EventId eventId, Type eventSource, Exception exception)
        {
            
        }

        public void Log<TState>(LogLevel logLevel, MsLogging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }
    }
}