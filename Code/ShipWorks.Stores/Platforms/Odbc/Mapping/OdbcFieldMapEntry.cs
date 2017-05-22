using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
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
            : this((IShipWorksOdbcMappableField) shipWorksField, externalField, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapEntry"/> class.
        /// </summary>
        public OdbcFieldMapEntry(IShipWorksOdbcMappableField shipWorksField, IExternalOdbcMappableField externalField, int index)
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
            MethodConditions.EnsureArgumentIsNotNull(record, nameof(record));
            ExternalField.LoadValue(record);
        }

        /// <summary>
        /// Loads the given ODBC Column into the External Field
        /// </summary>
        public void LoadExternalField(OdbcColumn column)
        {
            ExternalField.Column = column;
        }

        /// <summary>
        /// Loads the given entity into the ShipWorksField
        /// </summary>
        public void LoadShipWorksField(IEntity2 entity, IOdbcFieldValueResolver valueResolver)
        {
            MethodConditions.EnsureArgumentIsNotNull(entity, nameof(entity));
            MethodConditions.EnsureArgumentIsNotNull(valueResolver, nameof(valueResolver));

            ShipWorksField.LoadValue(entity, valueResolver);
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
