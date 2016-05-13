using System;
using System.IO;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field map readers and writers
    /// </summary>
    public class JsonOdbcFieldMapIOFactory : IOdbcFieldMapIOFactory
    {
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonOdbcFieldMapIOFactory(Func<Type, ILog> logFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(logFactory);
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Creates a field map reader.
        /// </summary>
        public IOdbcFieldMapReader CreateReader(Stream stream) => new JsonOdbcFieldMapReader(stream, logFactory(typeof(JsonOdbcFieldMapReader)));

        /// <summary>
        /// Creates a field map writer.
        /// </summary>
        public IOdbcFieldMapWriter CreateWriter() => new JsonOdbcFieldMapWriter();
    }
}
