using Interapptive.Shared.UI;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// Represents a factory for getting the ProductEditorDialog
    /// </summary>
    public interface IProductEditorDialogFactory
    {
        /// <summary>
        /// Create the dialog
        /// </summary>
        IDialog Create();
    }
}
