using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Shipment Details for the PostalShipmentDetailsControl
    /// </summary>
    public partial class BestRateShipmentDetailsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShipmentDetailsControl()
        {
            InitializeComponent();
            Loaded += OnControlLoaded;
        }

        /// <summary>
        /// Handles the control load event
        /// </summary>
        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            Provider.Focus();
        }
    }
}
