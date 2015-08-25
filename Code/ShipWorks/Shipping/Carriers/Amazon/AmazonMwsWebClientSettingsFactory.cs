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
    }
}
