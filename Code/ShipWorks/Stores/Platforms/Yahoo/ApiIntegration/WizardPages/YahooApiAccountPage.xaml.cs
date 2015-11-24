using System.Diagnostics;
using System.Windows.Navigation;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// Interaction logic for YahooApiAccountSettings.xaml
    /// </summary>
    public partial class YahooApiAccountSettings 
    {
        public YahooApiAccountSettings()
        {
            InitializeComponent();
            
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
