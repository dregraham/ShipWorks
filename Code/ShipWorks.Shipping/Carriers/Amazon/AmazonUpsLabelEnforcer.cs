using System;
using System.Linq;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Enforce label restrictions for Ups shipments through Amazon
    /// </summary>
    public class AmazonUpsLabelEnforcer : IAmazonLabelEnforcer
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private IStoreManager storeManager;
        private IDateTimeProvider dateTimeProvider;

        /// Constructor
        /// </summary>
        /// <param name="accountRepository"></param>
        public AmazonUpsLabelEnforcer(ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            this.accountRepository = accountRepository;
            this.storeManager = storeManager;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Is Amazon allowed for the given shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        /// <exception cref="ShippingException"></exception>
        public EnforcementResult CheckRestriction(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            AmazonStoreEntity store = GetStore(shipment);

            JToken token = JToken.Parse(SecureText.Decrypt(store.AmazonShippingToken, "AmazonShippingToken"));

            JToken errorReason = token.SelectToken("ErrorReason");
            JToken errorDate = token.SelectToken("ErrorDate");

            DateTime errorDateTime = DateTime.Parse(errorDate.ToString());

            if (!accountRepository.Accounts.Any() || errorDateTime.Date == dateTimeProvider.CurrentSqlServerDateTime.Date)
            {
                return new EnforcementResult(errorReason.ToString());
            }

            return EnforcementResult.Success;
        }

        /// <summary>
        /// Verify that the processed shipment is valid
        /// </summary>
        /// <param name="shipment"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void VerifyShipment(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            string upsTracking = shipment.TrackingNumber;

            if (accountRepository.Accounts.Cast<UpsAccountEntity>().Any(account => upsTracking.Contains(account.AccountNumber)))
            {
                return;
            }

            AmazonStoreEntity store = GetStore(shipment);

            string token =
                    $"{{\"ErrorDate\":\"{dateTimeProvider.CurrentSqlServerDateTime.Date}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central.\"}}";

            store.AmazonShippingToken = SecureText.Encrypt(token, "AmazonShippingToken");
        }

        /// <summary>
        /// Gets the store the shipment originated from
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">Amazon as shipping carrier can only be used on orders from an Amazon store</exception>
        private AmazonStoreEntity GetStore(ShipmentEntity shipment)
        {
            AmazonStoreEntity store = storeManager.GetRelatedStore(shipment.ShipmentID) as AmazonStoreEntity;

            if (store == null)
            {
                throw new ShippingException("Amazon as shipping carrier can only be used on orders from an Amazon store");
            }
            return store;
        }
    }
}
