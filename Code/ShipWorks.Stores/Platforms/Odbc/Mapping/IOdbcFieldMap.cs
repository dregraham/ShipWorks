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
        /// The ODBC Field Map Entries.
        /// </summary>
        IEnumerable<IOdbcFieldMapEntry> Entries { get; }

        /// <summary>
        /// The External Table Name.
        /// </summary>
        string ExternalTableName { get; set; }

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
    }
}