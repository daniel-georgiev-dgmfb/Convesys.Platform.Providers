
namespace Pirina.Providers.EntityFramework.CustomServices
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    internal class CustomModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            return ((IDbContext) context).CustomConfiguration.ModelKey;
        }
    }
}