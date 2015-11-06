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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository"></param>
        public AmazonUpsLabelEnforcer(ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            this.accountRepository = accountRepository;
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

            AmazonStoreEntity store = StoreManager.GetRelatedStore(shipment.OrderID) as AmazonStoreEntity;

            if (store == null)
            {
                throw new ShippingException("Amazon as shipping carrier can only be used on orders from an Amazon store");
            }

            JToken token = SecureText.Decrypt(store.AmazonShippingToken, "AmazonShippingToken");

            JToken errorReason = token.SelectToken("ErrorReason");

            if (accountRepository.Accounts.Any())
            {
                return new EnforcementResult(errorReason.ToString());
            }

            JToken errorDate = token.SelectToken("ErrorDate");

            DateTime errorDateTime = DateTime.Parse(errorDate.ToString());

            if (errorDateTime == SqlSession.Current.GetLocalDate())
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

            AmazonStoreEntity store = StoreManager.GetRelatedStore(shipment.OrderID) as AmazonStoreEntity;

            if (store == null)
            {
                throw new ShippingException("Amazon as shipping carrier can only be used on orders from an Amazon store");
            }

            string token =
                $"{{\"ErrorDate\":\"{SqlSession.Current.GetLocalDate()}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central.\"}}";

            store.AmazonShippingToken = SecureText.Encrypt(token, "AmazonShippingToken");
        }
    }
}
