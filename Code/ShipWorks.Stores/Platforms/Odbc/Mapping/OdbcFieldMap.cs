using System;
using Interapptive.Shared.Utility;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;

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
            foreach (IOdbcFieldMapEntry entry in entries.Where(e => e.ExternalField.Value != null))
            {
                // Copy the External fields to the ShipWorks fields
                entry.CopyValueToShipWorksField();

                string destinationName = entry.ShipWorksField.Name;
                Type destinationType = entity.Fields[entry.ShipWorksField.Name].DataType;
                object destinationValue = entry.ShipWorksField.Value;

                // Set the CurrentValue of the entity field who's name matches the entry field
                entity.Fields[destinationName].CurrentValue = ChangeType(destinationValue, destinationType);
            }
        }

        /// <summary>
        /// Convert the given object to the supplied type
        /// </summary>
        private static object ChangeType(object value, Type type)
        {
            try
            {
                return Convert.ChangeType(value, type);
            }
            catch (Exception ex)
            {
                throw new ShipWorksOdbcException($"Unable to convert {value} to {type}", ex);
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
    }
}
