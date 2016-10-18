using Interapptive.Shared.UI;
using System;
using System.Windows;
using System.Windows.Interop;


namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Interaction logic for DismissableWebBrowserDlg.xaml
    /// </summary>
    public partial class DismissableWebBrowserDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DismissableWebBrowserDlg"/> class.
        /// </summary>
        public DismissableWebBrowserDlg()
        {
            InitializeComponent();
        }
       
        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Loads the owner.
        /// </summary>
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
