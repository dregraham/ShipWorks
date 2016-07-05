using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

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
        bool? Show(IOdbcDataSource dataSource, IOdbcColumnSource columnSource);
    }
}