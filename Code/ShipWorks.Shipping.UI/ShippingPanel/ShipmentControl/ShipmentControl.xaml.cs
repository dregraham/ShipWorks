using System.Windows.Controls;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Interaction logic for ShipmentControl.xaml
    /// </summary>
    public partial class ShipmentControl : UserControl
    {
        public ShipmentControl()
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
