using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// The client for communicating with the Rakuten API
    /// </summary>
    [Component]
    public class RakutenWebClient : IRakutenWebClient
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly RakutenStoreEntity store;
        private readonly IHttpRequestSubmitterFactory submitterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWebClient(RakutenStoreEntity store,
            IEncryptionProviderFactory encryptionProviderFactory,
            IHttpRequestSubmitterFactory submitterFactory)
        {
            this.store = store;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.submitterFactory = submitterFactory;
        }

        /// <summary>
        /// Get a list of orders from Rakuten
        /// </summary>
        public RakutenOrdersResponse GetOrders(DateTime startDate)
        {

        }

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        public void ConfirmShipping(RakutenOrderEntity order)
        {

        }

        /// <summary>
        /// Create a request to channel advisor
        /// </summary>
        private IHttpVariableRequestSubmitter CreateRequest(string endpoint, HttpVerb method)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory.GetHttpVariableRequestSubmitter();
            submitter.Uri = new Uri(endpoint);
            submitter.Verb = method;

            submitter.ContentType = "application/x-www-form-urlencoded";
            AuthenticateRequest(submitter);

            return submitter;
        }

        private void ProcessRequest()
        {
            authKey = encryptionProviderFactory.CreateSecureTextEncryptionProvider("Rakuten")
                .Decrypt(rakutenStore.AuthKey);
        }
    }
}
