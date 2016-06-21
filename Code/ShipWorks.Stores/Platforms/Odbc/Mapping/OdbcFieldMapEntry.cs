using Interapptive.Shared.Utility;
using Newtonsoft.Json;
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
        /// Constructor
        /// </summary>
        /// <param name="shipWorksField">Mapped Shipworks field.</param>
        /// <param name="externalField">Mapped External Field.</param>
        /// <param name="index">Index can be used to help identify this entry.</param>
        [JsonConstructor]
        public OdbcFieldMapEntry(ShipWorksOdbcMappableField shipWorksField, ExternalOdbcMappableField externalField, int index) 
        {
            ShipWorksField = shipWorksField;
            ExternalField = externalField;
            Index = index;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapEntry"/> class.
        /// </summary>
        public OdbcFieldMapEntry(ShipWorksOdbcMappableField shipWorksField, ExternalOdbcMappableField externalField)
            : this(shipWorksField, externalField, 0)
        {
        }

        /// <summary>
        /// Index to help identify the entry
        /// </summary>
        public int Index { get; }


        /// <summary>
        /// Mapped Shipworks field
        /// </summary>
        public IShipWorksOdbcMappableField ShipWorksField { get; }

        /// <summary>
        /// Mapped External Field
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
