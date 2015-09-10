using System.Windows.Controls;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Interaction logic for ShipmentPanelControl.xaml
    /// </summary>
    public partial class ShipmentPanelControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPanelControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Model associated with this view
        /// </summary>
        public ShipmentPanelViewModel ViewModel
        {
            get { return (ShipmentPanelViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
