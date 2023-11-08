using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Dhl Ecommerce BestRate Broker
    /// </summary>
    public class DhlEcommerceBestRateBroker : BestRateBroker<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity>
    {
        public DhlEcommerceBestRateBroker(ShipmentType shipmentType, 
            ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository, 
            IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) : 
            base(shipmentType, accountRepository, "", bestRateExcludedAccountRepository)
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings)
        {
            return (InsuranceProvider) settings.DhlEcommerceInsuranceProvider;
        }

        /// <summary>
        /// Creates and attaches a new instance of a DhlEcommerceShipmentEntity to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment) => shipment.DhlEcommerce = new DhlEcommerceShipmentEntity();

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        protected override bool IsExcludedServiceType(object tag) => false;

        /// <summary>
        /// Sets the service type on the Dhl eCommerce shipment from the value in the rate tag
        /// </summary>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.DhlEcommerce.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Gets the service type from tag.
        /// </summary>
        protected override int GetServiceTypeFromTag(object tag) => (int) EnumHelper.GetEnumByApiValue<DhlEcommerceServiceType>((string) tag);

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(DhlEcommerceAccountEntity account) => account.AccountDescription;

        /// <summary>
        /// Updates data on the DHL Ecommerce child shipment that is required for checking best rate
        /// </summary>
        /// <param name="currentShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, DhlEcommerceAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.DhlEcommerce.DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.DhlEcommerce.DimsLength = originalShipment.BestRate.DimsLength;
            currentShipment.DhlEcommerce.DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.DhlEcommerce.DimsProfileID = originalShipment.BestRate.DimsProfileID;

            currentShipment.DhlEcommerce.DimsWeight = originalShipment.BestRate.DimsWeight;
            currentShipment.DhlEcommerce.DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.DhlEcommerce.PackagingType = (int) DhlEcommercePackagingType.ParcelSelectMachinable;
            currentShipment.DhlEcommerce.InsuranceValue = originalShipment.BestRate.InsuranceValue;
            currentShipment.Insurance = originalShipment.BestRate.Insurance;

            ShipmentType.UpdateTotalWeight(currentShipment);

            currentShipment.DhlEcommerce.DhlEcommerceAccountID = account.DhlEcommerceAccountID;
        }
    }
}
