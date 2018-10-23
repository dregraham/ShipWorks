using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.OrderLookup.Controls.OrderConfirmationDialog
{
    public partial class OrderConfirmationDialog 
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public OrderConfirmationDialog(IWin32Window owner, object viewModel) : base(owner, viewModel, false)
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
