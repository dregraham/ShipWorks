using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Data.Common;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcDownloadCommand for a store
    /// </summary>
    public class OdbcDownloadCommandFactory : IOdbcDownloadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcDownloadCommandFactory(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap)
        {
            IOdbcQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, downloadQuery);
        }

        /// <summary>
        /// Creates the download command using OnlineLastModified.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, DateTime onlineLastModified, IOdbcFieldMap odbcFieldMap)
        {
            IOdbcQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);

            string lastModifiedColumnName = odbcFieldMap.FindEntriesBy(OrderFields.OnlineLastModified).FirstOrDefault()?.ExternalField.Column.Name;
            string quotedLastModifiedColumnName = WrapColumnInQuoteIdentifier(lastModifiedColumnName);
            
            IOdbcQuery lastModifiedQuery = new OdbcLastModifiedDownloadQuery(
                (OdbcColumnSourceType) store.ImportColumnSourceType,
                downloadQuery, 
                onlineLastModified, 
                lastModifiedColumnName,
                quotedLastModifiedColumnName);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, lastModifiedQuery);
        }

        /// <summary>
        /// Create a download command using orderNumber
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store, string orderNumber, IOdbcFieldMap odbcFieldMap)
        {
            IOdbcQuery downloadQuery = GetDownloadQuery(store, odbcFieldMap, dataSource, dbProviderFactory);
            
            string orderNumberColumnName = odbcFieldMap.FindEntriesBy(OrderFields.OrderNumberComplete).FirstOrDefault()?.ExternalField.Column.Name;
            string quotedOrderNumberColumnName = WrapColumnInQuoteIdentifier(orderNumberColumnName);

            IOdbcQuery orderNumberQuery = new OdbcOrderNumberDownloadQuery(
                (OdbcColumnSourceType) store.ImportColumnSourceType,
                downloadQuery, 
                orderNumber, 
                orderNumberColumnName,
                quotedOrderNumberColumnName);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, orderNumberQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcQuery GetDownloadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            dataSource.Restore(store.ImportConnectionString);

            return store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table
                ? (IOdbcQuery)new OdbcTableDownloadQuery(store, dbProviderFactory, odbcFieldMap, dataSource)
                : new OdbcCustomDownloadQuery(store);
        }

        /// <summary>
        /// Wraps the given column string in the data sources quoted identifier
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        private string WrapColumnInQuoteIdentifier(string column)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();
                return cmdBuilder.QuoteIdentifier(column);
            }
        }
    }
}
