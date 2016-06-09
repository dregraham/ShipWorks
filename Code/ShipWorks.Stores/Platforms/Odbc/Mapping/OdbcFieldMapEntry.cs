using Interapptive.Shared.Utility;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Entry in the OdbcFieldMap.
    /// Maps a ShipWorks database column to an external Odbc column
    /// </summary>
    [Obfuscation(Exclude = true)]
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
        public IShipWorksOdbcMappableField ShipWorksField { get; }

        /// <summary>
        /// Gets the external field.
        /// </summary>
        public IExternalOdbcMappableField ExternalField { get; }

        /// <summary>
        /// Loads the given ODBC Record into the External Field
        /// </summary>
        [Obfuscation(Exclude = false)]
        public void LoadExternalField(OdbcRecord record)
        {
            MethodConditions.EnsureArgumentIsNotNull(record);
            ExternalField.LoadValue(record);
        }

        /// <summary>
        /// Copy the value from the External Field into the ShipWorks Field
        /// </summary>
        [Obfuscation(Exclude = false)]
        public void CopyExternalValueToShipWorksField()
        {
            ShipWorksField?.LoadValue(ExternalField?.Value);
        }
    }
}
