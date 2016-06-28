using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcCustomQueryDlgFactory : IOdbcCustomQueryDlgFactory
    {
        private readonly IWin32Window owner;
        private readonly IOdbcColumnSourceFactory columnSourceFactory;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlgFactory"/> class.
        /// </summary>
        public OdbcCustomQueryDlgFactory(IWin32Window owner, IOdbcColumnSourceFactory columnSourceFactory,
            IShipWorksDbProviderFactory dbProviderFactory, Func<Type, ILog> logFactory)
        {
            this.owner = owner;
            this.columnSourceFactory = columnSourceFactory;
            this.dbProviderFactory = dbProviderFactory;
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        public void ShowCustomQueryDlg(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery, IMessageHelper messageHelper)
        {
            OdbcCustomQueryDlg dlg = new OdbcCustomQueryDlg();
            dlg.LoadOwner(owner);

            columnSource.Query = customQuery;

            dlg.DataContext = new OdbcCustomQueryDlgViewModel(dataSource, dbProviderFactory, columnSource, messageHelper, logFactory);

            dlg.ShowDialog();
        }
    }
}