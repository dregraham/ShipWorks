using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Creates an instance of AmazonMwsWebClientSettings
    /// </summary>
    public class AmazonMwsWebClientSettingsFactory : IAmazonMwsWebClientSettingsFactory
    {
        IAmazonAccountManager accountManager;

        public AmazonMwsWebClientSettingsFactory(IAmazonAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonShipmentEntity
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(AmazonShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, () => shipment);

            AmazonAccountEntity account = accountManager.GetAccount(shipment.AmazonAccountID);

            if (account == null)
            {
                throw new AmazonShipperException("Amazon shipping account no longer exists");
            }

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(account.MerchantID, account.AuthToken, "US"));
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonStoreEntity
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(AmazonStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, () => store);

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(store.MerchantID, store.AuthToken, store.AmazonApiRegion));
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonStoreEntity
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(string merchantId, string authToken, string apiRegion)
        {
            if (string.IsNullOrWhiteSpace(merchantId) &&
                string.IsNullOrWhiteSpace(authToken) &&
                string.IsNullOrWhiteSpace(apiRegion))
            {
                throw new ArgumentException("Cannot pass empty string to Amazon Mws Connection");
            }

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(merchantId, authToken, apiRegion));
        }
    }
}
