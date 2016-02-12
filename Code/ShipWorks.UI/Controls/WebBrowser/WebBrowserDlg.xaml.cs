using System.Windows;
using System.Windows.Interop;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

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
        /// Constructor
        /// </summary>
        public WebBrowserDlg(IWin32Window owner) : this()
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };

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
