using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private iParcelShipmentAdapter(iParcelShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager) :
            base(shipment, shipmentTypeManager, customsManager, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.IParcel, nameof(shipment.IParcel));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.IParcel.IParcelAccountID; }
            set { Shipment.IParcel.IParcelAccountID = value.GetValueOrDefault(); }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => false;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType
        {
            get { return Shipment.IParcel.Service; }
            set { Shipment.IParcel.Service = value; }
        }

        /// <summary>
        /// Add a new package to the shipment
        /// </summary>
        public override IPackageAdapter AddPackage()
        {
            IParcelPackageEntity package = iParcelShipmentType.CreateDefaultPackage();
            Shipment.IParcel.Packages.Add(package);
            UpdateDynamicData();

            return new iParcelPackageAdapter(Shipment, package, Shipment.IParcel.Packages.IndexOf(package) + 1);
        }

        /// <summary>
        /// Delete a package from the shipment
        /// </summary>
        public override void DeletePackage(IPackageAdapter packageAdapter)
        {
            DeletePackageFromCollection(Shipment.IParcel.Packages, x => x.IParcelPackageID == packageAdapter.PackageId);
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // If there is more than one package, only declared value is allowed, so just return.
            if (Shipment.IParcel.Packages.Count > 1)
            {
                return;
            }

            if (Shipment.InsuranceProvider != shippingSettings.IParcelInsuranceProvider)
            {
                Shipment.InsuranceProvider = shippingSettings.IParcelInsuranceProvider;
            }

            foreach (IParcelPackageEntity packageEntity in Shipment.IParcel.Packages)
            {
                if (packageEntity.InsurancePennyOne != shippingSettings.IParcelInsurancePennyOne)
                {
                    packageEntity.InsurancePennyOne = shippingSettings.IParcelInsurancePennyOne;
                }
            }
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            iParcelServiceType? service = tag as iParcelServiceType? ?? (tag as iParcelRateSelection)?.ServiceType;

            return service.HasValue ? (int?) service.Value : base.GetServiceTypeAsIntFromTag(tag);
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            int? service = GetServiceTypeAsIntFromTag(rate.Tag);
            if (service.HasValue)
            {
                Shipment.IParcel.Service = service.Value;
            }
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new iParcelShipmentAdapter(this);
    }
}
