using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcUploadCommand for a store
    /// </summary>
    public class OdbcUploadCommandFactory : IOdbcUploadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly ITemplateTokenProcessor templateTokenProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcUploadCommandFactory(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory, IOdbcFieldMap fieldMap, ITemplateTokenProcessor templateTokenProcessor)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.fieldMap = fieldMap;
            this.templateTokenProcessor = templateTokenProcessor;
        }

        /// <summary>
        /// Creates the upload command for the given store and map
        /// </summary>
        public IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "store");
            MethodConditions.EnsureArgumentIsNotNull(shipment, "shipment");

            IOdbcQuery uploadQuery = CreateUploadQuery(store, shipment);

            return new OdbcUploadCommand(dataSource, dbProviderFactory, uploadQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private IOdbcQuery CreateUploadQuery(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            switch (store.UploadColumnSourceType)
            {
                case (int)OdbcColumnSourceType.Table:
                    return CreateTableUploadQuery(store, shipment);
                case (int)OdbcColumnSourceType.CustomQuery:
                    return CreateCustomUploadQuery(store, shipment);
                default:
                    string columnSource = EnumHelper.GetDescription((OdbcColumnSourceType)store.UploadColumnSourceType);
                    throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload source is '{columnSource}'.");
            }
        }

        /// <summary>
        /// Creates the custom upload query for the given store and shipment
        /// </summary>
        private IOdbcQuery CreateCustomUploadQuery(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            return new OdbcCustomUploadQuery(store, shipment, templateTokenProcessor);
        }

        /// <summary>
        /// Creates the table upload query for the given store and shipment
        /// </summary>
        private IOdbcQuery CreateTableUploadQuery(OdbcStoreEntity store, ShipmentEntity shipment)
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
                    string uploadStrategy = EnumHelper.GetDescription((OdbcShipmentUploadStrategy)store.UploadStrategy);
                    throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload strategy is '{uploadStrategy}'.");
            }

            fieldMap.ApplyValues(new IEntity2[] { shipment, shipment.Order });
            return new OdbcTableUploadQuery(fieldMap, store, dbProviderFactory, dataSource);
        }
    }
}