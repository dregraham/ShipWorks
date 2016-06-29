using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Odbc;
using System;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Used to display a CustomQueryModalDialog window
    /// </summary>
    public class OdbcCustomQueryModalDialog : IOdbcCustomQueryModalDialog
    {
        private readonly Func<IOdbcDataSource, IOdbcColumnSource, IOdbcCustomQueryDlgViewModel> odbcCustomQueryDlgViewModelFactory;
        private readonly Func<string, IDialog> dialogFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryModalDialog"/> class.
        /// </summary>
        public OdbcCustomQueryModalDialog(
            Func<IOdbcDataSource, IOdbcColumnSource, IOdbcCustomQueryDlgViewModel> odbcCustomQueryDlgViewModelFactory,
            Func<string, IDialog> dialogFactory)
        {
            this.odbcCustomQueryDlgViewModelFactory = odbcCustomQueryDlgViewModelFactory;
            this.dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        public void Show(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery)
        {
            IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
            warningDlg.ShowDialog();

            IDialog customQueryDlg = dialogFactory("OdbcCustomQueryDlg");
            columnSource.Query = customQuery;

            IOdbcCustomQueryDlgViewModel context = odbcCustomQueryDlgViewModelFactory(dataSource, columnSource);
            customQueryDlg.DataContext = context;
            customQueryDlg.ShowDialog();
        }
    }
}