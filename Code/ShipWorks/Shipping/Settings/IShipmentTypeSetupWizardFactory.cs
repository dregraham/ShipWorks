using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    public interface IShipmentTypeSetupWizardFactory : IFactory<ShipmentTypeCode, IShipmentTypeSetupWizard>
    {
    }
}
