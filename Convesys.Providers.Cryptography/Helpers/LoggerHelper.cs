using System;

namespace Convesys.Platform.Cryptography.Helpers
{
    internal class LoggerHelper
    {
        static internal Func<object, Exception, string> Formatter
        {
            get
            {
                return (_, __) => _.ToString();
            }
        }
    }
}