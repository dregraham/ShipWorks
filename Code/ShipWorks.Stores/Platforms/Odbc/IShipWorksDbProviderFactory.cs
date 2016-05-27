using System.Data;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface for getting odbc resources, can be mocked up for testing
    /// </summary>
    public interface IShipWorksDbProviderFactory
    {
        /// <summary>
        /// Creates an Odbc DbConnection
        /// </summary>
        IDbConnection CreateOdbcConnection();

        /// <summary>
        /// Creates an Odbc DbConnection with a given connection string
        /// </summary>
        IDbConnection CreateOdbcConnection(string connectionString);
    }
}