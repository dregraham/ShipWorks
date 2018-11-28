using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// Factory for creating the product editor dialog
    /// </summary>
    [Component]
    public class ProductEditorDialogFactory : IProductEditorDialogFactory
    {
        /// <summary>
        /// Create a ProductEditorDialog
        /// </summary>
        /// <returns></returns>
        public IDialog Create()
        {
            return new ProductEditorDialog();
        }
    }
}
