using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field map readers and writers
    /// </summary>
    public class JsonOdbcFieldMapIOFactory : IOdbcFieldMapIOFactory
	{
        /// <summary>
        /// Creates a field map reader.
        /// </summary>
        public IOdbcFieldMapReader CreateReader(OdbcFieldMap map)
		{
            return new JsonOdbcFieldMapReader(map);
        }

        /// <summary>
        /// Creates a field map writer.
        /// </summary>
        public IOdbcFieldMapWriter CreateWriter(OdbcFieldMap map)
		{
            return new JsonOdbcFieldMapWriter(map);
        }
    }
}
