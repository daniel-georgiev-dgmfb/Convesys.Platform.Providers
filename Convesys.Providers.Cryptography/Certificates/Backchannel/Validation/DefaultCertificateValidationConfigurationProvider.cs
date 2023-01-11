using System;

namespace Convesys.Platform.Cryptography.Certificates.Backchannel.Validation
{
    internal class DefaultCertificateValidationConfigurationProvider : ICertificateValidationConfigurationProvider
    {
        public BackchannelConfiguration GeBackchannelConfiguration(string federationPartyId)
        {
            return new BackchannelConfiguration
            {
                UsePinningValidation = false
            };
        }

        public BackchannelConfiguration GeBackchannelConfiguration(Uri partyUri)
        {
            return this.GeBackchannelConfiguration(partyUri.AbsoluteUri);
        }

        public CertificateValidationConfiguration GetConfiguration(string federationPartyId)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}