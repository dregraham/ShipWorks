﻿using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBroker that Rate broker that 
    /// finds the best rates for FedEx accounts.
    /// </summary>
    public class FedExBestRateBroker : PackageBasedBestRateBroker<FedExAccountEntity, FedExPackageEntity>
    {
        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public FedExBestRateBroker()
            : this(new FedExShipmentType(), new FedExAccountRepository())
        {

        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a FedEx shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get FedEx accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public FedExBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<FedExAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "FedEx")
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider) settings.FedExInsuranceProvider;
        }

        /// <summary>
        /// Applies FedEx specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, FedExAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);
            
            currentShipment.FedEx.Packages[0].DimsHeight = currentShipment.BestRate.DimsHeight;
            currentShipment.FedEx.Packages[0].DimsWidth = currentShipment.BestRate.DimsWidth;
            currentShipment.FedEx.Packages[0].DimsLength = currentShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.FedEx.Packages[0].Weight = originalShipment.ContentWeight;
            currentShipment.FedEx.Packages[0].DimsAddWeight = false;
            currentShipment.FedEx.PackagingType = (int)FedExPackagingType.Custom;
            currentShipment.FedEx.Service = (int)FedExServiceType.FedExGround;
            currentShipment.FedEx.FedExAccountID = account.FedExAccountID;
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        /// <param name="tag">FedEx service type from the rate tag</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            return false;
        }

        /// <summary>
        /// Creates and attaches a new instance of a FedExShipment to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.FedEx = new FedExShipmentEntity();
        }

        /// <summary>
        /// Sets the service type on the FedEx shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.FedEx.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Gets the service type from tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        protected override int GetServiceTypeFromTag(object tag)
        {
            return (int) ((FedExRateSelection)tag).ServiceType;
        }

        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.FedEx.Fields.State;
        }

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected override IList<FedExPackageEntity> Packages(ShipmentEntity shipment)
        {
            return shipment.FedEx.Packages;
        }

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected override long PackageId(FedExPackageEntity package)
        {
            return package.FedExPackageID;
        }

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected override void SetPackageId(FedExPackageEntity package, long packageId)
        {
            package.FedExPackageID = packageId;
        }
    }
}
