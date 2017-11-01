using ShipWorks.Shipping.Carriers.BestRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers.Dhl;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DHL Express best rate broker
    /// </summary>
    public class DhlExpressBestRateBroker : PackageBasedBestRateBroker<DhlExpressAccountEntity, IDhlExpressAccountEntity, DhlExpressPackageEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "DHL Express")
        {

        }

        /// <summary>
        /// Gets the insurance provider - DHL only supports Shipworks insurance
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings)
        {
            return InsuranceProvider.ShipWorks;
        }
        
        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.DhlExpress.Fields.State;
        }

        /// <summary>
        /// Creates and attaches a new instance of a DhlExpressShipmentEntity to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity();
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        protected override bool IsExcludedServiceType(object tag)
        {
            return false;
        }

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected override long PackageId(DhlExpressPackageEntity package)
        {
            return package.DhlExpressPackageID;
        }

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected override IList<DhlExpressPackageEntity> Packages(ShipmentEntity shipment)
        {
            return shipment.DhlExpress.Packages;
        }

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected override void SetPackageId(DhlExpressPackageEntity package, long packageId)
        {
            package.DhlExpressPackageID = packageId;
        }

        /// <summary>
        /// Gets the service type from tag.
        /// </summary>
        protected override int GetServiceTypeFromTag(object tag)
        {
            return (int) EnumHelper.GetEnumByApiValue<DhlExpressServiceType>((string) tag);
        }

        /// <summary>
        /// Sets the service type on the Dhl Express shipment from the value in the rate tag
        /// </summary>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.DhlExpress.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Applies DhlExpress specific data to the specified shipment
        /// </summary>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, DhlExpressAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.DhlExpress.Packages[0].DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.DhlExpress.Packages[0].DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.DhlExpress.Packages[0].DimsLength = originalShipment.BestRate.DimsLength;
            currentShipment.DhlExpress.Packages[0].DimsProfileID = originalShipment.BestRate.DimsProfileID;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.DhlExpress.Packages[0].Weight = originalShipment.ContentWeight;
            currentShipment.DhlExpress.Packages[0].DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.DhlExpress.Packages[0].DimsWeight = originalShipment.BestRate.DimsWeight;

            // Update total weight
            ShipmentType.UpdateTotalWeight(currentShipment);

            currentShipment.DhlExpress.Service = (int) DhlExpressServiceType.ExpressWorldWide;
            currentShipment.DhlExpress.DhlExpressAccountID = account.DhlExpressAccountID;
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(DhlExpressAccountEntity account)
        {
            return account.Description;
        }
    }
}
