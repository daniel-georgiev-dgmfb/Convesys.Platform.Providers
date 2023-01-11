
using Pirina.Kernel.DependencyResolver;
using Pirina.Providers.Cryptography.Stores.Azure;
using System;

namespace Pirina.Platform.Cryptography.Stores.Azure.Extensions
{
    public static class AzureVaultExtensions
  {
    public static IDependencyResolver AddAzureSecretStore(
      this IDependencyResolver dependencyResolver)
    {
      throw new NotImplementedException();
    }

    public static IDependencyResolver AddAzureSecretStore(
      this IDependencyResolver dependencyResolver,
      Action<AzureVaultOptions> options)
    {
      throw new NotImplementedException();
    }

    public static IDependencyResolver AddAzureCertificateStore(
      this IDependencyResolver dependencyResolver)
    {
      throw new NotImplementedException();
    }

    public static IDependencyResolver AddAzureCertificateStore(
      this IDependencyResolver dependencyResolver,
      Action<AzureVaultOptions> options)
    {
      throw new NotImplementedException();
    }
  }
}
