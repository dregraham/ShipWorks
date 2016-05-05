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
        private readonly IDsnProvider dsnProvider;
        private readonly Func<OdbcDataSource> odbcDataSourceFactory;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceRepository"/> class.
        /// </summary>
        public OdbcDataSourceRepository(
            IDsnProvider dsnProvider,
            Func<OdbcDataSource> odbcDataSourceFactory,
            Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(OdbcDataSourceRepository));

            this.dsnProvider = dsnProvider;
            this.odbcDataSourceFactory = odbcDataSourceFactory;
        }

        /// <summary>
        /// Gets the available data sources.
        /// </summary>
        /// <exception cref="System.Data.DataException">
        /// Thrown when there is an issue retrieving information from the data sources.
        /// </exception>
        public IEnumerable<OdbcDataSource> GetDataSources()
        {
            try
            {
                List<OdbcDataSource> dataSources = new List<OdbcDataSource>();

                foreach (string dataSourceName in dsnProvider.GetDataSourceNames())
                {
                    OdbcDataSource dataSource = odbcDataSourceFactory();
                    dataSource.ChangeConnection(dataSourceName, string.Empty, string.Empty);
                    dataSources.Add(dataSource);
                }

                dataSources.Add(EmptyCustomDataSource());

                return dataSources;
            }
            catch (DataException ex)
            {
                log.Error("Error getting data source names.", ex);
                throw;
            }
        }

        /// <summary>
        /// Returns an empty custom data source
        /// </summary>
        private OdbcDataSource EmptyCustomDataSource()
        {
            OdbcDataSource dataSource = odbcDataSourceFactory();
            dataSource.ChangeConnection(string.Empty);
            return dataSource;
        }
    }
}