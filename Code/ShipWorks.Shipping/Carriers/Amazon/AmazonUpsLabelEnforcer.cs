using System;
using System.Globalization;
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
        private readonly IStoreManager storeManager;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUpsLabelEnforcer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="storeManager">The store manager.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        public AmazonUpsLabelEnforcer(ICarrierAccountRepository<UpsAccountEntity> accountRepository, IStoreManager storeManager, IDateTimeProvider dateTimeProvider)
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

            AmazonShippingToken shippingToken = store.GetShippingToken();

            if (!accountRepository.Accounts.Any() || shippingToken.ErrorDate.Date == dateTimeProvider.CurrentSqlServerDateTime.Date)
            {
                return new EnforcementResult(shippingToken.ErrorReason);
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

            if (accountRepository.Accounts.Any(account => upsTracking.Contains(account.AccountNumber)))
            {
                return;
            }

            AmazonStoreEntity store = GetStore(shipment);
            AmazonShippingToken shippingToken = new AmazonShippingToken
            {
                ErrorDate = dateTimeProvider.CurrentSqlServerDateTime.Date,
                ErrorReason =
                    "ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central."
            };

            store.SetShippingToken(shippingToken);
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
