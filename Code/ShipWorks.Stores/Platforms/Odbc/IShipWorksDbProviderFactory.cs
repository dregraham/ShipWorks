using System.Data;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface for getting odbc resources, can be mocked up for testing
    /// </summary>
    public interface IShipWorksDbProviderFactory
    {
        /// <summary>
        /// Creates and Odbc DbConnection
        /// </summary>
        IDbConnection CreateOdbcConnection();
    }
}