using System.Windows.Controls;

namespace ShipWorks.Shipping.UI
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

        ///// <summary>
        ///// Model associated with this view
        ///// </summary>
        //public ShippingPanelViewModel ViewModel
        //{
        //    get { return (ShippingPanelViewModel)DataContext; }
        //    set { DataContext = value; }
        //}
    }
}
