using System.Windows.Forms;
using ShipWorks.Products.ProductEditor;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// Dialog for editing products
    /// </summary>
    public partial class ProductEditorDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductEditorDialog(IWin32Window owner) : base(owner, false)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the Closing event
        /// </summary>
        private void HandleClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If the user can't edit, it means there's a request in flight so we don't want to allow closing
            e.Cancel = !((DataContext as ProductEditorViewModel)?.CanEdit).GetValueOrDefault(true);
        }
    }
}
