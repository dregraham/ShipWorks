using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Collections.Generic;
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

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
		    MethodConditions.EnsureArgumentIsNotNull(ioFactory);

		    this.ioFactory = ioFactory;
		    Entries = new List<IOdbcFieldMapEntry>();
		}

        /// <summary>
        /// The ODBC Field Map Entries
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<IOdbcFieldMapEntry> Entries { get; }

        /// <summary>
        /// Gets the maximum index.
        /// </summary>
        [JsonIgnore]
        public int MaxIndex
        {
            get { return Entries.Any() ? Entries.Max(e => e.Index) : 0; }
        }

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
        public string RecordIdentifierSource { get; set; }

        /// <summary>
        /// Add the given ODBC Field Map Entry to the ODBC Field Map
        /// </summary>
        public void AddEntry(IOdbcFieldMapEntry entry)
		{
			Entries.Add(entry);
		}

        /// <summary>
        /// Removes the entry
        /// </summary>
        public void RemoveEntry(IOdbcFieldMapEntry entry)
        {
            Entries.Remove(entry);
        }

        /// <summary>
        /// Reset all of the entries external fields
        /// </summary>
        private void ResetValues()
        {
            foreach (var entry in Entries)
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
                .Where(
                    e =>
                        e.ShipWorksField.Value != null &&
                        e.ShipWorksField.ContainingObjectName == entity.LLBLGenProEntityName && e.Index == index);

            foreach (IOdbcFieldMapEntry entry in applicableEntries)
            {
                string destinationName = entry.ShipWorksField.Name;

                // Set the CurrentValue of the entity field who's name matches the entry field
                entity.Fields[destinationName].CurrentValue = entry.ShipWorksField.Value;
            }
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
