using System;
using Interapptive.Shared.Utility;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains Field Mapping information for ODBC
    /// </summary>
	public class OdbcFieldMap
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
            foreach (IOdbcFieldMapEntry entry in entries)
            {
                // Copy the External fields to the ShipWorks fields
                entry.CopyValueToShipWorksField();

                string destinationName = entry.ShipWorksField.Name;
                Type destinationType = entity.Fields[entry.ShipWorksField.Name].DataType;
                object destinationValue = entry.ShipWorksField.Value;

                // Set the CurrentValue of the entity field who's name matches the entry field
                entity.Fields[destinationName].CurrentValue = Convert.ChangeType(destinationValue, destinationType);
            }
        }

        /// <summary>
        /// Apply the given record values to the entries external fields
        /// </summary>
        /// <param name="record"></param>
        public void ApplyValues(OdbcRecord record)
        {
            entries.ForEach(e => e.LoadExternalField(record));
        }

        /// <summary>
        /// Loads the ODBC Field Map from the given stream
        /// </summary>
        /// <param name="stream"></param>
		public void Load(Stream stream)
		{
		    IOdbcFieldMapReader reader = ioFactory.CreateReader(stream);

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
        /// <param name="stream"></param>
		public void Save(Stream stream)
		{
		    IOdbcFieldMapWriter writer = ioFactory.CreateWriter(this);
            writer.Write(stream);
		}
	}
}
