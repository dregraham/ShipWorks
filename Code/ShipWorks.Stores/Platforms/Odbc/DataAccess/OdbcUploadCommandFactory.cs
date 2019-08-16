﻿using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Creates an OdbcUploadCommand for a store
    /// </summary>
    [Component]
    public class OdbcUploadCommandFactory : IOdbcUploadCommandFactory
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly ITemplateTokenProcessor templateTokenProcessor;
        private readonly IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver> odbcFieldValueResolvers;
        private readonly IOdbcStoreRepository odbcStoreRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommandFactory"/> class.
        /// </summary>
        public OdbcUploadCommandFactory(
            IOdbcDataSource dataSource,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcFieldMap fieldMap,
            ITemplateTokenProcessor templateTokenProcessor,
            IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver> odbcFieldValueResolvers,
            IOdbcStoreRepository odbcStoreRepository)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.fieldMap = fieldMap;
            this.templateTokenProcessor = templateTokenProcessor;
            this.odbcFieldValueResolvers = odbcFieldValueResolvers;
            this.odbcStoreRepository = odbcStoreRepository;
        }

        /// <summary>
        /// Creates the upload command for the given store and map
        /// </summary>
        public IOdbcUploadCommand CreateUploadCommand(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            OdbcShipmentUploadStrategy uploadStrategy = (OdbcShipmentUploadStrategy) odbcStoreRepository.GetStore(store).UploadStrategy;
            switch (uploadStrategy)
            {
                case OdbcShipmentUploadStrategy.UseImportDataSource:
                    dataSource.Restore(store.ImportConnectionString);
                    break;
                case OdbcShipmentUploadStrategy.UseShipmentDataSource:
                    dataSource.Restore(store.UploadConnectionString);
                    break;
                default:
                    string uploadStrategyName = EnumHelper.GetDescription(uploadStrategy);
                    throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload strategy is '{uploadStrategyName}'.");
            }

            IOdbcQuery uploadQuery = CreateUploadQuery(store, shipment);

            return new OdbcUploadCommand(dataSource, dbProviderFactory, uploadQuery);
        }

        /// <summary>
        /// Creates the download query used to retrieve orders.
        /// </summary>
        private IOdbcQuery CreateUploadQuery(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            OdbcColumnSourceType uploadColumnSourceType = 
                (OdbcColumnSourceType) odbcStoreRepository.GetStore(store).UploadColumnSourceType;

            switch (uploadColumnSourceType)
            {
                case OdbcColumnSourceType.Table:
                    return CreateTableUploadQuery(store, shipment);
                case OdbcColumnSourceType.CustomQuery:
                case OdbcColumnSourceType.CustomParameterizedQuery:
                    return CreateCustomUploadQuery(store, shipment);
                default:
                    string columnSourceType = EnumHelper.GetDescription(uploadColumnSourceType);
                    throw new ShipWorksOdbcException($"Unable to create upload command for store when the store upload source is '{columnSourceType}'.");
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
            fieldMap.Load(odbcStoreRepository.GetStore(store).UploadMap);
            fieldMap.ApplyValues(new IEntity2[] { shipment, shipment.Order }, odbcFieldValueResolvers);

            return new OdbcTableUploadQuery(fieldMap, store, dbProviderFactory, dataSource);
        }
    }
}