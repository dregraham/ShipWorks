using Autofac.Core;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    public interface IShipmentTypeSetupWizardFactory
    {
        /// <summary>
        /// Create a wizard for the given shipment type
        /// </summary>
        IShipmentTypeSetupWizard Create(ShipmentTypeCode shipmentType, OpenedFromSource openedFrom, params Parameter[] parameters);
    }
}
