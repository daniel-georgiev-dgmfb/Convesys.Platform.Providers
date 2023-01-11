using Convesys.Kernel.Configuration;
using Convesys.Kernel.DependencyResolver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;

namespace Convesys.Providers.Logging.Debug
{
    public static class DebugLoggingExtensions
    {
        private const string LogLevel = "DEBUGLOGLEVEL";
        public static IDependencyResolver AddDebug(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<ILoggerProvider, DebugLoggerProvider>(Lifetime.Singleton);
            dependencyResolver.RegisterFactory<Func<string, LogLevel, bool>>(() =>
            (s, l) =>
            {
                try
                {
                    var configuration = dependencyResolver.Resolve<IConfiguration>();
                    if (configuration == null)
                        return false;
                    LogLevel level;
                    var levelVariable = configuration.GetValue<string>(DebugLoggingExtensions.LogLevel);
                    if (String.IsNullOrEmpty(levelVariable))
                        return false;
                    return Enum.TryParse<LogLevel>(levelVariable, true, out level) && l >= level;
                }
                catch(Exception)
                {
                    return false;
                }
            }, Lifetime.Singleton);
            return dependencyResolver;
        }
    }
}