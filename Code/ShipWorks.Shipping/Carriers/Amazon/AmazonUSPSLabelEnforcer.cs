using System;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Enforce whether a label can be created through Amazon for a shipment
    /// </summary>
    public class AmazonUspsLabelEnforcer : IAmazonLabelEnforcer
    {
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

            JToken errorDate = token.SelectToken("ErrorDate");
            JToken errorReason = token.SelectToken("ErrorReason");

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
        /// <exception cref="ShippingException"></exception>
        public void VerifyShipment(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            string sdcTracking = shipment.TrackingNumber.Substring(5, 2);

            if (!sdcTracking.Equals("11", StringComparison.Ordinal) && !sdcTracking.Equals("16", StringComparison.Ordinal) && shipment.Amazon.CarrierName.Equals("STAMPS_DOT_COM"))
            {
                AmazonStoreEntity store = StoreManager.GetRelatedStore(shipment.OrderID) as AmazonStoreEntity;

                if (store == null)
                {
                    throw new ShippingException("");
                }

                string token =
                    $"{{\"ErrorDate\":\"{SqlSession.Current.GetLocalDate()}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your Stamps.com account is linked correctly in Amazon Seller Central.\"}}";

                store.AmazonShippingToken = SecureText.Encrypt(token, "AmazonShippingToken");
            }
        }
    }
}
