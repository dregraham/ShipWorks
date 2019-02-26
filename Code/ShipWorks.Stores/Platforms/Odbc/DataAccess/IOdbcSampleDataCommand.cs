using System.Data;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Interface to get a sample of the results of this query.
    /// </summary>
    public interface IOdbcSampleDataCommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        DataTable Execute(IOdbcDataSource dataSource, string query, int numberOfResults, OdbcImportStrategy importStrategy);
    }
}