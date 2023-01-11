using Pirina.Platform.Cryptography.Helpers;
using System;
using System.Threading.Tasks;

namespace Convesys.Platform.Cryptography.Secrets
{
    public class SecretManager : ISecretManager
    {
        private readonly ISecretStore _secretStore;
        private readonly IEventLogger<SecretManager> _logger;

        public SecretManager(ISecretStore secretStore, IEventLogger<SecretManager> logger)
        {
            if (secretStore == null)
                throw new ArgumentNullException(nameof(secretStore));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            this._secretStore = secretStore;
            this._logger = logger;
        }

        public Task<string> GetSecret(SecretContext secretContext)
        {
            this._logger.Log(SeverityLevel.Info, 0, String.Format("Getting secret {0} version: {1}", secretContext.SecretName, secretContext.Version), null, LoggerHelper.Formatter);
            return this._secretStore.GetSecret(secretContext);
        }

        public Task<string> GetSecret(string secretName)
        {
            return this.GetSecret(new SecretContext(secretName));
        }

        public Task<string> GetSecret(string secretName, string version)
        {
            return this.GetSecret(new SecretContext(secretName) { Version = version });
        }
    }
}