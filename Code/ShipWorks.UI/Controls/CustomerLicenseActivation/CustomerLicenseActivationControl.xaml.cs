using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// Interaction logic for TangoUserControl.xaml
    /// </summary>
    public partial class CustomerLicenseActivationControl : UserControl
    {
        public CustomerLicenseActivationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens hyperlink in default browser
        /// </summary>
        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

    }
}