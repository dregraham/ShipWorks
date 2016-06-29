using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Used to display a CustomQueryModalDialog window
    /// </summary>
    public class OdbcCustomQueryModalDialog : IOdbcCustomQueryModalDialog
    {
        private readonly IWin32Window owner;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryModalDialog"/> class.
        /// </summary>
        public OdbcCustomQueryModalDialog(IWin32Window owner, IOdbcSampleDataCommand sampleDataCommand, Func<Type, ILog> logFactory)
        {
            this.owner = owner;
            this.sampleDataCommand = sampleDataCommand;
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        public void Show(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery, IMessageHelper messageHelper)
        {
            OdbcCustomQueryDlg dlg = new OdbcCustomQueryDlg();
            dlg.LoadOwner(owner);
            columnSource.Query = customQuery;

            using (var context = new OdbcCustomQueryDlgViewModel(dataSource, sampleDataCommand, columnSource, messageHelper, logFactory))
            {
                dlg.DataContext = context;

                dlg.ShowDialog();
            }

        }
    }
}