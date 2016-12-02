using System.Diagnostics;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.Magento.WizardPages
{
    /// <summary>
    /// Interaction logic for MagentoStoreSetupWizardPage.xaml
    /// </summary>
    public partial class MagentoStoreSetupControl
    {
        public MagentoStoreSetupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the link in browser
        /// </summary>
        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
