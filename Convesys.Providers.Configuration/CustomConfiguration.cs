using System;

namespace Pirina.Providers.Configuration
{
    public class CustomConfiguration : IConfiguration
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _innerConfiguration;
        public CustomConfiguration(Microsoft.Extensions.Configuration.IConfiguration innerConfiguration)
        {
            this._innerConfiguration = innerConfiguration;
        }
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public T GetValue<T>(string key)
        {
            return Microsoft.Extensions.Configuration.ConfigurationBinder.GetValue<T>(this._innerConfiguration, key);
        }

        public void SetValue(string key, object value)
        {
            Microsoft.Extensions.Configuration.ConfigurationBinder.Bind(this._innerConfiguration, key, value);
        }
    }
}
