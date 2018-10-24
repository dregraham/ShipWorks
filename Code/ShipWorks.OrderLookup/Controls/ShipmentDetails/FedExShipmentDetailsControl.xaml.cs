using System.Windows;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Shipment details control for FedEx
    /// </summary>
    public partial class FedExShipmentDetailsControl
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public FedExShipmentDetailsControl()
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
