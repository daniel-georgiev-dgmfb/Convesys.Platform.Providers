//using Glasswall.Platform.Cryptography.Certificates.Backchannel.Validation;
using System;

namespace Convesys.Platform.Cryptography.Extensions
{
    public static class CryptographyExtensions
    {
        public static IDependencyResolver AddBackchannelCertificateValidation(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            if(!dependencyResolver.Contains<IBackchannelCertificateValidator, BackchannelCertificateValidator>())
                dependencyResolver.RegisterType<IBackchannelCertificateValidator, BackchannelCertificateValidator>(Lifetime.Transient);

            if (!dependencyResolver.Contains<ICertificateValidationConfigurationProvider, DefaultCertificateValidationConfigurationProvider>())
                dependencyResolver.RegisterType<ICertificateValidationConfigurationProvider, DefaultCertificateValidationConfigurationProvider>(Lifetime.Transient);

            BackchannelCertificateValidationRulesFactory.InstanceCreator = t => (IBackchannelCertificateValidationRule)dependencyResolver.Resolve(t);
            BackchannelCertificateValidationRulesFactory.RulesFactory = () => dependencyResolver.ResolveAll<IBackchannelCertificateValidationRule>();
            BackchannelCertificateValidationRulesFactory.CertificateValidatorResolverFactory = t => (ICertificateValidatorResolver)dependencyResolver.Resolve(t);
            return dependencyResolver;
        }

        public static IDependencyResolver AddCertificateManagment(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            if (!dependencyResolver.Contains<ICertificateManager, CertificateManager>())
                dependencyResolver.RegisterType<ICertificateManager, CertificateManager>(Lifetime.Transient);
            return dependencyResolver;
        }

        public static IDependencyResolver AddSecretManagment(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            if (!dependencyResolver.Contains<ISecretManager, SecretManager>())
                dependencyResolver.RegisterType<ISecretManager, SecretManager>(Lifetime.Transient);
            return dependencyResolver;
        }
    }
}