using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcStoreSettingsTelemetryCollector : IStoreSettingsTelemetryCollector
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly IOdbcFieldMapFactory odbcFieldMapFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreSettingsTelemetryCollector"/> class.
        /// </summary>
        public OdbcStoreSettingsTelemetryCollector(IOdbcDataSourceService dataSourceService, IOdbcFieldMapFactory odbcFieldMapFactory)
        {
            this.dataSourceService = dataSourceService;
            this.odbcFieldMapFactory = odbcFieldMapFactory;
        }

        /// <summary>
        /// Collects telemetry for a store
        /// </summary>
        public void CollectTelemetry(StoreEntity store, ITrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                OdbcStoreEntity odbcStore = store as OdbcStoreEntity;
                if (odbcStore == null)
                {
                    // This is invalid, but we don't want to throw an exception here
                    // that could result in a crash.
                    trackedDurationEvent.AddProperty("Error", "An attempt was made to collect ODBC telemetry on a non-ODBC store.");
                }
                else
                {
                    trackedDurationEvent.AddProperty("Import.Driver", GetImportDriverName(odbcStore));
                    trackedDurationEvent.AddProperty("Import.QueryType", GetImportColumnSourceTypeName(odbcStore));
                    trackedDurationEvent.AddProperty("Import.OrderItemDataStructure", OrderItemDataStructure(odbcStore));
                    trackedDurationEvent.AddProperty("Upload.Strategy", GetUploadStrategyName(odbcStore));
                    trackedDurationEvent.AddProperty("Upload.Driver", GetUploadDriverName(odbcStore));
                    trackedDurationEvent.AddProperty("Upload.QueryType", GetUploadColumnSourceTypeName(odbcStore));
                }
            }
            catch (Exception exception)
            {
                // Just denote this as an invalid operation. We never want telemetry
                // to result in a crash.
                trackedDurationEvent.AddProperty("Error", exception.Message);
            }
        }

        /// <summary>
        /// Gets the import driver string
        /// </summary>
        private string GetImportDriverName(OdbcStoreEntity odbcStore)
        {
            return string.IsNullOrWhiteSpace(odbcStore.ImportConnectionString) ?
                "None" :
                dataSourceService.GetImportDataSource(odbcStore).Driver;
        }

        /// <summary>
        /// Gets the name of the import column source.
        /// </summary>
        private string GetImportColumnSourceTypeName(OdbcStoreEntity odbcStore)
        {
            return GetImportDriverName(odbcStore).Equals("None", StringComparison.InvariantCultureIgnoreCase) ?
                "None" :
                EnumHelper.GetApiValue((OdbcColumnSourceType) odbcStore.ImportColumnSourceType);
        }

        /// <summary>
        /// Determines if the import map is single line.
        /// </summary>
        /// <returns>Single line, Multi-line, or Unknown</returns>
        private string OrderItemDataStructure(OdbcStoreEntity odbcStore)
        {
            if (GetImportDriverName(odbcStore).Equals("None", StringComparison.InvariantCultureIgnoreCase))
            {
                return "None";
            }

            IOdbcFieldMap importMap = odbcFieldMapFactory.CreateEmptyFieldMap();
            importMap.Load(odbcStore.ImportMap);

            IOdbcFieldMapEntry orderNumberEntry = importMap.FindEntriesBy(OrderFields.OrderNumber, true).SingleOrDefault();

            IEnumerable<IOdbcFieldMapEntry> odbcFieldMapEntries = importMap.Entries as IOdbcFieldMapEntry[] ?? importMap.Entries.ToArray();

            if (!odbcFieldMapEntries.Any())
            {
                return "None";
            }

            if (orderNumberEntry == null)
            {
                return "Unknown";
            }

            int numberOfItemsPerOrder = odbcFieldMapEntries.Max(e => e.Index) + 1;

            if (numberOfItemsPerOrder == 1 && importMap.RecordIdentifierSource == orderNumberEntry.ExternalField.Column.Name)
            {
                return "Single line";
            }

            return "Multi-line";
        }

        /// <summary>
        /// Gets the name of the upload strategy.
        /// </summary>
        private string GetUploadStrategyName(OdbcStoreEntity odbcStore)
        {
            string orderItemDataStructure = OrderItemDataStructure(odbcStore);

            if (orderItemDataStructure.Equals("None", StringComparison.InvariantCultureIgnoreCase) ||
                orderItemDataStructure.Equals("Unknown", StringComparison.InvariantCultureIgnoreCase))
            {
                return "None";
            }

            return EnumHelper.GetApiValue((OdbcShipmentUploadStrategy) odbcStore.UploadStrategy);
        }

        /// <summary>
        /// Gets the name of the upload driver.
        /// </summary>
        private string GetUploadDriverName(OdbcStoreEntity odbcStore)
        {
            if (odbcStore.UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload ||
                string.IsNullOrWhiteSpace(odbcStore.UploadConnectionString))
            {
                return "None";
            }

            return dataSourceService.GetUploadDataSource(odbcStore)?.Driver ?? "Unknown";
        }


        /// <summary>
        /// Gets the name of the upload column source type.
        /// </summary>
        private string GetUploadColumnSourceTypeName(OdbcStoreEntity odbcStore)
        {
            return GetUploadDriverName(odbcStore).Equals("None", StringComparison.InvariantCultureIgnoreCase) ?
                "None" :
                EnumHelper.GetApiValue((OdbcColumnSourceType) odbcStore.UploadColumnSourceType);
        }
    }
}