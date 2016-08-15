using System;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.UI;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// Interaction logic for CustomerLicenseActivationDlg.xaml
    /// </summary>
    public partial class CustomerLicenseActivationDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerLicenseActivationDlg"/> class.
        /// </summary>
        public CustomerLicenseActivationDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Called when [click cancel].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
