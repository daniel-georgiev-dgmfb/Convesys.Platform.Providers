
using Pirina.Kernel.Security.CertificateManagement;
using Pirina.Kernel.Security.SecretManagement;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Pirina.Providers.Cryptography.Stores.Azure
{
    public class AzureCertificateVault : ICertificateStore
  {
    private readonly AzureVaultCertificateContext _certificateContext;
    private ISecretStore _secretStore;

    public StoreLocation StoreLocation
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public AzureCertificateVault(
      AzureVaultCertificateContext certificateContext,
      ISecretStore secretManager)
    {
      if (certificateContext == null)
        throw new ArgumentNullException(nameof (certificateContext));
      if (secretManager == null)
        throw new ArgumentNullException(nameof (secretManager));
      this._certificateContext = certificateContext;
      this._secretStore = secretManager;
    }

    public X509Certificate2 GetX509Certificate2()
    {
      return new X509Certificate2(Convert.FromBase64String(this._secretStore.GetSecret((SecretContext) new AzureVaultSecretContext(this._certificateContext._configuration, this._certificateContext.CertificateName)).GetAwaiter().GetResult()));
    }
  }
}
