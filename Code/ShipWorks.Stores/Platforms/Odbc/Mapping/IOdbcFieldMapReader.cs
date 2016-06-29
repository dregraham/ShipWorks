namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Reader for field map entries
    /// </summary>
    public interface IOdbcFieldMapReader
    {
        /// <summary>
        /// Reads the ODBC Field Map entry from the stream
        /// </summary>
        OdbcFieldMapEntry ReadEntry();

        /// <summary>
        /// Reads the record identifier source from the stream
        /// </summary>
        string ReadRecordIdentifierSource();

        /// <summary>
        /// Reads the custom query from the stream
        /// </summary>
        string ReadCustomQuery();
    }
}
