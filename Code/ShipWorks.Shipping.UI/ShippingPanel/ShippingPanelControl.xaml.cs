using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Shipping.UI.ShippingPanel
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

            // To support selecting all text in the text box when tabbing into it, register the got focus event
            // to select all text for text boxes
            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotFocusEvent,
                new RoutedEventHandler(OnTextBoxGotFocus));
        }

        /// <summary>
        /// Event fired when a text box gets focus.  Selects all the text in the box.
        /// </summary>
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }
    }
}
