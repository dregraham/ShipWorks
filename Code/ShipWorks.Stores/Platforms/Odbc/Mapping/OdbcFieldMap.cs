using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains Field Mapping information for ODBC.
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Mapping.IOdbcFieldMap" />
	public class OdbcFieldMap : IOdbcFieldMap
    {
		private readonly IOdbcFieldMapIOFactory ioFactory;
        private readonly List<IOdbcFieldMapEntry> entries;
        private readonly Dictionary<Type, object> typeDefaultValues; 

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
		    MethodConditions.EnsureArgumentIsNotNull(ioFactory);

		    this.ioFactory = ioFactory;
		    entries = new List<IOdbcFieldMapEntry>();

            typeDefaultValues = GetTypeDefaultValues();
		}

        /// <summary>
        /// The ODBC Field Map Entries
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcFieldMapEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string RecordIdentifierSource { get; set; }

        /// <summary>
        /// Add the given ODBC Field Map Entry to the ODBC Field Map
        /// </summary>
        public void AddEntry(IOdbcFieldMapEntry entry)
		{
			entries.Add(entry);
		}

        /// <summary>
        /// Reset all of the entries external fields
        /// </summary>
        private void ResetValues()
        {
            foreach (IOdbcFieldMapEntry entry in Entries)
            {
                entry.ExternalField.ResetValue();
            }
        }

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        public void CopyToEntity(IEntity2 entity) => CopyToEntity(entity, 0);

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        public void CopyToEntity(IEntity2 entity, int index)
        {
            IEnumerable<IOdbcFieldMapEntry> applicableEntries = Entries
                .Where(e => e.ShipWorksField.ContainingObjectName == entity.LLBLGenProEntityName && e.Index == index);

            foreach (IOdbcFieldMapEntry entry in applicableEntries)
            {
                string destinationName = entry.ShipWorksField.Name;
                object value = entry.ShipWorksField.Value;
                IEntityField2 destinationField = entity.Fields[destinationName];

                // Don't write to readonly or primary key fields. They shouldn't be mapped.
                if (destinationField.IsPrimaryKey || destinationField.IsReadOnly)
                {
                    throw new ShipWorksOdbcException(
                        $"Invalid Map. '{entry.ShipWorksField.ContainingObjectName}.{entry.ShipWorksField.Name}' should never be mapped.");
                }

                Debug.Assert(typeDefaultValues.ContainsKey(destinationField.DataType), $"No default value for {destinationField.Name} because the type is {destinationField.DataType}");
                if (value == null && !destinationField.IsNullable && typeDefaultValues.ContainsKey(destinationField.DataType))
                {
                    value = typeDefaultValues[destinationField.DataType];
                }

                // Set the CurrentValue of the entity field who's name matches the entry field
                entity.SetNewFieldValue(destinationName, value);
            }
        }

        private static Dictionary<Type, object> GetTypeDefaultValues()
        {
            return new Dictionary<Type, object>
            {
                {typeof(string), string.Empty},
                {typeof(double), 0D},
                {typeof(float), 0F},
                {typeof(decimal), 0M},
                {typeof(int), 0},
                {typeof(long), 0L},
                {typeof(short), 0},
                {typeof(bool), false},
                {typeof(DateTime), null} // We don't want to override dates with any default, so keep it null.
            };
        }

        /// <summary>
        /// Apply the given record values to the entries external fields
        /// </summary>
        public void ApplyValues(OdbcRecord record)
        {
            // Reset all the values first
            ResetValues();

            // If the record is null return after resetting values
            if (record == null)
            {
                return;
            }

            foreach (IOdbcFieldMapEntry entry in Entries)
            {
                // Load data from OdbcRecord
                entry.LoadExternalField(record);

                // Copy the External fields to the ShipWorks fields
                entry.CopyExternalValueToShipWorksField();
            }
        }

        /// <summary>
        /// Loads the ODBC Field Map from the given stream
        /// </summary>
		public void Load(Stream stream)
        {
            IOdbcFieldMapReader reader = ioFactory.CreateReader(stream);

            Load(reader);
        }

        /// <summary>
        /// Loads the ODBC Field Map from the given string
        /// </summary>
        public void Load(string serializedMap)
        {
            IOdbcFieldMapReader reader = ioFactory.CreateReader(serializedMap);

            Load(reader);
        }

        /// <summary>
        /// Loads the ODBC Field Map from the given IOdbcFieldMapReader
        /// </summary>
        private void Load(IOdbcFieldMapReader reader)
        {
            OdbcFieldMapEntry entry = reader.ReadEntry();
            while (entry != null)
            {
                AddEntry(entry);

                entry = reader.ReadEntry();
            }

            RecordIdentifierSource = reader.ReadRecordIdentifierSource();
        }

        /// <summary>
        /// Writes the ODBC Field Map to the given stream
        /// </summary>
		public void Save(Stream stream)
		{
		    IOdbcFieldMapWriter writer = ioFactory.CreateWriter(this);
            writer.Write(stream);
		}

        /// <summary>
        /// Finds the OdbcFieldMapEntries corresponding to the given field
        /// </summary>
        public IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field)
        {
            return Entries.Where(entry =>
                entry.ShipWorksField.Name == field.Name &&
                entry.ShipWorksField.ContainingObjectName == field.ContainingObjectName);
        }

        /// <summary>
        /// Finds the OdbcFieldMapEntries corresponding to the given field
        /// </summary>
        public IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field, bool includeWhenShipworksFieldIsNull)
        {
            return FindEntriesBy(field).Where(e => includeWhenShipworksFieldIsNull || e.ShipWorksField.Value != null);
        }

        /// <summary>
        /// Finds the entries by entity and index.
        /// </summary>
        public IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(IEnumerable<EntityType> entityTypes, int index, bool includeWhenShipworksFieldIsNull)
        {
            return Entries
                .Where(entry => entityTypes.Any(e => e.ToString() == entry.ShipWorksField.ContainingObjectName))
                .Where(e => includeWhenShipworksFieldIsNull || e.ShipWorksField.Value != null)
                .Where(e => e.Index == index);
        }

        /// <summary>
        /// Make a copy of the OdbcFieldMap
        /// </summary>
        public IOdbcFieldMap Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Save(stream);
                OdbcFieldMap clonedFieldMap = new OdbcFieldMap(ioFactory);
                clonedFieldMap.Load(stream);

                return clonedFieldMap;
            }
        }

        /// <summary>
        /// Gets the name of the external table.
        /// </summary>
        public string GetExternalTableName()
        {
            return Entries.FirstOrDefault()?.ExternalField.Table.Name ?? string.Empty;
        }
    }
}
