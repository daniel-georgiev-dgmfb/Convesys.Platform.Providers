using Convesys.Kernel.Configuration;
using Convesys.Kernel.Resilience;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convesys.Providers.Polly.CircuitBreaker
{
    public class PollyCircuitBreaker<TException> : ICircuitBreaker where TException : Exception
    {
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private IConfiguration _configuration;
        public event EventHandler<CircuitBreakerOpenedEventArgs> CircuitBreakerOpened;
        public event EventHandler CircuitBreakerReset;

        public PollyCircuitBreaker(IConfiguration configuration)
        {
            this._configuration = configuration;
            var exceptionsAllowedBeforeBreaking = this._configuration.GetValue<double>("exceptionsAllowedBeforeBreaking");
            var durationOfBreak = TimeSpan.FromSeconds(_configuration.GetValue<int>("durationOfBreak"));
            var minimumThroughput = this._configuration.GetValue<int>("minimumThroughput");

            this._circuitBreakerPolicy = Policy
                .Handle<TException>()
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: exceptionsAllowedBeforeBreaking,
                    samplingDuration: durationOfBreak,
                    minimumThroughput: minimumThroughput,
                    durationOfBreak: durationOfBreak,
                    onBreak: OnBreak,
                    onReset: OnReset);
        }

        private void OnBreak(Exception exception, TimeSpan duration)
        {
            CircuitBreakerOpened?.Invoke(this, new CircuitBreakerOpenedEventArgs(exception, duration));
        }

        private void OnReset()
        {
            CircuitBreakerReset?.Invoke(this, new EventArgs());
        }

        public async Task ExecuteAsync(Func<CancellationToken, Task> executeAction, Func<Exception, Task> failureAction, CancellationToken cancellationToken)
        {
            if (executeAction == null)
                throw new ArgumentNullException(nameof(executeAction), "an execute action must be specified");
            if (failureAction == null)
                throw new ArgumentNullException(nameof(failureAction), "a failure action must be specified");

            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(executeAction, cancellationToken);
            }
            catch (Exception e)
            {
                await failureAction(e);
            }
        }
    }
}