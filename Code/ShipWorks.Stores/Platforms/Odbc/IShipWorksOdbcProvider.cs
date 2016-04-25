using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface for getting odbc resources, can be mocked up for testing
    /// </summary>
    interface IShipWorksOdbcProvider
    {
        /// <summary>
        /// Creates and Odbc DbConnection
        /// </summary>
        DbConnection CreateOdbcConnection();
    }
}