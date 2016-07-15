namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Interaction logic for OdbcMappingControl.xaml
    /// </summary>
    public partial class OdbcMappingControl
    {
        public OdbcMappingControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When a new map is selected, reset the mapping grid scrollbar to the top
        /// </summary>
        private void SelectedFieldMapChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MappingGridScrollbar.ScrollToTop();
        }
    }
}
