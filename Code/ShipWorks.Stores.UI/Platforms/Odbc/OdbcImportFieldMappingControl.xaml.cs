namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Interaction logic for OdbcImportFieldMappingControl.xaml
    /// </summary>
    public partial class OdbcImportFieldMappingControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControl"/> class.
        /// </summary>
        public OdbcImportFieldMappingControl()
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
