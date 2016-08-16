using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls.Import
{
    /// <summary>
    /// Interaction logic for OdbcImportMapSettingsControl.xaml
    /// </summary>
    public partial class OdbcImportMapSettingsControl
    {
        public OdbcImportMapSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegates scrolling of the datagrid to the scrollviewer
        /// </summary>
        private void MouseWheelScrolled(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - (float) e.Delta / 3);
        }
    }
}
