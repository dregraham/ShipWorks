using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcCustomQueryDlgFactory : IOdbcCustomQueryDlgFactory
    {
        private readonly IWin32Window owner;

        public OdbcCustomQueryDlgFactory(IWin32Window owner)
        {
            this.owner = owner;
        }

        public void ShowCustomQueryDlg()
        {
            OdbcCustomQueryDlg dlg = new OdbcCustomQueryDlg();
            dlg.LoadOwner(owner);
            dlg.DataContext = new OdbcCustomQueryDlgViewModel();
            dlg.ShowDialog();
        }
    }
}