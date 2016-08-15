using ShipWorks.Stores.Platforms.Odbc.DataAccess;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Generates a context specific Odbc query
    /// </summary>
    public interface IOdbcQuery
    {
        /// <summary>
        /// Generates the Sql for the query
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        string GenerateSql();

        /// <summary>
        /// Adds Command Text to the given sql command
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        void ConfigureCommand(IShipWorksOdbcCommand command);
    }
}
