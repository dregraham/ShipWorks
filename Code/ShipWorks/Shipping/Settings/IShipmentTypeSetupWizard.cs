using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Wizard for setting up a shipment type
    /// </summary>
    public interface IShipmentTypeSetupWizard : IForm
    {
        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        IShipmentTypeSetupWizard GetUnwrappedWizard();
    }
}