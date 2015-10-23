using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Shipping.UI.ShippingPanel.AddressControl
{
    /// <summary>
    /// Interaction logic for AddressControl.xaml
    /// </summary>
    public partial class AddressControl : UserControl
    {
        public AddressControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close the popup when a suggestion button was clicked
        /// </summary>
        /// <remarks>While this is code in the view, it is purely related to view functionality and trying to 
        /// reproduce the effect in XAML would require much more than this.</remarks>
        private void OnAddressSuggestionButtonClick(object sender, RoutedEventArgs e) => AddressPopup.IsOpen = false;
    }
}
