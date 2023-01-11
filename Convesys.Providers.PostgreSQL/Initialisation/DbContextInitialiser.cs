//  ----------------------------------------------------------------------- 
//   <copyright file="DbContextInitialiser.cs" company="Glasswall Solutions Ltd.">
//       Glasswall Solutions Ltd.
//   </copyright>
//  ----------------------------------------------------------------------- 

namespace Pirina.Providers.EntityFramework.Initialisation
{
    using Kernel.DependencyResolver;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class DbContextInitialiser
    {
        /// <summary>
        ///     Performs initialisation.
        /// </summary>
        /// <param name="dependencyResolver"></param>
        /// <returns></returns>
        public static Task InitialiseInternal(this IDependencyResolver dependencyResolver)
        {
            //Register EF context manually. Multiple context may exist to pick from
            dependencyResolver.RegisterType<PirinaDbContext>(Lifetime.Transient);

            //Register connection string dependency in ConnectionDefinitionParser(..., Func<PropertyInfo, string> configNameConverter)
            dependencyResolver.RegisterFactory<Func<PropertyInfo, string>>(() => x => x.Name, Lifetime.Transient);
            ////Register connection string dependency in ConnectionDefinitionParser(Func<NameValueCollection> connectionPropertiesFactory, ...)
            //dependencyResolver.RegisterFactory<Func<NameValueCollection>>(() => this.GetConnecitonStringAttributes, Lifetime.Transient);
            return Task.FromResult<object>(null);
        }
    }
}