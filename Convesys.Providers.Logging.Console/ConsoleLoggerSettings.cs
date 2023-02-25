using Twilight.Kernel.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Primitives;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Convesys.Providers.Logging.Console.Tests.L0")]
namespace Twilight.Providers.Logging.Console
{
    internal class ConsoleLoggerSettings : IConsoleLoggerSettings
    {
        internal const string LogLevel = "LOGLEVEL";

        private readonly IConfiguration _configuration;

        public bool IncludeScopes => true;

        public IChangeToken ChangeToken => (IChangeToken)null;

        public ConsoleLoggerSettings(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this._configuration = configuration;
        }

        public IConsoleLoggerSettings Reload()
        {
            return new ConsoleLoggerSettings(this._configuration);
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            level = Microsoft.Extensions.Logging.LogLevel.None;
            try
            {
                var levelVariable = this._configuration.GetValue<string>(ConsoleLoggerSettings.LogLevel);
                if (String.IsNullOrEmpty(levelVariable))
                    return false;

                var result = Enum.TryParse<LogLevel>(levelVariable, true, out level);
                if (!result)
                    level = Microsoft.Extensions.Logging.LogLevel.None;
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}