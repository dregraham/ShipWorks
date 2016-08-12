using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcStoreSettingsTelemetryCollectorTest : IDisposable
    {
        private readonly AutoMock mock;
        readonly OdbcStoreEntity odbcStore;

        private const string UploadDriverName = "uploadDriver";
        private const string ImportDriverName = "importDriver";
        private readonly string mapRecordIdentifierSource = "externalColumnName";

        private string orderNumberExternalColumnName = "externalColumnName";
        private int maxEntryIndex = 0;

        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private Mock<IOdbcFieldMapFactory> fieldMapFactory;
        private Mock<IOdbcFieldMap> importFieldMapMock;

        public OdbcStoreSettingsTelemetryCollectorTest()
        {
            odbcStore = new OdbcStoreEntity()
            {
                ImportConnectionString = "not a null string",
                UploadConnectionString = "also not null"
            };
            
            mock = AutoMock.GetLoose();

            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();

            MockDataSourceService();
            MockFieldMap();
        }

        [Fact]
        public void CollectTelemetry_AddsErrorProperty_WhenStoreIsNotOdbcStore()
        {
            OdbcStoreSettingsTelemetryCollector testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(new EbayStoreEntity(), trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Error", "An attempt was made to collect ODBC telemetry on a non-ODBC store."));
        }

        [Fact]
        public void CollectTelemetry_AddsErrorProperty_WhenStoreIsNull()
        {
            OdbcStoreSettingsTelemetryCollector testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(null, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Error", "An attempt was made to collect ODBC telemetry on a non-ODBC store."));
        }

        [Fact]
        public void CollectTelemetry_AddsErrorProperty_WhenExceptionOccurs()
        {
            // Throw an exception when the import driver property is added
            trackedDurationEventMock.Setup(e => e.AddProperty("Import.Driver", It.IsAny<string>())).Throws(new Exception("the message"));

            OdbcStoreSettingsTelemetryCollector testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Error", "the message"));
        }

        [Fact]
        public void CollectTelemetry_ImportDriverSetFromDataSourceService()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.Driver", ImportDriverName));
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomQuery)]
        [InlineData(OdbcColumnSourceType.Table)]
        public void CollectTelemetry_ImportColumnSourceTypeSetFromOdbcStore(OdbcColumnSourceType sourceType)
        {
            odbcStore.ImportColumnSourceType = (int) sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.QueryType", EnumHelper.GetApiValue(sourceType)));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToUnknown_WhenNoOrderNumberEntry()
        {
            importFieldMapMock.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "OrderNumber"), true))
                .Returns(() => new IOdbcFieldMapEntry[0]);

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Unknown"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToYes_WhenOneItemPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Single line"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenTwoItemsPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            maxEntryIndex = 1;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Multi-line"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenOneItemPerOrder_AndRecordIdentifierSourceNotEqualToOrderNumberExternalFieldColumnName()
        {
            orderNumberExternalColumnName = "AnotherName";
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Multi-line"));
        }

        [Theory]
        [InlineData(OdbcShipmentUploadStrategy.DoNotUpload)]
        [InlineData(OdbcShipmentUploadStrategy.UseImportDataSource)]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource)]
        public void CollectTelemetry_UploadStrategyTypeSetFromOdbcStore(OdbcShipmentUploadStrategy sourceType)
        {
            odbcStore.UploadStrategy = (int)sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.Strategy", EnumHelper.GetApiValue(sourceType)));
        }

        [Theory]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource, UploadDriverName)]
        [InlineData(OdbcShipmentUploadStrategy.UseImportDataSource, UploadDriverName)]
        [InlineData(OdbcShipmentUploadStrategy.DoNotUpload, "None")]
        public void CollectTelemetry_UploadDriverNameIsSetFromDataSourceService(OdbcShipmentUploadStrategy strategy, string expectedResult)
        {
            odbcStore.UploadStrategy = (int) strategy;
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.Driver", expectedResult), Times.Once);
        }

        [Theory]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource, OdbcColumnSourceType.CustomQuery, "CustomQuery")]
        [InlineData(OdbcShipmentUploadStrategy.UseImportDataSource, OdbcColumnSourceType.Table, "Table")]
        [InlineData(OdbcShipmentUploadStrategy.DoNotUpload, OdbcColumnSourceType.Table, "None")]
        public void CollectTelemetry_UploadColumnSourceTypeSetFromOdbcStore(OdbcShipmentUploadStrategy strategy, OdbcColumnSourceType sourceType, string expectedResult)
        {
            odbcStore.UploadStrategy = (int) strategy;

            odbcStore.UploadColumnSourceType = (int)sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.QueryType", expectedResult));
        }

        private void MockDataSourceService()
        {
            var uploadDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            uploadDataSource.Setup(d => d.Driver).Returns(() => UploadDriverName);

            var importDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            importDataSource.Setup(d => d.Driver).Returns(() => ImportDriverName);

            var dataSourceServiceMock = mock.Mock<IOdbcDataSourceService>();
            dataSourceServiceMock.Setup(s => s.GetUploadDataSource(odbcStore)).Returns(uploadDataSource.Object);
            dataSourceServiceMock.Setup(s => s.GetImportDataSource(odbcStore)).Returns(importDataSource.Object);
        }

        private void MockFieldMap()
        {
            var orderNumberExternalFieldMock = mock.MockRepository.Create<IExternalOdbcMappableField>();
            orderNumberExternalFieldMock.Setup(e => e.Column).Returns(() => new OdbcColumn(orderNumberExternalColumnName));

            var orderNumberFieldMapEntryMock = mock.MockRepository.Create<IOdbcFieldMapEntry>();
            orderNumberFieldMapEntryMock.Setup(e => e.ExternalField).Returns(() => orderNumberExternalFieldMock.Object);
            orderNumberFieldMapEntryMock.Setup(e => e.Index).Returns(() => maxEntryIndex);

            importFieldMapMock = mock.MockRepository.Create<IOdbcFieldMap>();
            importFieldMapMock.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "OrderNumber"), true))
                .Returns(() => new[] { orderNumberFieldMapEntryMock.Object });
            importFieldMapMock.Setup(m => m.RecordIdentifierSource).Returns(() => mapRecordIdentifierSource);
            importFieldMapMock.Setup(m=>m.Entries).Returns(() => new[] { orderNumberFieldMapEntryMock.Object });


            fieldMapFactory = mock.MockRepository.Create<IOdbcFieldMapFactory>();
            fieldMapFactory.Setup(f => f.CreateEmptyFieldMap()).Returns(importFieldMapMock.Object);

            mock.Provide(fieldMapFactory.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
