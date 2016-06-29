using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    ///Interface for Modal Dialog that retrieves a custom query
    /// </summary>
    public interface IOdbcCustomQueryModalDialog
    {
        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        void Show(IOdbcDataSource dataSource, IOdbcColumnSource columnSource, string customQuery);
    }
}