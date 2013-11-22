using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// Rate broker that finds the best rates for UPS accounts
    /// </summary>
    public class UpsBestRateBroker : PackageBasedBestRateBroker<UpsAccountEntity, UpsPackageEntity>
    {
        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public UpsBestRateBroker()
            : this(new UpsOltShipmentType(), new UpsAccountRepository())
        {
        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a UPS shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get UPS accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public UpsBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "UPS")
        {

        }

        /// <summary>
        /// Gets a list of Ups rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <param name="exceptionHandler">Action that performs exception handling</param>
        /// <returns>List of NoncompetitiveRateResults</returns>
        /// <remarks>This is overridden because Ups has a requirement that we have to hide their branding if
        /// other carriers rates are present</remarks>
        public override List<RateResult> GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            return base.GetBestRates(shipment, exceptionHandler).Select(x => new NoncompetitiveRateResult(x)).ToList<RateResult>();
        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider)settings.UpsInsuranceProvider;
        }

        /// <summary>
        /// Applies Ups specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, UpsAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.Ups.Packages[0].DimsHeight = currentShipment.BestRate.DimsHeight;
            currentShipment.Ups.Packages[0].DimsWidth = currentShipment.BestRate.DimsWidth;
            currentShipment.Ups.Packages[0].DimsLength = currentShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.Ups.Packages[0].Weight = originalShipment.ContentWeight;
            currentShipment.Ups.Packages[0].DimsAddWeight = false;
            currentShipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.Custom;
            currentShipment.Ups.Service = (int)UpsServiceType.UpsGround;
            currentShipment.Ups.UpsAccountID = account.UpsAccountID;

            currentShipment.Ups.Packages[0].Insurance = originalShipment.Insurance;
            currentShipment.Ups.Packages[0].InsuranceValue = originalShipment.BestRate.InsuranceValue;
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        /// <param name="tag">Ups service type from the rate tag</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            UpsServiceType serviceType = (UpsServiceType)tag;
            return serviceType == UpsServiceType.UpsSurePostBoundPrintedMatter || serviceType == UpsServiceType.UpsSurePostMedia;
        }

        /// <summary>
        /// Creates and attaches a new instance of a UpsShipment to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.Ups = new UpsShipmentEntity();
        }

        /// <summary>
        /// Sets the service type on the Ups shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.Ups.Service = (int) tag;
        }

        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.Ups.Fields.State;
        }

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected override IList<UpsPackageEntity> Packages(ShipmentEntity shipment)
        {
            return shipment.Ups.Packages;
        }

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected override long PackageId(UpsPackageEntity package)
        {
            return package.UpsPackageID;
        }

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected override void SetPackageId(UpsPackageEntity package, long packageId)
        {
            package.UpsPackageID = packageId;
        }
    }
}
