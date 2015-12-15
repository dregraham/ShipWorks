using Autofac;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// Best rate broker for USPS accounts
    /// </summary>
    public class UspsBestRateBroker : PostalResellerBestRateBroker<UspsAccountEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsBestRateBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        public UspsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity> accountRepository) :
            this(shipmentType, accountRepository, "USPS")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsBestRateBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="carrierDescription">The carrier description.</param>
        protected UspsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        { }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider)settings.UspsInsuranceProvider;
        }

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            base.CreateShipmentChild(shipment);
            shipment.Postal.Usps = new UspsShipmentEntity();
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, UspsAccountEntity account)
        {
            postalShipmentEntity.Usps.UspsAccountID = account.UspsAccountID;
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(UspsAccountEntity account)
        {
            return account.Description;
        }

        /// <summary>
        /// Gets rates from the UspsRatingService without Express1 rates
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rates;

            // Get rates from ISupportExpress1Rates if it is registered for the shipmenttypecode
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {

                UspsShipmentType shipmentType = lifetimeScope.Resolve<UspsShipmentType>();

                shipmentType.UpdateDynamicShipmentData(shipment);

                OrderHeader orderHeader = DataProvider.GetOrderHeader(shipment.OrderID);

                // Confirm the address of the cloned shipment with the store giving it a chance to inspect/alter the shipping address
                StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(orderHeader.StoreID));
                storeType.OverrideShipmentDetails(shipment);

                ISupportExpress1Rates ratingService =
                    lifetimeScope.ResolveKeyed<ISupportExpress1Rates>((ShipmentTypeCode)shipment.ShipmentType);

                // Get rates without express1 rates
                rates = ratingService.GetRates(shipment, false);
            }

            rates = rates.CopyWithRates(MergeDescriptionsWithNonSelectableRates(rates.Rates));
            // If a postal counter provider, show USPS logo, otherwise show appropriate logo such as endicia:
            rates.Rates.ForEach(UseProperUspsLogo);

            return rates;
        }
    }
}
