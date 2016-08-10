namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Writes field map entries
    /// </summary>
    public interface IOdbcFieldMapSerializer
    {
        /// <summary>
        /// Serializes the field map
        /// </summary>
        string Serialize();
    }
}
