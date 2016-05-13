using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field map readers and writers
    /// </summary>
    public class JsonOdbcFieldMapIOFactory : IOdbcFieldMapIOFactory
	{
        private readonly Func<OdbcFieldMap, IOdbcFieldMapReader> readerFactory;
        private readonly Func<OdbcFieldMap, IOdbcFieldMapWriter> writerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonOdbcFieldMapIOFactory(Func<OdbcFieldMap, IOdbcFieldMapReader> readerFactory, Func<OdbcFieldMap, IOdbcFieldMapWriter> writerFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(readerFactory);
            MethodConditions.EnsureArgumentIsNotNull(writerFactory);

            this.readerFactory = readerFactory;
            this.writerFactory = writerFactory;
        }

        /// <summary>
        /// Creates a field map reader.
        /// </summary>
        public IOdbcFieldMapReader CreateReader(OdbcFieldMap map) => readerFactory(map);


        /// <summary>
        /// Creates a field map writer.
        /// </summary>
        public IOdbcFieldMapWriter CreateWriter(OdbcFieldMap map) => writerFactory(map);
	}
}
