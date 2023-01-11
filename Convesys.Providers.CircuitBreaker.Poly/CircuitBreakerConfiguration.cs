using Convesys.Kernel.Configuration;
using System;

namespace Convesys.Providers.Polly.CircuitBreaker
{
    public class CircuitBreakerConfiguration : IConfiguration
    {
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}