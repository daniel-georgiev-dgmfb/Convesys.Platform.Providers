using Convesys.Kernel.Configuration;
using Convesys.Kernel.DependencyResolver;
using Convesys.Kernel.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using MS = Microsoft.Extensions.Logging;
namespace Convesys.Providers.Logging.Microsoft
{
    public static class LoggingExtensions
    {
        private const string MinLogLevel = "MINLOGLEVEL";
        public static IDependencyResolver AddLogging(this IDependencyResolver dependencyResolver)
        {
            if(!dependencyResolver.Contains<ILoggerFactory>())
                dependencyResolver.RegisterType<ILoggerFactory, LoggerFactory>(Lifetime.Singleton);
            if (!dependencyResolver.Contains(typeof(IEventLogger<>)))
                dependencyResolver.RegisterType(typeof(IEventLogger<>), typeof(Logger<>), Lifetime.Singleton);
            if (!dependencyResolver.Contains<IOptionsMonitor<LoggerFilterOptions>>())
                dependencyResolver.RegisterFactory<IOptionsMonitor<LoggerFilterOptions>>(() =>
                {
                    MS.LogLevel logLevel = MS.LogLevel.Information;
                    try
                    {
                        var configuration = dependencyResolver.Resolve<IConfiguration>();
                        var minLevel = configuration.GetValue<string>(LoggingExtensions.MinLogLevel);
                        if (String.IsNullOrWhiteSpace(minLevel) || !Enum.TryParse<MS.LogLevel>(minLevel, true, out logLevel))
                            logLevel = MS.LogLevel.Information;
                    }
                    catch (Exception)
                    {
                        logLevel = MS.LogLevel.Information;
                    }
                    var options = new LoggerFilterOptions
                    {
                        MinLevel = logLevel
                    };
                    return new LoggingOptionsMonitor(options);
                }, Lifetime.Singleton);
            return dependencyResolver;
        }
    }
}