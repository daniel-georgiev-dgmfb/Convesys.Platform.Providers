
using Pirina.Kernel.Configuration;
using Pirina.Kernel.Security.CertificateManagement;
using System;

namespace Pirina.Providers.Cryptography.Stores.Azure
{
    public class AzureVaultCertificateContext : CertificateContext
  {
    internal readonly IConfiguration _configuration;

    public string ClientId
    {
      get
      {
        string str = this._configuration.GetValue<string>("ASPNETCORE_VAULT_CLIENTID");
        if (!string.IsNullOrWhiteSpace(str))
          return str;
        throw new InvalidOperationException(string.Format("{0} variable not found", (object) "ASPNETCORE_VAULT_CLIENTID"));
      }
    }

    public string ClientSecret
    {
      get
      {
        string str = this._configuration.GetValue<string>("ASPNETCORE_VAULT_SECRET");
        if (!string.IsNullOrWhiteSpace(str))
          return str;
        throw new InvalidOperationException(string.Format("{0} variable not found", (object) "ASPNETCORE_VAULT_SECRET"));
      }
    }

    public string CertificateName
    {
      get
      {
        string str = this._configuration.GetValue<string>("ASPNETCORE_SSLCERT");
        if (!string.IsNullOrWhiteSpace(str))
          return str;
        throw new InvalidOperationException(string.Format("{0} variable not found", (object) "ASPNETCORE_SSLCERT"));
      }
    }

    public string VaultBaseUrl
    {
      get
      {
        string str = this._configuration.GetValue<string>("ASPNETCORE_VAULTDNS");
        if (!string.IsNullOrWhiteSpace(str))
          return str;
        throw new InvalidOperationException(string.Format("{0} variable not found", (object) "ASPNETCORE_VAULTDNS"));
      }
    }

    public AzureVaultCertificateContext(IConfiguration configuration)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      this._configuration = configuration;
    }
  }
}
