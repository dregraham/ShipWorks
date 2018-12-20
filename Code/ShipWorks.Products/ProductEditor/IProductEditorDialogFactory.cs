using Interapptive.Shared.UI;

namespace ShipWorks.Products.ProductEditor
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
