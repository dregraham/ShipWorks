using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    public interface IShipmentTypeSetupWizardFactory
    {
        IShipmentTypeSetupWizard Create(ShipmentTypeCode shipmentType, OpenedFromSource openedFrom);
    }
}
