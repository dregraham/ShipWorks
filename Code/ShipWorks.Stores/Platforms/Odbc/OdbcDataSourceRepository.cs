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
        private readonly Func<IOdbcDataSource> odbcDataSourceFactory;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceRepository"/> class.
        /// </summary>
        public OdbcDataSourceRepository(
            IDsnProvider dsnProvider,
            Func<IOdbcDataSource> odbcDataSourceFactory,
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
        public IEnumerable<IOdbcDataSource> GetDataSources()
        {
            try
            {
                List<IOdbcDataSource> dataSources = new List<IOdbcDataSource>();

                // Build a collection of OdbcDataSources available on the machine
                foreach (string dataSourceName in dsnProvider.GetDataSourceNames())
                {
                    // Create a new data source and call change connection to
                    // initialize the data source to the data source name
                    IOdbcDataSource dataSource = odbcDataSourceFactory();
                    dataSource.ChangeConnection(dataSourceName, string.Empty, string.Empty);

                    // Add the data source to the collection
                    dataSources.Add(dataSource);
                }

                // Add an custom data source to the collection
                dataSources.Add(CreateEmptyCustomDataSource());

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
        private IOdbcDataSource CreateEmptyCustomDataSource()
        {
            IOdbcDataSource dataSource = odbcDataSourceFactory();
            dataSource.ChangeConnection(string.Empty);
            return dataSource;
        }
    }
}