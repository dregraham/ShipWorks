namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Interaction logic for RatingPanelControl.xaml
    /// </summary>
    public partial class RatingPanelControl
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public RatingPanelControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ctor that loads the view model
        /// </summary>
        public RatingPanelControl(RatingPanelViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
