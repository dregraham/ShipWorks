using System;
using System.Windows.Interop;
using Interapptive.Shared.UI;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// Interaction logic for AutoPrintConfirmationDlg.xaml
    /// </summary>
    public partial class AutoPrintConfirmationDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintConfirmationDlg"/> class.
        /// </summary>
        public AutoPrintConfirmationDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Loads the owner
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
