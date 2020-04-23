﻿using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;

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
        public DhlExpressBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) :
            base(shipmentType, accountRepository, "", bestRateExcludedAccountRepository)
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

            currentShipment.DhlExpress.Packages[0].Insurance = originalShipment.Insurance;
            currentShipment.DhlExpress.Packages[0].InsuranceValue = originalShipment.BestRate.InsuranceValue;

            // Update total weight
            ShipmentType.UpdateTotalWeight(currentShipment);

            currentShipment.DhlExpress.DhlExpressAccountID = account.DhlExpressAccountID;

            // Customs items are cleared from the shipment when ConfigureNewShipment is called.
            // This was causing DHL not to return rates because it needs valid customs items.
            currentShipment.CustomsItems.Clear();
            foreach (ShipmentCustomsItemEntity customItem in originalShipment.CustomsItems)
            {
                currentShipment.CustomsItems.Add(EntityUtility.CloneEntity(customItem));
            }
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(DhlExpressAccountEntity account)
        {
            return account.AccountDescription;
        }
    }
}
