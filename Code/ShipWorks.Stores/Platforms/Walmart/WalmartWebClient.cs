using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Web client for interacting with Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Walmart.IWalmartWebClient" />
    [Component]
    public class WalmartWebClient : IWalmartWebClient
    {
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IEncryptionProvider encryptionProvider;
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartWebClient"/> class.
        /// </summary>
        /// <param name="encryptionProviderFactory">The encryption provider factory.</param>
        /// <param name="apiLogEntryFactory"></param>
        public WalmartWebClient(IEncryptionProviderFactory encryptionProviderFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            encryptionProvider = encryptionProviderFactory.CreateWalmartEncryptionProvider();
        }

        /// <summary>
        /// Tests the connection to Walmart, throws if invalid credentials
        /// </summary>
        public void TestConnection(WalmartStoreEntity store)
        {
            string epoch = (DateTimeUtility.ToUnixTimestamp(DateTime.UtcNow) * 1000).ToString(CultureInfo.InvariantCulture);
            string signature = GetSignature(store.ConsumerID, store.PrivateKey, TestConnectionUrl, "GET", epoch);

            HttpXmlVariableRequestSubmitter requestSubmitter = new HttpXmlVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(TestConnectionUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            requestSubmitter.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
            requestSubmitter.Headers.Add("WM_CONSUMER.ID", store.ConsumerID);
            requestSubmitter.Headers.Add("WM_SEC.TIMESTAMP", epoch);
            requestSubmitter.Headers.Add("WM_SEC.AUTH_SIGNATURE", signature);
            requestSubmitter.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", store.ChannelType);
            requestSubmitter.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());

            ProcessRequest(requestSubmitter, "TestConnection");
        }

        /// <summary>
        /// Get the Walmart auth signature
        /// </summary>
        private string GetSignature(string consumerId, string privateKey, string requestUrl, string requestMethod, string epoch)
        {
            string message = $"{consumerId}\n{requestUrl}\n{requestMethod.ToUpper()}\n{epoch}\n";

            RsaKeyParameters rsaKeyParameter;
            try
            {
                byte[] keyBytes = Convert.FromBase64String(encryptionProvider.Decrypt(privateKey));
                AsymmetricKeyParameter asymmetricKeyParameter = PrivateKeyFactory.CreateKey(keyBytes);
                rsaKeyParameter = (RsaKeyParameters)asymmetricKeyParameter;
            }
            catch (Exception)
            {
                throw new WalmartException("Unable to load Walmart private key");
            }

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, rsaKeyParameter);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);

            byte[] signed = signer.GenerateSignature();

            return Convert.ToBase64String(signed);
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private void ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                IApiLogEntry logEntry = apiLogEntryFactory(ApiLogSource.Walmart, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "xml");
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(WalmartException));
            }
        }

        public IEnumerable<Order> GetOrders(DateTime start)
        {
            throw new NotImplementedException();
        }

        public void Acknowledge(string purchaseOrderId)
        {
            throw new NotImplementedException();
        }
    }
}