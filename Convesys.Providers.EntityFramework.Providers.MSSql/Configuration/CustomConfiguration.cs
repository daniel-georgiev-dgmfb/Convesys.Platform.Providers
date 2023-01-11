using Microsoft.EntityFrameworkCore;
using Pirina.Kernel.Data.Connection;
using Pirina.Kernel.Data.ORM;
using Pirina.Kernel.Data.Tenancy;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Pirina.Providers.EntityFramework.Configuration
{
    internal class CustomConfiguration : DbConfiguration, IDbCustomConfiguration
    {
        private IConnectionStringProvider<SqlConnectionStringBuilder> _connectionStringProvider;
        public CustomConfiguration(IConnectionStringProvider<SqlConnectionStringBuilder> connectionStringProvider)
        {
            this._connectionStringProvider = connectionStringProvider;
        }
        public ICollection<ISeeder> Seeders => new List<ISeeder>();

        public Func<IEnumerable<Type>> ModelsFactory => throw new NotImplementedException();

        public Func<ITenantManager> TenantManager => throw new NotImplementedException();

        public Func<IEnumerable<IDbMapper>> ModelMappers => () => new List<IDbMapper>();

        public string Schema => throw new NotImplementedException();

        public string ModelKey => "pirina.EF";

        public virtual async Task ConfigureOptions<T>(T options)
        {
            var optionsBuilder = options as DbContextOptionsBuilder;
            var connectionStringProvider = this._connectionStringProvider.GetConnectionString();
            SqlServerDbContextOptionsExtensions.UseSqlServer(optionsBuilder, connectionStringProvider.ConnectionString);
            ProxiesExtensions.UseLazyLoadingProxies(optionsBuilder);
        }
    }
}
