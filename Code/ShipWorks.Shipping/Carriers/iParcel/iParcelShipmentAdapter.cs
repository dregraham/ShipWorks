using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

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
        /// Constructor
        /// </summary>
        public iParcelShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
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
            if (Shipment.IParcel.Packages.Count < 2)
            {
                return;
            }

            IParcelPackageEntity package = Shipment.IParcel.Packages
                .FirstOrDefault(x => x.IParcelPackageID == packageAdapter.PackageId);

            if (package != null)
            {
                Shipment.IParcel.Packages.Remove(package);
                UpdateDynamicData();
            }
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
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            if (rate.Tag is int)
            {
                Shipment.IParcel.Service = (int) rate.Tag;
            }
        }
    }
}
