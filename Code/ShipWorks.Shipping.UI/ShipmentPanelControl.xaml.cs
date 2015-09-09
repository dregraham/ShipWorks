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

        public object ViewModel { get { return (object)DataContext; } set { DataContext = value; } }
    }
}
