using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface to contain Odbc Field Mapping information.
    /// </summary>
    public interface IOdbcFieldMap
    {
        /// <summary>
        /// The ODBC Field Map Entries.
        /// </summary>
        ObservableCollection<IOdbcFieldMapEntry> Entries { get; }

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
        string RecordIdentifierSource { get; set; }

        /// <summary>
        /// Add the given ODBC Field Map Entry to the ODBC Field Map.
        /// </summary>
        void AddEntry(IOdbcFieldMapEntry entry);

        /// <summary>
        /// Removes the entry
        /// </summary>
        void RemoveEntry(IOdbcFieldMapEntry entry);

        /// <summary>
        /// Loads the ODBC Field Map from the given string.
        /// </summary>
        void Load(string serializedMap);

        /// <summary>
        /// Loads the ODBC Field Map from the given stream.
        /// </summary>
        void Load(Stream stream);

        /// <summary>
        /// Writes the ODBC Field Map to the given stream
        /// </summary>
        void Save(Stream stream);

        /// <summary>
        /// Apply the given record values to the entries external fields
        /// </summary>
        void ApplyValues(OdbcRecord record);

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        void CopyToEntity(IEntity2 entity);

        /// <summary>
        /// Finds the OdbcFieldMapEntries corresponding to the given field
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field);

        /// <summary>
        /// Finds the entries by.
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field, bool includeEntriesWhereShipworksFieldIsNull);

        /// <summary>
        /// Make a copy of the IOdbcFieldMap
        /// </summary>
        IOdbcFieldMap Clone();

        /// <summary>
        /// Gets the name of the external table.
        /// </summary>
        string GetExternalTableName();
    }
}