using Interapptive.Shared.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Used to retrieve OdbcDataSources from system DSNs.
    /// </summary>
    public class OdbcDataSourceRepository : IOdbcDataSourceRepository
    {
        private readonly IDsnProvider dsnProvider;
        private readonly IShipWorksDbProviderFactory odbcProvider;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceRepository"/> class.
        /// </summary>
        public OdbcDataSourceRepository(
            IDsnProvider dsnProvider,
            IShipWorksDbProviderFactory odbcProvider,
            IEncryptionProvider encryptionProvider,
            Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(OdbcDataSourceRepository));

            this.dsnProvider = dsnProvider;
            this.odbcProvider = odbcProvider;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Gets the available data sources.
        /// </summary>
        /// <exception cref="System.Data.DataException">
        /// Thrown when there is an issue retrieving information from the Datasources.
        /// </exception>
        public IEnumerable<OdbcDataSource> GetDataSources()
        {
            try
            {
                return dsnProvider.GetDataSourceNames()
                    .Select(name => new OdbcDataSource(odbcProvider, encryptionProvider)
                {
                    Name = name
                });
            }
            catch (DataException ex)
            {
                log.Error("Error in GetNextDsnName", ex);
                throw;
            }
        }
    }
}