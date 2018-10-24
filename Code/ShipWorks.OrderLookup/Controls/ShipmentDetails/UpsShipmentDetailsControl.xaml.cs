using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Interaction logic for OrderLookupUpsShipmentDetailsControl.xaml
    /// </summary>
    public partial class UpsShipmentDetailsControl : UserControl
    {
        public UpsShipmentDetailsControl()
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
