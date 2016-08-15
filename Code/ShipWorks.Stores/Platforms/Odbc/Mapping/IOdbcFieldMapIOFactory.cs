namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field map readers and writers
    /// </summary>
    public interface IOdbcFieldMapIOFactory
    {
        /// <summary>
        /// Creates a field map reader.
        /// </summary>
        IOdbcFieldMapReader CreateReader(string serializedMap);

        /// <summary>
        /// Creates a field map writer.
        /// </summary>
        IOdbcFieldMapSerializer CreateWriter(OdbcFieldMap map);
    }
}
