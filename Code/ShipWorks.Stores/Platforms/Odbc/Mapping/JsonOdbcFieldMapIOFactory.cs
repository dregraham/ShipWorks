using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using System;
using System.IO;

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
            MethodConditions.EnsureArgumentIsNotNull(logFactory, nameof(logFactory));
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Creates a field map reader
        /// </summary>
        public IOdbcFieldMapReader CreateReader(string serializedMap) => 
            new JsonOdbcFieldMapReader(serializedMap, logFactory(typeof(JsonOdbcFieldMapReader)));

        /// <summary>
        /// Creates a field map writer.
        /// </summary>
        public IOdbcFieldMapSerializer CreateWriter(OdbcFieldMap map) => new JsonOdbcFieldMapSerializer(map);
    }
}
