using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Interaction logic for ShipmentPanelControl.xaml
    /// </summary>
    public partial class ShipmentPanelControl : UserControl
    {
        public ShipmentPanelControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Hello!");
        }
    }
}
