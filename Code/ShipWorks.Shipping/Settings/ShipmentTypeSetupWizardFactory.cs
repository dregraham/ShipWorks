using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Configuration;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    [Component]
    public class ShipmentTypeSetupWizardFactory : IShipmentTypeSetupWizardFactory
    {
        private readonly IIndex<ShipmentTypeCode, ShipmentTypeSetupWizardForm> lookup;
        private readonly Func<IShipmentTypeSetupWizard, ShipmentTypeCode, OpenedFromSource, TelemetricShipmentTypeSetupWizardForm> wrapWithTelemetry;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeSetupWizardFactory(IIndex<ShipmentTypeCode, ShipmentTypeSetupWizardForm> lookup,
            Func<IShipmentTypeSetupWizard, ShipmentTypeCode, OpenedFromSource, TelemetricShipmentTypeSetupWizardForm> wrapWithTelemetry)
        {
            this.wrapWithTelemetry = wrapWithTelemetry;
            this.lookup = lookup;
        }

        /// <summary>
        /// Create a wizard for the given shipment type
        /// </summary>
        public IShipmentTypeSetupWizard Create(ShipmentTypeCode shipmentType, OpenedFromSource openedFrom)
        {
            ShipmentTypeSetupWizardForm wizard = null;
            if (lookup.TryGetValue(shipmentType, out wizard))
            {
                return wrapWithTelemetry(wizard, shipmentType, openedFrom);
            }

            return null;
        }
    }
}
