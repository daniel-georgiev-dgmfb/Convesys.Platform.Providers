using Pirina.Kernel.DependencyResolver;
using Pirina.Providers.Transport.AzureServiceBus.Providers;
using Pirina.Providers.Transport.AzureServiceBus.Transport;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Pirina.Providers.Transport.AzureServiceBus.Initialisation
{
    [ExcludeFromCodeCoverage]
    public static class AzureServiceBusTransportProviderInitialiser
    {
        public static Task InitialiseInternal(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<AzureServiceBusReadOnlyProvider>(Lifetime.Singleton);
            dependencyResolver.RegisterType<AzureServiceBusReadWriteProvider>(Lifetime.Singleton);
            dependencyResolver.RegisterType<AzureServiceBusWriteOnlyProvider>(Lifetime.Singleton);

            dependencyResolver.RegisterType<ServiceBusListener>(Lifetime.Transient);
            dependencyResolver.RegisterType<ServiceBusTransportManager>(Lifetime.Transient);
            dependencyResolver.RegisterType<ServiceBusTransportDispatcher>(Lifetime.Transient);
            dependencyResolver.RegisterType<ServiceBusTransport>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}