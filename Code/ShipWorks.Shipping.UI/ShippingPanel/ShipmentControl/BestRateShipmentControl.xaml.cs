using System.Windows.Controls;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Interaction logic for BestRateShipmentControl.xaml
    /// </summary>
    public partial class BestRateShipmentControl : UserControl
    {
        public BestRateShipmentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the manage dimensions profiles click
        /// </summary>
        private void OnManageDimensionsProfiles(object sender, System.Windows.RoutedEventArgs e)
        {
            using (DimensionsManagerDlg dlg = new DimensionsManagerDlg())
            {
                dlg.ShowDialog();
            }
        }
    }
}
