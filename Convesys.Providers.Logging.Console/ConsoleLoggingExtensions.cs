using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Twilight.Kernel.DependencyResolver;

namespace Twilight.Providers.Logging.Console
{
    public static class ConsoleLoggingExtensions
    {
        public static IDependencyResolver AddConsole(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<ILoggerProvider, ConsoleLoggerProvider>(Lifetime.Singleton);
            dependencyResolver.RegisterType<IConsoleLoggerSettings, ConsoleLoggerSettings> (Lifetime.Singleton);
            return dependencyResolver;
        }
    }
}