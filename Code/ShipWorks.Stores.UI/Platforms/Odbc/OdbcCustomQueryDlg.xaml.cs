using Interapptive.Shared.UI;
using System;
using System.Windows.Interop;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Interaction logic for OdbcCustomQueryDlg.xaml
    /// </summary>
    public partial class OdbcCustomQueryDlg : IWin32Window, IDialog
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlg"/> class.
        /// </summary>
        public OdbcCustomQueryDlg()
        {
            InitializeComponent();
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

        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        public IntPtr Handle { get; private set; }
    }
}
