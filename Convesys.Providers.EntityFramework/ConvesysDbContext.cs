namespace Convesys.Providers.EntityFramework
{
    using Convesys.Kernel.Data.ORM;
    using Convesys.Kernel.Data.Tenancy;
    using Convesys.Kernel.Reflection.Reflection;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ConvesysDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext, ICatalogContext
    {
        private static readonly MethodInfo SetGlobalQueryMethod = typeof(ConvesysDbContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public ConvesysDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<ConvesysDbContext> options, IDbCustomConfiguration customConfiguration) 
            : base(options)
        {
            CustomConfiguration = customConfiguration ?? throw new ArgumentNullException(nameof(customConfiguration));
        }

        public IDbCustomConfiguration CustomConfiguration { get; }

        T IDbContext.Add<T>(T item)
        {
            if (!typeof(BaseTenantModel).IsAssignableFrom(typeof(T))) return base.Set<T>().Add(item).Entity;

            var queryBuilder = CustomConfiguration.TenantManager();
            var tenantModel = item as BaseTenantModel;

            queryBuilder.AssignTenantId(tenantModel);

            return base.Set<T>().Add(item).Entity;
        }

        bool IDbContext.Remove<T>(T item)
        {
            var entityEntry = base.Set<T>().Remove(item);
            return entityEntry != null;
        }
        
        IQueryable<T> IDbContext.Set<T>()
        {
            return base.Set<T>();
        }

        int IDbContext.SaveChanges()
        {
            Database.EnsureCreated();
            return base.SaveChanges();
        }

        async Task<int> IDbContext.SaveChangesAsync()
        {
            Database.EnsureCreated();
            return await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
                this.CustomConfiguration.ConfigureOptions(optionsBuilder);
                //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
                //optionsBuilder.UseLazyLoadingProxies();
            //}

            //optionsBuilder.ReplaceService<IModelCacheKeyFactory, CustomModelCacheKeyFactory>();
        }

        protected sealed override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            CreateModel(modelBuilder);
        }

        protected virtual void CreateModel(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            //Either resolve models by assembly scanning or use the ones provided in db configuration
            //This runs onces when the models is being created. If the context is used with different schemas model key needs to be provide. TBD
            var models = CustomConfiguration.ModelsFactory != null
                ? CustomConfiguration.ModelsFactory()
                : ReflectionHelper.GetAllTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface && typeof(BaseTenantModel).IsAssignableFrom(t) && t != typeof(object));

            //Register models
            foreach (var m in models)
            {
                if (modelBuilder.Model.FindEntityType(m) != null) continue;

                modelBuilder.Model.AddEntityType(m);

                if (!typeof(BaseTenantModel).IsAssignableFrom(m)) continue;

                // If the type is assignable to BaseTenantModel then apply global filtering to the generic entity.
                var method = SetGlobalQueryMethod.MakeGenericMethod(m);
                method.Invoke(this, new object[] {modelBuilder});
            }

            //apply mappings
            // TODO: Work out intended behaviour, casting IDbMapper to IDbMapper<ModelBuilder> isn't a valid cast.
            if (CustomConfiguration.ModelMappers != null)
            {
                var mappers = CustomConfiguration.ModelMappers();
                var tasks = mappers.Cast<IDbMapper<ModelBuilder>>()
                    .Select(x => x.Configure(modelBuilder, CustomConfiguration));
                Task.WaitAll(tasks.ToArray());
            }

            if (CustomConfiguration.Seeders.IsNullOrEmpty())
            {
                return;
            }

            foreach (var s in CustomConfiguration.Seeders.OrderBy(x => x.SeedingOrder)
                .Cast<ISeeder<ModelBuilder>>())
                s.Seed(modelBuilder);
        }

        // This method is called for every loaded entity type in OnModelCreating method.
        // Here type is known through generic parameter and we can use EF Core methods.
        private void SetGlobalQuery<T>(Microsoft.EntityFrameworkCore.ModelBuilder builder) where T : BaseTenantModel
        {
            builder.Entity<T>().HasKey(e => e.Id);
            builder.Entity<T>().HasQueryFilter(e =>
                e.TenantId == CustomConfiguration.TenantManager().ResolveTenant() && !e.IsDeleted);
        }
    }
}