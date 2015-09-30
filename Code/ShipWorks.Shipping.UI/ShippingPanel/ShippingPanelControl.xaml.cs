using System.Windows.Controls;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Interaction logic for ShipmentPanelControl.xaml
    /// </summary>
    public partial class ShippingPanelControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelControl(ShippingPanelViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
