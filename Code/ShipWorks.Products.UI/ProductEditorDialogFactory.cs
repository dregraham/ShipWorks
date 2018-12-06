using System.Windows.Forms;
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
        private readonly IWin32Window owner;

        public ProductEditorDialogFactory(IWin32Window owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Create a ProductEditorDialog
        /// </summary>
        public IDialog Create()
        {
            return new ProductEditorDialog(owner);
        }
    }
}
