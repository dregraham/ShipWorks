using System.Windows.Controls;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// Interaction logic for OrderLookupSearchControl.xaml
    /// </summary>
    public partial class OrderLookupSearchControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSearchControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set searchbox focus
        /// </summary>
        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchBox.Focus();
        }
    }
}
