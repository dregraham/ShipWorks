using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Class for obtaining odbc resources with interface that can be mocked up for testing purposes
    /// </summary>
    public class OdbcShipWorksDbProviderFactory : IShipWorksDbProviderFactory
    {
        /// <summary>
        /// Returns an OdbcConnection
        /// </summary>
        public DbConnection CreateOdbcConnection()
        {
            return DbProviderFactories.GetFactory("System.Data.Odbc").CreateConnection();
        }

        /// <summary>
        /// Creates an Odbc DbConnection with a given connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DbConnection CreateOdbcConnection(string connectionString)
        {
            DbConnection connection = CreateOdbcConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
