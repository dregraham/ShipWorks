using System.Windows.Controls;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Interaction logic for RatingPanelControl.xaml
    /// </summary>
    public partial class RatingPanelControl : UserControl
    {
        public RatingPanelControl(RatingPanelViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
