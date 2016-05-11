namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface for a mappable field, which will be part of a field map entry
    /// </summary>
    public interface IOdbcMappableField
    {
        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        string GetQualifiedName();

        /// <summary>
        /// The fields value
        /// </summary>
        string Value { get; }

        /// <summary>
        /// The fields display name
        /// </summary>
        string DisplayName { get; }
	}
}
