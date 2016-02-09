using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Adapter for Ups specific shipment information
    /// </summary>
    public class UpsShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Ups, nameof(shipment.Ups));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the Ups account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.Ups.UpsAccountID; }
            set { Shipment.Ups.UpsAccountID = value.GetValueOrDefault(); }
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
        public override bool SupportsPackageTypes => true;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType
        {
            get { return Shipment.Ups.Service; }
            set { Shipment.Ups.Service = value; }
        }

        /// <summary>
        /// Add a new package to the shipment
        /// </summary>
        public override IPackageAdapter AddPackage()
        {
            UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
            Shipment.Ups.Packages.Add(package);

            UpdateDynamicData();

            return new UpsPackageAdapter(Shipment, package, Shipment.Ups.Packages.IndexOf(package) + 1);
        }

        /// <summary>
        /// Delete a package from the shipment
        /// </summary>
        public override void DeletePackage(IPackageAdapter packageAdapter)
        {
            DeletePackageFromCollection(Shipment.Ups.Packages, x => x.UpsPackageID == packageAdapter.PackageId);
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // If there is more than one package, only declared value is allowed, so just return.
            if (Shipment.Ups.Packages.Count() > 1)
            {
                return;
            }

            if (Shipment.InsuranceProvider != shippingSettings.UpsInsuranceProvider)
            {
                Shipment.InsuranceProvider = shippingSettings.UpsInsuranceProvider;
            }

            foreach (UpsPackageEntity packageEntity in Shipment.Ups.Packages)
            {
                if (packageEntity.InsurancePennyOne != shippingSettings.UpsInsurancePennyOne)
                {
                    packageEntity.InsurancePennyOne = shippingSettings.UpsInsurancePennyOne;
                }
            }
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            UpsServiceType? service = tag as UpsServiceType?;

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
                Shipment.Ups.Service = service.Value;
            }
        }
    }
}
