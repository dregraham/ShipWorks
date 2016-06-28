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
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlgFactory"/> class.
        /// </summary>
        /// <param name="columnSourceFactory"></param>
        public OdbcCustomQueryDlgFactory(IWin32Window owner, IOdbcSampleDataCommand sampleDataCommand, Func<Type, ILog> logFactory)
        {
            this.owner = owner;
            this.sampleDataCommand = sampleDataCommand;
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

            dlg.DataContext = new OdbcCustomQueryDlgViewModel(dataSource, sampleDataCommand, columnSource, messageHelper, logFactory);

            dlg.ShowDialog();
        }
    }
}