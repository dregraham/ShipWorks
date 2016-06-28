using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    ///
    /// </summary>
    public interface IOdbcCustomQueryDlgFactory
    {
        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        OdbcCustomQueryDlg CreateCustomQueryDlg(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery);
    }
}