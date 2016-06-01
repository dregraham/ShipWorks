using System;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Enforce label restrictions for Ups shipments through Amazon
    /// </summary>
    public class AmazonUpsLabelEnforcer : IAmazonLabelEnforcer
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private readonly IStoreManager storeManager;
        private readonly ISqlDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUpsLabelEnforcer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="storeManager">The store manager.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        public AmazonUpsLabelEnforcer(ICarrierAccountRepository<UpsAccountEntity> accountRepository, IStoreManager storeManager, ISqlDateTimeProvider dateTimeProvider)
        {
            this.accountRepository = accountRepository;
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

            if (shippingToken.ErrorDate.Date >= dateTimeProvider.GetLocalDate().Date)
            {
                return new EnforcementResult(shippingToken.ErrorReason);
            }

            return EnforcementResult.Success;
        }


        /// <summary>
        /// Verify that the processed shipment is valid
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3215:\"interface\" instances should not be cast to concrete types", Justification = "<Pending>")]
        public void VerifyShipment(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (!IsUpsShipment(shipment))
            {
                return;
            }

            string upsTracking = shipment.TrackingNumber;

            if (accountRepository.Accounts.Any(account => upsTracking.IndexOf(account.AccountNumber, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return;
            }

            IAmazonCredentials store = GetStore(shipment);
            AmazonShippingToken shippingToken = new AmazonShippingToken
            {
                ErrorDate = dateTimeProvider.GetLocalDate().Date,
                ErrorReason =
                    "ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. " +
                    "Please confirm your UPS account is linked correctly in Amazon Seller Central. " +
                    "If you have confirmed your account is linked properly, please call ShipWorks customer support at 1-800-952-7784."
            };

            store.SetShippingToken(shippingToken);
            storeManager.SaveStore(store as StoreEntity);
        }

        /// <summary>
        /// Gets the store the shipment originated from
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">Amazon as shipping carrier can only be used on orders from an Amazon store</exception>
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
        /// Does the shipment use UPS
        /// </summary>
        private static bool IsUpsShipment(ShipmentEntity shipment) => shipment.Amazon.CarrierName == "UPS";
    }
}
