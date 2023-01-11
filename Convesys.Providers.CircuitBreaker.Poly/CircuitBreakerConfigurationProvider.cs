using System;

namespace Convesys.Providers.Polly.CircuitBreaker
{
    public class CircuitBreakerConfigurationProvider
    {
        public int ExceptionsAllowedBeforeBreaking { get; set; }
        public TimeSpan DurationOfBreak { get; internal set; }
    }
}