using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.OrderLookup.Controls.OrderConfirmationDialog
{
    /// <summary>
    /// Dialog for confirming orders
    /// </summary>
    public partial class OrderConfirmationDialog : IOrderConfirmationDialog
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public OrderConfirmationDialog(IWin32Window owner, IOrderConfirmationViewModel viewModel) : base(owner, viewModel, false)
        {
            InitializeComponent();
        }

        /// <summary>
        /// When select is clicked, set the dialog result to true and close.
        /// </summary>
        private void OnClickSelect(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
