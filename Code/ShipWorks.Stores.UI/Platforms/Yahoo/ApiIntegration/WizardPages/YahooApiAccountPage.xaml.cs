using System.Diagnostics;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// Interaction logic for YahooApiAccountSettings.xaml
    /// </summary>
    public partial class YahooApiAccountPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountPage"/> class.
        /// </summary>
        public YahooApiAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the Help Link
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
