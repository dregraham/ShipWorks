using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SparkPayAccountControl : UserControl
    {
        public SparkPayAccountControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the Help Link
        /// </summary>
        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
