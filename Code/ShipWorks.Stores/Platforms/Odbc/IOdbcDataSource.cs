using System.Data.Common;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an interface for interacting with an OdbcDataSource
    /// </summary>
    public interface IOdbcDataSource
    {
        /// <summary>
        /// Tests the Data Source's connection
        /// </summary>
        GenericResult<OdbcDataSource> TestConnection();

        /// <summary>
        /// Creates an ODBC connection to the Data Source
        /// </summary>
        DbConnection CreateConnection();

        /// <summary>
        /// Name of the data source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Set connection string from serialized json.
        /// </summary>
        /// <param name="json">The json.</param>
        void Restore(string json);
    }
}