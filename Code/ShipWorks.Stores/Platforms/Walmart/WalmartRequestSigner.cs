using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using System.Globalization;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Signer for Walmart web requests
    /// </summary>
    [Component]
    public class WalmartRequestSigner : IWalmartRequestSigner
    {
        private readonly IEncryptionProvider encryptionProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartRequestSigner"/> class.
        /// </summary>
        /// <param name="encryptionProviderFactory">The encryption provider factory.</param>
        public WalmartRequestSigner(IEncryptionProviderFactory encryptionProviderFactory, IDateTimeProvider dateTimeProvider)
        {
            encryptionProvider = encryptionProviderFactory.CreateWalmartEncryptionProvider();
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Signs the given request with the timestamp and generated signature
        /// </summary>
        public void Sign(IHttpRequestSubmitter requestSubmitter, WalmartStoreEntity store)
        {
            string epoch = (dateTimeProvider.Epoc * 1000).ToString(CultureInfo.InvariantCulture);
            RsaKeyParameters rsaKeyParameter;
            try
            {
                byte[] keyBytes = Convert.FromBase64String(encryptionProvider.Decrypt(store.PrivateKey));
                AsymmetricKeyParameter asymmetricKeyParameter = PrivateKeyFactory.CreateKey(keyBytes);
                rsaKeyParameter = (RsaKeyParameters)asymmetricKeyParameter;
            }
            catch (Exception)
            {
                throw new WalmartException("Unable to load Walmart private key");
            }

            string method = EnumHelper.GetDescription(requestSubmitter.Verb).ToUpper();

            Uri uri = requestSubmitter.GetPreparedRequestUri();

            string message = $"{store.ConsumerID}\n{uri.AbsoluteUri}\n{method}\n{epoch}\n";

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, rsaKeyParameter);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);

            byte[] signed = signer.GenerateSignature();

            string signature =  Convert.ToBase64String(signed);

            requestSubmitter.Headers.Add("WM_SEC.TIMESTAMP", epoch);
            requestSubmitter.Headers.Add("WM_SEC.AUTH_SIGNATURE", signature);
        }
    }
}