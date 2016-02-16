using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Adapter for FedEx specific shipment information
    /// </summary>
    public class FedExShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private FedExShipmentAdapter(FedExShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
            MethodConditions.EnsureArgumentIsNotNull(shipment.FedEx, nameof(shipment.FedEx));
        }

        /// <summary>
        /// Id of the FedEx account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.FedEx.FedExAccountID; }
            set { Shipment.FedEx.FedExAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.FedEx.Service; }
            set { Shipment.FedEx.Service = value; }
        }

        /// <summary>
        /// Add a new package to the shipment
        /// </summary>
        public override IPackageAdapter AddPackage()
        {
            FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
            Shipment.FedEx.Packages.Add(package);
            UpdateDynamicData();

            return new FedExPackageAdapter(Shipment, package, Shipment.FedEx.Packages.IndexOf(package) + 1);
        }

        /// <summary>
        /// Delete a package from the shipment
        /// </summary>
        public override void DeletePackage(IPackageAdapter packageAdapter)
        {
            DeletePackageFromCollection(Shipment.FedEx.Packages, x => x.FedExPackageID == packageAdapter.PackageId);
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // If there is more than one package, only declared value is allowed, so just return.
            if (Shipment.FedEx.Packages.Count() > 1)
            {
                return;
            }

            if (Shipment.InsuranceProvider != shippingSettings.FedExInsuranceProvider)
            {
                Shipment.InsuranceProvider = shippingSettings.FedExInsuranceProvider;
            }

            foreach (FedExPackageEntity packageEntity in Shipment.FedEx.Packages)
            {
                if (packageEntity.InsurancePennyOne != shippingSettings.FedExInsurancePennyOne)
                {
                    packageEntity.InsurancePennyOne = shippingSettings.FedExInsurancePennyOne;
                }
            }
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            FedExServiceType? service = tag as FedExServiceType? ?? (tag as FedExRateSelection)?.ServiceType;

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
                Shipment.FedEx.Service = service.Value;
            }
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new FedExShipmentAdapter(this);
    }
}
