using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.iParcel.BestRate
{
    public class iParcelBestRateBroker : PackageBasedBestRateBroker<IParcelAccountEntity, IParcelPackageEntity>
    {
        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a IParcel shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get IParcel accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public iParcelBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<IParcelAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "iParcel")
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider)settings.IParcelInsuranceProvider;
        }

        /// <summary>
        /// Applies IParcel specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, IParcelAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.IParcel.Packages[0].DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.IParcel.Packages[0].DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.IParcel.Packages[0].DimsLength = originalShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.IParcel.Packages[0].Weight = originalShipment.ContentWeight;
            currentShipment.IParcel.Packages[0].DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.IParcel.Packages[0].DimsWeight = originalShipment.BestRate.DimsWeight;

            // Update total weight 
            ShipmentType.UpdateTotalWeight(currentShipment);

            currentShipment.IParcel.Service = (int)iParcelServiceType.Saver;
            currentShipment.IParcel.IParcelAccountID = account.IParcelAccountID;

            currentShipment.IParcel.Packages[0].Insurance = currentShipment.Insurance;
            currentShipment.IParcel.Packages[0].InsuranceValue = currentShipment.BestRate.InsuranceValue;
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        /// <param name="tag">IParcel service type from the rate tag</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            return false;
        }

        /// <summary>
        /// Creates and attaches a new instance of a IParcelShipment to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.IParcel = new IParcelShipmentEntity();
        }

        /// <summary>
        /// Gets the service type from tag.
        /// </summary>
        protected override int GetServiceTypeFromTag(object tag)
        {
            return (int) ((iParcelRateSelection) tag).ServiceType;
        }

        /// <summary>
        /// Sets the service type on the IParcel shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.IParcel.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.IParcel.Fields.State;
        }

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected override IList<IParcelPackageEntity> Packages(ShipmentEntity shipment)
        {
            return shipment.IParcel.Packages;
        }

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected override long PackageId(IParcelPackageEntity package)
        {
            return package.IParcelPackageID;
        }

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected override void SetPackageId(IParcelPackageEntity package, long packageId)
        {
            package.IParcelPackageID = packageId;
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(IParcelAccountEntity account)
        {
            return account.Description;
        }
    }
}
