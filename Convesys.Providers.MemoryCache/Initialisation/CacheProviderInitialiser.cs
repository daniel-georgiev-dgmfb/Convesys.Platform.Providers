using Convesys.Kernel.DependencyResolver;
using System.Threading.Tasks;

namespace Convesys.MemoryCacheProvider.Initialisation
{
    public static class CacheProviderInitialiser
    {
        public static Task AddMemoryCacheProvider(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<MemoryCacheRuntimeImplementor>(Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}