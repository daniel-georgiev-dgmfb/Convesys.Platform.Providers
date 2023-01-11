using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Convesys.Platform.Cryptography.Certificates.Management
{
    public class CertificateManager : ICertificateManager
    {
        private readonly IEventLogger<CertificateManager> _logProvider;
        private readonly Func<CertificateContext, ICertificateStore> _storeFactory;

        public IBackchannelCertificateValidator BackchannelCertificateValidator => throw new NotImplementedException();

        public CertificateManager(Func<CertificateContext, ICertificateStore> storeFactory, IEventLogger<CertificateManager> logProvider)
        {
            if (storeFactory == null)
                throw new ArgumentNullException(nameof(storeFactory));
            if (logProvider == null)
                throw new ArgumentNullException(nameof(logProvider));

            this._storeFactory = storeFactory;
            this._logProvider = logProvider;
        }

        /// <summary>
        /// Try to add a certifictae to a store in given location. Optionally it creates the store if it doesn't exist
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="location"></param>
        /// <param name="certificate"></param>
        /// <param name="createIfNotExist"></param>
        /// <returns></returns>
        public bool TryAddCertificateToStore(string storeName, StoreLocation location, X509Certificate2 certificate, bool createIfNotExist)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instantiate X509Certificate2 from a certifictae file and password
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public X509Certificate2 GetCertificate(string path, SecureString password)
        {
            return new X509Certificate2(path, password);
        }

        /// <summary>
        /// Instantiate X509Certificate2 from a given certificate store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public X509Certificate2 GetCertificate(ICertificateStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            return store.GetX509Certificate2();
        }

        /// <summary>
        /// Instantiate X509Certificate2 from certificate context. 
        /// </summary>
        /// <param name="certContext"></param>
        /// <returns></returns>
        public X509Certificate2 GetCertificateFromContext(CertificateContext certContext)
        {
            var store = this.GetStoreFromContext(certContext);
            return this.GetCertificate(store);
        }

        /// <summary>
        /// Get certificate thumbprint
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public string GetCertificateThumbprint(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException("certificate");
            return certificate.Thumbprint;
        }

        /// <summary>
        /// Get x509 store from certificate context. Only x509 store supported.
        /// </summary>
        /// <param name="certContext"></param>
        /// <returns></returns>
        public ICertificateStore GetStoreFromContext(CertificateContext certContext)
        {
            return this._storeFactory(certContext);
        }

        /// <summary>
        /// Sign data and encode it as base64
        /// </summary>
        /// <param name="dataToSign"></param>
        /// <param name="certContext"></param>
        /// <returns></returns>
        public string SignToBase64(string dataToSign, CertificateContext certContext)
        {
            //this._logProvider.LogMessage(String.Format("Signing data with certificate from context: {0}", certContext.ToString()));
            var data = Encoding.UTF8.GetBytes(dataToSign);
            var cert = this.GetCertificateFromContext(certContext);
            var signed = this.SignData(dataToSign, cert);

            var base64 = Convert.ToBase64String(signed);
            return base64;
        }
        public byte[] SignData(string dataToSign, X509Certificate2 certificate)
        {
            var data = Encoding.UTF8.GetBytes(dataToSign);
            var signed = RSADataProtection.SignDataSHA1((RSA)certificate.PrivateKey, data);
            return signed;
        }

        /// <summary>
        /// Varify signature
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signed"></param>
        /// <param name="certContext"></param>
        /// <returns></returns>
        public bool VerifySignatureFromBase64(string data, string signed, CertificateContext certContext)
        {
            var cert = this.GetCertificateFromContext(certContext);
            return this.VerifySignatureFromBase64(data, signed, cert);
        }

        public bool VerifySignatureFromBase64(string data, string signed, X509Certificate2 certificate)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signedBytes = Convert.FromBase64String(signed);
            var verified = RSADataProtection.VerifyDataSHA1Signed((RSA)certificate.PublicKey.Key, dataBytes, signedBytes);
            return verified;
        }

        /// <summary>
        /// Try to extract subject public key information from certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="spkiEncoded"></param>
        /// <returns></returns>
        public bool TryExtractSpkiBlob(X509Certificate2 certificate, out string spkiEncoded)
        {
            try
            {
                var spki = Utility.ExtractSpkiBlob(certificate);
                spkiEncoded = Utility.HashSpki(spki);
                return !String.IsNullOrWhiteSpace(spkiEncoded);
            }
            catch (Exception ex)
            {
                //Exception innerEx;
                //this._logProvider.TryLogException(ex, out innerEx);
                spkiEncoded = null;
                return false;
            }
        }

        /// <summary>
        /// Get subject identifier from the certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public string GetSubjectKeyIdentifier(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException("certificate");
            return Utility.GetSubjectKeyIdentifier(certificate);
        }

        public TResolver GetX509CertificateStoreTokenResolver<TResolver>(X509CertificateContext x509CertificateContext)
        {
            throw new NotImplementedException();
        }
    }
}