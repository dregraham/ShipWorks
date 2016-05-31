namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Entry in the OdbcFieldMap.
    /// Maps a ShipWorks database column to an external Odbc column
    /// </summary>
    public class OdbcFieldMapEntry : IOdbcFieldMapEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapEntry"/> class.
        /// </summary>
        /// <param name="shipWorksField">The ship works field.</param>
        /// <param name="externalField">The external field.</param>
        public OdbcFieldMapEntry(ShipWorksOdbcMappableField shipWorksField, ExternalOdbcMappableField externalField)
		{
		    ShipWorksField = shipWorksField;
		    ExternalField = externalField;
		}

        /// <summary>
        /// Gets the ShipWorks field.
        /// </summary>
        public ShipWorksOdbcMappableField ShipWorksField { get; }

        /// <summary>
        /// Gets the external field.
        /// </summary>
        public ExternalOdbcMappableField ExternalField { get; }
	}
}
