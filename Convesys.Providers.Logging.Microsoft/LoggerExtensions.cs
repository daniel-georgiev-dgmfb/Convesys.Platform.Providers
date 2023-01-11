using Convesys.Kernel.Logging;
using System;

namespace Convesys.Providers.Logging.Microsoft
{
    public static class LoggerExtensions
    {
        private static readonly Func<object, Exception, string> _messageFormatter = new Func<object, Exception, string>(LoggerExtensions.MessageFormatter);

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogDebug(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Debug, eventId, exception, message, args);
        }

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
        public static void LogDebug(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Debug, eventId, message, args);
        }

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
        public static void LogDebug(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Debug, exception, message, args);
        }

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
        public static void LogDebug(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Debug, message, args);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogTrace(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Trace, eventId, exception, message, args);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
        public static void LogTrace(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Trace, eventId, message, args);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
        public static void LogTrace(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Trace, exception, message, args);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
        public static void LogTrace(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Trace, message, args);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogInformation(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Info, eventId, exception, message, args);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
        public static void LogInformation(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Info, eventId, message, args);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
        public static void LogInformation(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Info, exception, message, args);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
        public static void LogInformation(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Info, message, args);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogWarning(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Warning, eventId, exception, message, args);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
        public static void LogWarning(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Warning, eventId, message, args);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
        public static void LogWarning(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Warning, exception, message, args);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
        public static void LogWarning(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Warning, message, args);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogError(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Error, eventId, exception, message, args);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
        public static void LogError(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Error, eventId, message, args);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
        public static void LogError(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Error, exception, message, args);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogError("Processing request from {Address}", address)</example>
        public static void LogError(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Error, message, args);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
        public static void LogCritical(this IEventLogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Critical, eventId, exception, message, args);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
        public static void LogCritical(this IEventLogger logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Critical, eventId, message, args);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
        public static void LogCritical(this IEventLogger logger, Exception exception, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Critical, exception, message, args);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
        public static void LogCritical(this IEventLogger logger, string message, params object[] args)
        {
            logger.Log(SeverityLevel.Critical, message, args);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void Log(this IEventLogger logger, SeverityLevel logLevel, string message, params object[] args)
        {
            logger.Log(logLevel, (EventId)0, (Exception)null, message, args);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void Log(this IEventLogger logger, SeverityLevel logLevel, EventId eventId, string message, params object[] args)
        {
            logger.Log(logLevel, eventId, (Exception)null, message, args);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void Log(this IEventLogger logger, SeverityLevel logLevel, Exception exception, string message, params object[] args)
        {
            logger.Log(logLevel, (EventId)0, exception, message, args);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void Log(this IEventLogger logger, SeverityLevel logLevel, EventId eventId, Exception exception, string message, params object[] args)
        {
            throw new NotImplementedException();
            //if (logger == null)
            //    throw new ArgumentNullException(nameof(logger));
            //logger.Log(logLevel, eventId, (object)new FormattedLogValues(message, args), exception, LoggerExtensions._messageFormatter);
        }

        /// <summary>Formats the message and creates a scope.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.IGWLogger" /> to create the scope in.</param>
        /// <param name="messageFormat">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A disposable scope object. Can be null.</returns>
        /// <example>
        /// using(logger.BeginScope("Processing request from {Address}", address))
        /// {
        /// }
        /// </example>
        public static IDisposable BeginScope(this IEventLogger logger, string messageFormat, params object[] args)
        {
            throw new NotImplementedException();
            //if (logger == null)
            //    throw new ArgumentNullException(nameof(logger));
            //return logger.BeginScope<FormattedLogValues>(new FormattedLogValues(messageFormat, args));
        }

        private static string MessageFormatter(object state, Exception error)
        {
            return state.ToString();
        }
    }
}
