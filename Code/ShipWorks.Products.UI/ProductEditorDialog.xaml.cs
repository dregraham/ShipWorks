using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.Products.UI
{
    [Component]
    public partial class ProductEditorDialog : IProductEditorDialog
    {
        public ProductEditorDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
