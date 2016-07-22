using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcUploadCommand for a store
    /// </summary>
    public class OdbcUploadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcFieldMap fieldMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcUploadCommandFactory(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory, IOdbcFieldMap fieldMap)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.fieldMap = fieldMap;
        }

        /// <summary>
        /// Creates the upload command for the given store and map
        /// </summary>
        public IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            fieldMap.ApplyValues(new IEntity2[] { shipment, shipment.Order });

            IOdbcQuery uploadQuery = GetUploadQuery(store, fieldMap, dataSource, dbProviderFactory);

            if (uploadQuery == null)
            {
                string uploadStrategy = EnumHelper.GetDescription((OdbcShipmentUploadStrategy)store.UploadStrategy);

                throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload strategy is '{uploadStrategy}'.");
            }

            return new OdbcUploadCommand(dataSource, dbProviderFactory, uploadQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private static IOdbcQuery GetUploadQuery(OdbcStoreEntity store, IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory)
        {
            switch (store.UploadStrategy)
            {
                case (int)OdbcShipmentUploadStrategy.UseImportDataSource:
                    dataSource.Restore(store.ImportConnectionString);
                    break;
                case (int)OdbcShipmentUploadStrategy.UseShipmentDataSource:
                    dataSource.Restore(store.UploadConnectionString);
                    break;
                default:
                    return null;
            }

            return new OdbcTableUploadQuery(odbcFieldMap, store, dbProviderFactory, dataSource);
        }
    }
}