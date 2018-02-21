using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Services.Dialogs
{
    /// <summary>
    /// Interface that represents a factory for creating the ShippingProfileManagerDialog
    /// </summary>
    public interface IShippingProfileManagerDialogFactory
    {
        /// <summary>
        /// Create the ShippingProfileManagerDialog
        /// </summary>
        IDialog Create();
    }
}
