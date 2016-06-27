using System;
using System.Windows.Interop;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Interaction logic for OdbcCustomQueryDlg.xaml
    /// </summary>
    public partial class OdbcCustomQueryDlg : IWin32Window, IDialog
    {

        public OdbcCustomQueryDlg()
        {
            InitializeComponent();
        }

        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }

        public IntPtr Handle { get; private set; }
    }
}
