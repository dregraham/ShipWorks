using System;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;

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
        public EnforcementResult CheckRestriction(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            IAmazonCredentials store = GetStore(shipment);
            AmazonShippingToken shippingToken = store.GetShippingToken();

            if (shippingToken.ErrorDate.Date >= dateTimeProvider.CurrentSqlServerDateTime.Date)
            {
                return new EnforcementResult(shippingToken.ErrorReason);
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

            if (!IsStampsDotComShipment(shipment))
            {
                return;
            }

            string sdcTracking = shipment.TrackingNumber.Substring(5, 2);

            IAmazonCredentials store = GetStore(shipment);

            if (!sdcTracking.Equals("11", StringComparison.Ordinal) && !sdcTracking.Equals("16", StringComparison.Ordinal))
            {
                AmazonShippingToken shippingToken = new AmazonShippingToken
                {
                    ErrorDate = dateTimeProvider.CurrentSqlServerDateTime.Date,
                    ErrorReason =
                        "ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your Stamps.com account is linked correctly in Amazon Seller Central. If you have confirmed your account is linked properly, please call ShipWorks customer support at 1-800-952-7784."
                };

                store.SetShippingToken(shippingToken);
                storeManager.SaveStore(store as StoreEntity);
            }
        }

        /// <summary>
        /// Gets the store the shipment originated from
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException"></exception>
        private IAmazonCredentials GetStore(ShipmentEntity shipment)
        {
            IAmazonCredentials store = storeManager.GetRelatedStore(shipment) as IAmazonCredentials;

            if (store == null)
            {
                throw new ShippingException("Amazon as shipping carrier can only be used on orders from an Amazon store");
            }

            return store;
        }

        /// <summary>
        /// Does the shipment use Stamps
        /// </summary>
        private static bool IsStampsDotComShipment(ShipmentEntity shipment) =>
            shipment.Amazon.CarrierName == "STAMPS_DOT_COM";
    }
}
