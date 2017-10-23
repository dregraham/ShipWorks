using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DHL Express Shipment Adapter
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentAdapter), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private DhlExpressShipmentAdapter(DhlExpressShipmentAdapter adapterToCopy) :
            base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager) :
            base(shipment, shipmentTypeManager, customsManager, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.DhlExpress.DhlExpressAccountID; }
            set { Shipment.DhlExpress.DhlExpressAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.DhlExpress.Service; }
            set { Shipment.DhlExpress.Service = value; }
        }

        /// <summary>
        /// Add a new package to the shipment
        /// </summary>
        public override IPackageAdapter AddPackage()
        {
            DhlExpressPackageEntity package = DhlExpressShipmentType.CreateDefaultPackage();
            Shipment.DhlExpress.Packages.Add(package);
            UpdateDynamicData();

            return new DhlExpressPackageAdapter(Shipment, package, Shipment.DhlExpress.Packages.IndexOf(package) + 1);
        }

        /// <summary>
        /// Delete a package from the shipment
        /// </summary>
        public override void DeletePackage(IPackageAdapter packageAdapter)
        {
            DeletePackageFromCollection(Shipment.DhlExpress.Packages, x => x.DhlExpressPackageID == packageAdapter.PackageId);
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            Shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            string serviceName = tag as string;
            if (serviceName == string.Empty)
            {
                return base.GetServiceTypeAsIntFromTag(tag);
            }

            return (int) EnumHelper.GetEnumByApiValue<DhlExpressServiceType>(serviceName);
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            int? service = GetServiceTypeAsIntFromTag(rate.Tag);
            if (service.HasValue)
            {
                Shipment.DhlExpress.Service = service.Value;
            }
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        public override ICarrierShipmentAdapter Clone() => new DhlExpressShipmentAdapter(this);
    }
}
