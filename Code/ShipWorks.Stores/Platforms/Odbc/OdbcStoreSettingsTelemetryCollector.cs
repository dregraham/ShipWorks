using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcStoreSettingsTelemetryCollector : IStoreSettingsTelemetryCollector
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly Func<IOdbcFieldMap> odbcFieldMapFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreSettingsTelemetryCollector"/> class.
        /// </summary>
        public OdbcStoreSettingsTelemetryCollector(IOdbcDataSourceService dataSourceService, Func<IOdbcFieldMap> odbcFieldMapFactory)
        {
            this.dataSourceService = dataSourceService;
            this.odbcFieldMapFactory = odbcFieldMapFactory;
        }

        /// <summary>
        /// Collects telemetry for a store
        /// </summary>
        public void CollectTelemetry(StoreEntity store, ITrackedDurationEvent trackedDurationEvent)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "store");

            OdbcStoreEntity odbcStore = store as OdbcStoreEntity;
            if (odbcStore == null)
            {
                throw new ArgumentException("Not an Odbc store.");
            }

            trackedDurationEvent.AddProperty("Import.Driver", GetImportDriverName(odbcStore));
            trackedDurationEvent.AddProperty("Import.ColumnSourceType", GetImportColumnSourceTypeName(odbcStore));
            trackedDurationEvent.AddProperty("Import.IsSingleLine", GetImportIsSingleLine(odbcStore));
            trackedDurationEvent.AddProperty("Upload.Driver", GetUploadDriverName(odbcStore));
            trackedDurationEvent.AddProperty("Upload.Strategy", GetUploadStrategyName(odbcStore));
            trackedDurationEvent.AddProperty("Upload.ColumnSourceType", GetUploadColumnSourceTypeName(odbcStore));
        }

        /// <summary>
        /// Gets the import driver string
        /// </summary>
        private string GetImportDriverName(OdbcStoreEntity odbcStore) => dataSourceService.GetImportDataSource(odbcStore).Driver;

        /// <summary>
        /// Gets the name of the import column source.
        /// </summary>
        private static string GetImportColumnSourceTypeName(OdbcStoreEntity odbcStore) => EnumHelper.GetDescription((OdbcColumnSourceType)odbcStore.ImportColumnSourceType);

        /// <summary>
        /// Determines if the import map is single line.
        /// </summary>
        /// <returns>Yes, No, or Unknown </returns>
        private string GetImportIsSingleLine(OdbcStoreEntity odbcStore)
        {
            IOdbcFieldMap importMap = odbcFieldMapFactory();
            importMap.Load(odbcStore.ImportMap);

            IOdbcFieldMapEntry orderNumberEntry = importMap.FindEntriesBy(OrderFields.OrderNumber, true).SingleOrDefault();
            int numberOfItemsPerOrder = importMap.Entries.Max(e => e.Index) + 1;

            if (orderNumberEntry == null)
            {
                return "Unknown";
            }

            if (numberOfItemsPerOrder == 1 && importMap.RecordIdentifierSource == orderNumberEntry.ExternalField.Column.Name)
            {
                return "Yes";
            }

            return "No";
        }

        /// <summary>
        /// Gets the name of the upload driver.
        /// </summary>
        private string GetUploadDriverName(OdbcStoreEntity odbcStore) => dataSourceService.GetUploadDataSource(odbcStore).Driver;

        /// <summary>
        /// Gets the name of the upload strategy.
        /// </summary>
        private string GetUploadStrategyName(OdbcStoreEntity odbcStore)
            => EnumHelper.GetDescription((OdbcShipmentUploadStrategy)odbcStore.UploadStrategy);

        /// <summary>
        /// Gets the name of the upload column source type.
        /// </summary>
        private string GetUploadColumnSourceTypeName(OdbcStoreEntity odbcStore)
            => EnumHelper.GetDescription((OdbcColumnSourceType) odbcStore.UploadColumnSourceType);
    }
}