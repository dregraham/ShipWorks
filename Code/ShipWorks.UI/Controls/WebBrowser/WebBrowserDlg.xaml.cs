using System.Windows;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Interaction logic for WebBrowserDlg.xaml
    /// </summary>
    public partial class WebBrowserDlg : IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowserDlg"/> class.
        /// </summary>
        public WebBrowserDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
