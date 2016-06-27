namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Factory for resolving all of the dependencies needed to create an ODBC Table
    /// </summary>
    public interface IOdbcColumnSourceFactory
    {
        /// <summary>
        /// Creates an ODBC table with the given schema and name
        /// </summary>
        IOdbcColumnSource CreateTable(string tableName);
    }
}