using System.Diagnostics;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Interaction logic for YahooApiAccountSettings.xaml
    /// </summary>
    public partial class YahooApiAccountSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountSettings"/> class.
        /// </summary>
        public YahooApiAccountSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the Help Link
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
