using System;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Enforce whether a label can be created through Amazon for a shipment
    /// </summary>
    public class AmazonUspsLabelEnforcer : IAmazonLabelEnforcer
    {
        private readonly IStoreManager storeManager;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUspsLabelEnforcer"/> class.
        /// </summary>
        /// <param name="storeManager">The store manager.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        public AmazonUspsLabelEnforcer(IStoreManager storeManager, IDateTimeProvider dateTimeProvider)
        {
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

            JToken errorDate = token.SelectToken("ErrorDate");
            JToken errorReason = token.SelectToken("ErrorReason");
            
            DateTime errorDateTime = DateTime.Parse(errorDate.ToString());

            if (errorDateTime.Date == dateTimeProvider.CurrentSqlServerDateTime.Date)
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

            AmazonStoreEntity store = GetStore(shipment);

            if (!sdcTracking.Equals("11", StringComparison.Ordinal) && !sdcTracking.Equals("16", StringComparison.Ordinal) && shipment.Amazon.CarrierName.Equals("STAMPS_DOT_COM"))
            {
                string token =
                    $"{{\"ErrorDate\":\"{dateTimeProvider.CurrentSqlServerDateTime.Date}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your Stamps.com account is linked correctly in Amazon Seller Central.\"}}";

                store.AmazonShippingToken = SecureText.Encrypt(token, "AmazonShippingToken");
            }
        }

        /// <summary>
        /// Gets the store the shipment originated from
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException"></exception>
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
