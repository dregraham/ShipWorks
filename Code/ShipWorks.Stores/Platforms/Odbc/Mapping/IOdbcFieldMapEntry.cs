namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains a mapping between a ExternalOdbcMappableField and ShipWorksOdbcMappableField
    /// </summary>
    public interface IOdbcFieldMapEntry
    {
        /// <summary>
        /// Mapped External Field
        /// </summary>
        ExternalOdbcMappableField ExternalField { get; }

        /// <summary>
        /// Mapped Shipworks field
        /// </summary>
        ShipWorksOdbcMappableField ShipWorksField { get; }
    }
}