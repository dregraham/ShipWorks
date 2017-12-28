using System;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Shipping.UI.Carriers.Postal.Usps
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    [Component(Service = typeof(IFirstClassInternationalWarningDialog))]
    public partial class FirstClassInternationalWarningDialog : Window, IFirstClassInternationalWarningDialog
    {
        public FirstClassInternationalWarningDialog()
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
        private void CancelAndClose(object sender, RoutedEventArgs e)
        {            
            Close();
        }

        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void AgreeAndClose(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Loads the owner.
        /// </summary>
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
