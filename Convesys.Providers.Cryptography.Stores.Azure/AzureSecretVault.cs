
using Convesys.Kernel.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pirina.Providers.Cryptography.Stores.Azure
{
    public class AzureSecretVault : ISecretStore
  {
    private ICacheProvider _cache;
    private readonly IEventLogger<AzureSecretVault> _logger;
    internal readonly IConfiguration _configuration;

    public AzureSecretVault(
      IConfiguration configuration,
      ICacheProvider cache,
      IEventLogger<AzureSecretVault> logger)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      if (cache == null)
        throw new ArgumentNullException(nameof (cache));
      if (logger == null)
        throw new ArgumentNullException(nameof (logger));
      this._configuration = configuration;
      this._cache = cache;
      this._logger = logger;
    }

    public string StoreLocation
    {
      get
      {
        string str = this._configuration.GetValue<string>("ASPNETCORE_VAULTDNS");
        if (!string.IsNullOrWhiteSpace(str))
          return str;
        throw new InvalidOperationException(string.Format("{0} variable not found", (object) "ASPNETCORE_VAULTDNS"));
      }
    }

    public Task<string> GetSecret(SecretContext secretContext)
    {
      if (secretContext is AzureVaultSecretContext)
        return this.GetSecretInternal((AzureVaultSecretContext) secretContext);
      return this.GetSecretInternal(new AzureVaultSecretContext(this._configuration, secretContext));
    }

    protected virtual async Task<string> GetSecretInternal(
      AzureVaultSecretContext azureVaultSecretContext)
    {
      string key = this.BuildCacheKey(azureVaultSecretContext);
      this._logger.Log(SeverityLevel.Info, 0, string.Format("Trying to get a secret from cache. Key: {0}", (object) key), (Exception) null, ((_, __) => _.ToString()));

      KeyVaultClient.AuthenticationCallback authenticationCallback = ((_, __, ___) =>
      { 
          var tokenTask =  AuthenticationHelper.GetToken(_, __, ___, azureVaultSecretContext.ClientId, azureVaultSecretContext.ClientSecret);
          return tokenTask;
      });
      return (await this._cache.GetOrAddAsync<SecretBundle>(key, (Func<object, Task<SecretBundle>>) (async o =>
      {
        this._logger.Log(SeverityLevel.Info, 0, string.Format("Begin retrieving secret from store: {0}", (object) this.StoreLocation), (Exception) null, ((_, __) => _.ToString()));
        
        KeyVaultClient keyVaultClient = new KeyVaultClient(authenticationCallback);
        bool flag = SecretIdentifier.IsSecretIdentifier(azureVaultSecretContext.SecretName);
        this._logger.Log(SeverityLevel.Trace, 0, string.Format("Secret name is{0}identifier.", flag ? (object) string.Empty : (object) " not "), (Exception) null,  ((_, __) => _.ToString()));
        this._logger.Log(SeverityLevel.Trace, 0, string.Format("Making a call to the secret store."), (Exception) null, ((_, __) => _.ToString()));
        return await (flag ? 
          KeyVaultClientExtensions.GetSecretAsync((IKeyVaultClient) keyVaultClient, azureVaultSecretContext.SecretName, new CancellationToken()) 
          : 
          KeyVaultClientExtensions.GetSecretAsync((IKeyVaultClient) keyVaultClient, this.StoreLocation, azureVaultSecretContext.SecretName, new CancellationToken()));
      }), CancellationToken.None)).Value;
    }

    private string BuildCacheKey(AzureVaultSecretContext azureVaultSecretContext)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0}_{1}", (object) azureVaultSecretContext.ClientId, (object) azureVaultSecretContext.SecretName);
      if (!string.IsNullOrWhiteSpace(azureVaultSecretContext.Version))
        stringBuilder.AppendFormat("_{0}", (object) azureVaultSecretContext.Version);
      return stringBuilder.ToString();
    }
  }
}
