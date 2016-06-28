using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using log4net;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Controls.ChannelLimit;
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
        /// <param name="owner">The default owner.</param>
        /// <param name="columnSourceFactory"></param>
        /// <param name="dbProviderFactory"></param>
        /// <param name="logFactory"></param>
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
        public OdbcCustomQueryDlg CreateCustomQueryDlg(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery)
        {
            OdbcCustomQueryDlg dlg = new OdbcCustomQueryDlg();
            dlg.LoadOwner(owner);



            dlg.DataContext = new OdbcCustomQueryDlgViewModel(dataSource, dbProviderFactory, columnSource, logFactory);
            ((IOdbcCustomQueryDlgViewModel) dlg.DataContext).Load(customQuery);

            return dlg;
        }

    }
}