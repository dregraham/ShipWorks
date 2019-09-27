using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Data.Common;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcDownloadCommand for a store
    /// </summary>
    public class OdbcDownloadCommandFactory : IOdbcDownloadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcStoreRepository odbcStoreRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcDownloadCommandFactory(IOdbcDataSource dataSource, 
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcStoreRepository odbcStoreRepository)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.odbcStoreRepository = odbcStoreRepository;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity storeEntity, IOdbcFieldMap odbcFieldMap)
        {
            OdbcStore store = odbcStoreRepository.GetStore(storeEntity);

            IOdbcQuery downloadQuery = GetDownloadQuery(storeEntity, store, odbcFieldMap, dataSource, dbProviderFactory);
 
            return new OdbcDownloadCommand(odbcFieldMap, dataSource, dbProviderFactory, downloadQuery);
        }

        /// <summary>
        /// Creates the download command using OnlineLastModified.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity storeEntity, DateTime onlineLastModified, IOdbcFieldMap odbcFieldMap)
        {
            OdbcStore store = odbcStoreRepository.GetStore(storeEntity);
            IOdbcQuery downloadQuery = GetDownloadQuery(storeEntity, store, odbcFieldMap, dataSource, dbProviderFactory);

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
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity storeEntity, string orderNumber, IOdbcFieldMap odbcFieldMap)
        {
            OdbcStore store = odbcStoreRepository.GetStore(storeEntity);
            IOdbcQuery downloadQuery = GetDownloadQuery(storeEntity, store, odbcFieldMap, dataSource, dbProviderFactory);
            
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
        private static IOdbcQuery GetDownloadQuery(OdbcStoreEntity storeEntity, OdbcStore store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            dataSource.Restore(storeEntity.ImportConnectionString);

            return store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table
                ? (IOdbcQuery) new OdbcTableDownloadQuery(storeEntity, store, dbProviderFactory, odbcFieldMap, dataSource)
                : new OdbcCustomDownloadQuery(storeEntity);
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
