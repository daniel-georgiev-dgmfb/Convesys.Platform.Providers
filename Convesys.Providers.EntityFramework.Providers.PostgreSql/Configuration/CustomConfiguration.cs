using Microsoft.EntityFrameworkCore;
using Npgsql;
using Pirina.Kernel.Data;
using Pirina.Kernel.Data.Connection;
using Pirina.Kernel.Data.ORM;
using Pirina.Kernel.Data.Tenancy;
using Pirina.Kernel.Reflection.Reflection;
using Pirina.Providers.EntityFramework.Providers.PostgreSql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Pirina.Providers.EntityFramework.Providers.PostgreSql.Tests.L1")]
namespace Pirina.Providers.EntityFramework.Configuration
{
    internal class CustomConfiguration : DbConfiguration, IDbCustomConfiguration
    {
        public ICollection<ISeeder> Seeders => Enumerable.Empty<ISeeder>().ToList();

        public Func<IEnumerable<Type>> ModelsFactory => () =>
        {
            var types = ReflectionHelper.GetAllTypes(t => t.IsAssignableFrom(typeof(BaseModel)) && !t.IsInterface && !t.IsAbstract && t != typeof(object))
            .Union(new[] { typeof(TestModel) });
            return types;
        };

        public Func<ITenantManager> TenantManager => throw new NotImplementedException();

        public Func<IEnumerable<IDbMapper>> ModelMappers => () => Enumerable.Empty<IDbMapper>();

        public string Schema => throw new NotImplementedException();

        public string ModelKey => "pirina.Postgre";

        private IConnectionStringProvider<NpgsqlConnectionStringBuilder> _connectionStringProvider;
        public CustomConfiguration(IConnectionStringProvider<NpgsqlConnectionStringBuilder> connectionStringProvider)
        {
            this._connectionStringProvider = connectionStringProvider;
            base.SetProviderFactory("postgre", NpgsqlFactory.Instance);
            base.SetProviderServices("postgre", NpgsqlServices.Instance);
        }
        public virtual async Task ConfigureOptions<T>(T options)
        {
            var optionsBuilder = options as DbContextOptionsBuilder;
            var connectionStringProvider = this._connectionStringProvider.GetConnectionString();
            optionsBuilder.UseNpgsql(connectionStringProvider.ConnectionString);
            ProxiesExtensions.UseLazyLoadingProxies(optionsBuilder); 
        }
    }
}
