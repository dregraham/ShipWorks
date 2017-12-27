﻿using System;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Shipping.UI.Carriers.Postal.Usps
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    [KeyedComponent(typeof(IDialog), "FirstClassInternationalWarningDlg")]
    public partial class FirstClassInternationalWarningDlg : Window, IDialog
    {
        public FirstClassInternationalWarningDlg()
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
