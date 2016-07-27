using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains Field Mapping information for ODBC.
    /// </summary>
    /// <seealso cref="IOdbcFieldMap" />
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
                entry.ShipWorksField.ResetValue();
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
            Entries
                .Where(e => e.ShipWorksField.ContainingObjectName == entity.LLBLGenProEntityName)
                .Where(e => e.Index == index)
                .ToList()
                .ForEach(entry => entity.SetNewFieldValue(entry.ShipWorksField.Name, entry.ShipWorksField.Value));
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
        /// Apply the given entity values to the entries ShipWorks fields
        /// </summary>
        /// <param name="entities">Entities we want to pull data from.</param>
        /// <param name="fieldValueResolverFactory">
        /// Each entry specifies a resolution strategy which cooresponds to a key in this parameter. This strategy
        /// is then used to get the correct value from the shipworks entities.
        /// </param>
        public void ApplyValues(IEnumerable<IEntity2> entities, IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver> fieldValueResolverFactory)
        {
            // Reset all the values first
            ResetValues();

            foreach (IEntity2 entity in entities ?? Enumerable.Empty<IEntity2>())
            {
                foreach (IOdbcFieldMapEntry entry in Entries)
                {
                    // Load data from entity
                    entry.LoadShipWorksField(entity, fieldValueResolverFactory[entry.ShipWorksField.ResolutionStrategy]);
                }
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
    }
}
