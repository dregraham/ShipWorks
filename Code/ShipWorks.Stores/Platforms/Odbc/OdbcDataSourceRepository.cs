﻿using Interapptive.Shared.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Used to retrieve OdbcDataSources from system DSNs.
    /// </summary>
    public class OdbcDataSourceRepository : IOdbcDataSourceRepository
    {
        private readonly IDnsProvider dnsProvider;
        private readonly IShipWorksOdbcProvider odbcProvider;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceRepository"/> class.
        /// </summary>
        public OdbcDataSourceRepository(
            IDnsProvider dnsProvider,
            IShipWorksOdbcProvider odbcProvider,
            IEncryptionProvider encryptionProvider,
            Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(OdbcDataSourceRepository));

            this.dnsProvider = dnsProvider;
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
                List<OdbcDataSource> odbcDataSources = new List<OdbcDataSource>();

                string nextOdbcDataSource = dnsProvider.GetNextDsnName();

                while (nextOdbcDataSource != null)
                {
                    OdbcDataSource odbcDataSource = new OdbcDataSource(odbcProvider, encryptionProvider)
                    {
                        Name = nextOdbcDataSource
                    };

                    odbcDataSources.Add(odbcDataSource);

                    nextOdbcDataSource = dnsProvider.GetNextDsnName();
                }

                return odbcDataSources;
            }
            catch (DataException ex)
            {
                log.Error("Error in GetNextDsnName", ex);
                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            dnsProvider.Dispose();
        }
    }
}