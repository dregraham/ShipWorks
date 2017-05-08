using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    [Component]
    public class ShipmentTypeSetupWizardFactory : IShipmentTypeSetupWizardFactory
    {
        private readonly IIndex<ShipmentTypeCode, ShipmentTypeSetupWizardForm> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeSetupWizardFactory(IIndex<ShipmentTypeCode, ShipmentTypeSetupWizardForm> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Create a wizard for the given shipment type
        /// </summary>
        public IShipmentTypeSetupWizard Create(ShipmentTypeCode shipmentType)
        {
            ShipmentTypeSetupWizardForm wizard = null;
            return lookup.TryGetValue(shipmentType, out wizard) ? wizard : null;
        }
    }
}
