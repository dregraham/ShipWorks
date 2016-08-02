using System.Diagnostics;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls.Upload
{
    /// <summary>
    /// Interaction logic for OdbcUploadMapSettingsControl.xaml
    /// </summary>
    public partial class OdbcUploadMapSettingsControl
    {
        public OdbcUploadMapSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the link.
        /// </summary>
        private void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
