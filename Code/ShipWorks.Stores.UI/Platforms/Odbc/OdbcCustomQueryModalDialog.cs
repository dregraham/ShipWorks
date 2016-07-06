using Interapptive.Shared.UI;
using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Used to display a CustomQueryModalDialog window
    /// </summary>
    public class OdbcCustomQueryModalDialog : IOdbcCustomQueryModalDialog
    {
        private readonly Func<IOdbcDataSource, OdbcStoreEntity, IOdbcCustomQueryDlgViewModel> odbcCustomQueryDlgViewModelFactory;
        private readonly Func<string, IDialog> dialogFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryModalDialog"/> class.
        /// </summary>
        public OdbcCustomQueryModalDialog(
            Func<IOdbcDataSource, OdbcStoreEntity, IOdbcCustomQueryDlgViewModel> odbcCustomQueryDlgViewModelFactory,
            Func<string, IDialog> dialogFactory)
        {
            this.odbcCustomQueryDlgViewModelFactory = odbcCustomQueryDlgViewModelFactory;
            this.dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        public bool? Show(IOdbcDataSource dataSource, OdbcStoreEntity store)
        {
            IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
            warningDlg.ShowDialog();

            IDialog customQueryDlg = dialogFactory("OdbcCustomQueryDlg");

            IOdbcCustomQueryDlgViewModel context = odbcCustomQueryDlgViewModelFactory(dataSource, store);
            customQueryDlg.DataContext = context;
            return customQueryDlg.ShowDialog();
        }
    }
}