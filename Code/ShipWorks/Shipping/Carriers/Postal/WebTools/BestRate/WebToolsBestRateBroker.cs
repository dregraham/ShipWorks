using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate
{
    /// <summary>
    /// Best rate broker for WebTools accounts
    /// </summary>
    public class WebToolsBestRateBroker : PostalResellerBestRateBroker<NullEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WebToolsBestRateBroker() : this(new PostalWebShipmentType(), new WebToolsAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebToolsBestRateBroker(PostalWebShipmentType shipmentType, ICarrierAccountRepository<NullEntity> accountRepository) :
            this(shipmentType, accountRepository, "USPS (w/o Postage)")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected WebToolsBestRateBroker(PostalWebShipmentType shipmentType, ICarrierAccountRepository<NullEntity> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        {

        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullEntity account)
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Ensure that account address cannot be chosen as the origin
        /// </summary>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, NullEntity account)
        {
            if (originalShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                throw new ShippingException("USPS (w/o Postage) does not support using an account address as the origin.");
            }

            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);
        }

        /// <summary>
        /// There is no extra selection logic for WebTools postal
        /// </summary>
        protected override void SelectChildShipment(PostalShipmentEntity rateShipment, ShipmentEntity selectedShipment)
        {

        }
    }
}
