namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Reader for field map entries
    /// </summary>
    public interface IOdbcFieldMapReader
	{
        /// <summary>
        /// Reads the field map entry.
        /// </summary>
        OdbcFieldMapEntry ReadEntry();
	}
}
