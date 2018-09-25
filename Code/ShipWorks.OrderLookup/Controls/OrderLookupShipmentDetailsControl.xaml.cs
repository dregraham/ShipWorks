using System.ComponentModel;
using System.Windows.Controls;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.OrderLookup.Controls
{
    public partial class OrderLookupShipmentDetailsControl : UserControl
    {
        public OrderLookupShipmentDetailsControl()
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
