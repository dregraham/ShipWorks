using System;
using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWebClient(RakutenStoreEntity store,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.store = store;
            this.encryptionProviderFactory = encryptionProviderFactory;

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

        private void ProcessRequest()
        {
            authKey = encryptionProviderFactory.CreateSecureTextEncryptionProvider("Rakuten")
                .Decrypt(rakutenStore.AuthKey);
        }
    }
}
