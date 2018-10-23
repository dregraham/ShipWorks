using System;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Shipment Details for the PostalShipmentDetailsControl
    /// </summary>
    public partial class PostalShipmentDetailsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalShipmentDetailsControl()
        {
            InitializeComponent();
            Loaded += OnControlLoaded;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            Provider.Focus();
        }
    }
}
