using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains Field Mapping information for ODBC.
    /// </summary>
	public class OdbcFieldMap : IOdbcFieldMap
    {
		private readonly IOdbcFieldMapIOFactory ioFactory;
        private readonly List<IOdbcFieldMapEntry> entries;

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
		    MethodConditions.EnsureArgumentIsNotNull(ioFactory);

		    this.ioFactory = ioFactory;
		    entries = new List<IOdbcFieldMapEntry>();
		}

        /// <summary>
        /// The ODBC Field Map Entries
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcFieldMapEntry> Entries => entries;

        /// <summary>
        /// The External Table Name
        /// </summary>
        public string ExternalTableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
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
        public void ResetValues()
        {
            entries.ForEach(e => e.ExternalField.ResetValue());
        }

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        public void CopyToEntity(IEntity2 entity)
        {
            IEnumerable<IOdbcFieldMapEntry> applicableEntries = entries
                .Where(e => e.ShipWorksField.Value != null && e.ShipWorksField.ContainingObjectName == entity.LLBLGenProEntityName);

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
            foreach (IOdbcFieldMapEntry entry in entries)
            {
                // Load data from OdbcRecord
                entry.LoadExternalField(record);

                // Copy the External fields to the ShipWorks fields
                entry.CopyValueToShipWorksField();
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

            ExternalTableName = reader.ReadExternalTableName();
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
        /// Finds the OdbcFieldMapEntry corresponding to the given field
        /// </summary>
        public IOdbcFieldMapEntry FindEntryBy(EntityField2 field)
        {
            return Entries.FirstOrDefault(entry =>
            entry.ShipWorksField.GetQualifiedName().Equals($"{field.ContainingObjectName}.{field.Name}",
            StringComparison.InvariantCulture));
        }
    }
}
