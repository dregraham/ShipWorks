using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Writes field map entries
    /// </summary>
    public interface IOdbcFieldMapWriter
	{
        /// <summary>
        /// Write the field map
        /// </summary>
        void Write(OdbcFieldMap map, Stream stream);
	}
}
