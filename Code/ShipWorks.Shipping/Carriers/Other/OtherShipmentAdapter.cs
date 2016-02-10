using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Other
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class OtherShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private OtherShipmentAdapter(OtherShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OtherShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the other account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used",
            Justification = "Other shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "Other shipment types don't have accounts")]
        public override long? AccountId
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => false;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType { get; set; } = 0;

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // Nothing to do as Other is only allowed to use ShipWorks insurance
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new OtherShipmentAdapter(this);
    }
}
