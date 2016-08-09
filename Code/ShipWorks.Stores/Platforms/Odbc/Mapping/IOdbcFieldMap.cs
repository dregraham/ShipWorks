using Autofac.Features.Indexed;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using System.Collections.Generic;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface to contain Odbc Field Mapping information.
    /// </summary>
    public interface IOdbcFieldMap
    {
        /// <summary>
        /// The name of the map
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The ODBC Field Map Entries.
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> Entries { get; }

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
        string RecordIdentifierSource { get; set; }

        /// <summary>
        /// Add the given ODBC Field Map Entry to the ODBC Field Map.
        /// </summary>
        void AddEntry(IOdbcFieldMapEntry entry);

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
        /// Applies the given entity values to the external fields
        /// </summary>
        /// <param name="entities">Entities we want to pull data from.</param>
        /// <param name="fieldValueResolverFactory">
        /// Each entry specifies a resolution strategy which cooresponds to a key in this parameter. This strategy
        /// is then used to get the correct value from the shipworks entities.
        /// </param>
        void ApplyValues(IEnumerable<IEntity2> entities, IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver> fieldValueResolverFactory);

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        void CopyToEntity(IEntity2 entity);

        /// <summary>
        /// Copies the values from the entries to corresponding fields on the entity
        /// </summary>
        void CopyToEntity(IEntity2 entity, int index);

        /// <summary>
        /// Finds the OdbcFieldMapEntries corresponding to the given field
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field);

        /// <summary>
        /// Finds the entries by.
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(EntityField2 field, bool includeWhenShipworksFieldIsNull);

        /// <summary>
        /// Make a copy of the IOdbcFieldMap
        /// </summary>
        IOdbcFieldMap Clone();

        /// <summary>
        /// Serializes this instance to JSON
        /// </summary>
        string Serialize();
    }
}