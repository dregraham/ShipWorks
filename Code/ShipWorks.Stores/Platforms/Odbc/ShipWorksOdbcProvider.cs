using System.Data.Common;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Class for obtaining odbc resources with interface that can be mocked up for testing purposes
    /// </summary>
    public class ShipWorksOdbcProvider : IShipWorksOdbcProvider
    {
        /// <summary>
        /// Returns an OdbcConnection
        /// </summary>
        public DbConnection CreateOdbcConnection()
        {
            return DbProviderFactories.GetFactory("System.Data.Odbc").CreateConnection();
        }
    }
}
