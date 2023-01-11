using Convesys.Common.Configuration;
using Convesys.Providers.Polly.CircuitBreaker;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convesys.Providers.CircuitBreaker.Polly.Tests.L0
{
    public class CircuitBreakerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Handle_if_the_exeption_is_the_specified_one()
        {
            //ARRANGE
            try
            {
                var configuration = new LocalStoreConfiguration();

                configuration.SetValue("exceptionsAllowedBeforeBreaking", 0.3);
                configuration.SetValue("durationOfBreak", 1);
                configuration.SetValue("minimumThroughput", 10);
                configuration.SetValue("failureThreshold", 0.5);
                var breaker = new PollyCircuitBreaker<NotImplementedException>(configuration);
                var failureCount = 0;
                var token = new CancellationToken();
                //PERFORM
                await breaker.ExecuteAsync(t => throw new NotImplementedException(), async e => failureCount++, token);
                //ASSERT
                Assert.AreEqual(1, failureCount);
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task Handle_if_the_exeption_is_the_specified_one1()
        {
            //ARRANGE
            try
            {
                var configuration = new LocalStoreConfiguration();

                configuration.SetValue("exceptionsAllowedBeforeBreaking", 0.3);
                configuration.SetValue("durationOfBreak", 1);
                configuration.SetValue("minimumThroughput", 10);
                configuration.SetValue("failureThreshold", 0.5);
                var breaker = new PollyCircuitBreaker<NotImplementedException>(configuration);
                var failureCount = 0;
                var token = new CancellationToken();
                //PERFORM
                while(failureCount < 10)
                    await breaker.ExecuteAsync(t => throw new MissingMemberException(), async e => failureCount++, token);
                //ASSERT
                Assert.AreEqual(1, failureCount);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}