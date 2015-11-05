using System;
using System.Linq;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// 
    /// </summary>
    public class AmazonUpsLabelEnforcer : IAmazonLabelEnforcer
    {
        private readonly ICarrierAccountRepository<IEntity2> accountRepository;

        public AmazonUpsLabelEnforcer(ICarrierAccountRepository<IEntity2> accountRepository)
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
                throw new ShippingException("");
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

            bool containsAccountNumber = false;

            foreach (IEntity2 account in accountRepository.Accounts)
            {
                UpsAccountEntity upsAccount = (UpsAccountEntity) account;
                
                if (upsTracking.Contains(upsAccount.AccountNumber))
                {
                    return;
                }
            }
            
            AmazonStoreEntity store = StoreManager.GetRelatedStore(shipment.OrderID) as AmazonStoreEntity;

            if (store == null)
            {
                throw new ShippingException("");
            }

            string token =
                $"{{\"ErrorDate\":\"{SqlSession.Current.GetLocalDate()}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central.\"}}";

            store.AmazonShippingToken = SecureText.Encrypt(token, "AmazonShippingToken");
        }
    }
}
