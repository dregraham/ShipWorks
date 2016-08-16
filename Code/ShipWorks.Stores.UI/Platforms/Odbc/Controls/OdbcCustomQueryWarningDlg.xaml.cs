using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using Interapptive.Shared.UI;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// Interaction logic for OdbcCustomQueryWarningDlg.xaml
    /// </summary>
    public partial class OdbcCustomQueryWarningDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryWarningDlg"/> class.
        /// </summary>
        public OdbcCustomQueryWarningDlg(IWin32Window owner)
        {
            InitializeComponent();
            LoadOwner(owner);
        }
         
        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Closes the window.
        /// </summary>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Opens the link.
        /// </summary>
        private void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Loads the owner
        /// </summary>
        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            var interopHelper = new WindowInteropHelper(this);
            interopHelper.Owner = owner.Handle;
        }
    }
}
