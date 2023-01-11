
using Pirina.Kernel.Configuration;
using Pirina.Kernel.Security.SecretManagement;
using System;

namespace Pirina.Providers.Cryptography.Stores.Azure
{
    public class AzureVaultSecretContext : SecretContext
  {
    private readonly IConfiguration _configuration;

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

    public AzureVaultSecretContext(IConfiguration configuration, string secretName)
      : base(secretName)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      this._configuration = configuration;
    }

    public AzureVaultSecretContext(IConfiguration configuration, SecretContext secretContext)
      : base(secretContext.SecretName)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      this._configuration = configuration;
      this.Version = secretContext.Version;
    }
  }
}
