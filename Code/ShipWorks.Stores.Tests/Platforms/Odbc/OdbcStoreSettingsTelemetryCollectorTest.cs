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

        private readonly string uploadDriver = "uploadDriver";
        private readonly string importDriver = "importDriver";
        private readonly string mapRecordIdentifierSource = "externalColumnName";

        private string orderNumberExternalColumnName = "externalColumnName";
        private int maxEntryIndex = 0;

        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private Mock<IOdbcFieldMap> importFieldMapMock;


        public OdbcStoreSettingsTelemetryCollectorTest()
        {
            odbcStore = new OdbcStoreEntity();
            mock = AutoMock.GetLoose();

            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();

            MockDataSourceService();
            MockFieldMap();
        }

        [Fact]
        public void CollectTelemetry_ImportDriverSetFromDataSourceService()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.Driver", importDriver));
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomQuery)]
        [InlineData(OdbcColumnSourceType.Table)]
        public void CollectTelemetry_ImportColumnSourceTypeSetFromOdbcStore(OdbcColumnSourceType sourceType)
        {
            odbcStore.ImportColumnSourceType = (int) sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.ColumnSourceType", EnumHelper.GetDescription(sourceType)));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToUnknown_WhenNoOrderNumberEntry()
        {
            importFieldMapMock.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "OrderNumber"), true))
                .Returns(() => new IOdbcFieldMapEntry[0]);

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.IsSingleLine", "Unknown"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToYes_WhenOneItemPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.IsSingleLine", "Yes"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenTwoItemsPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            maxEntryIndex = 1;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.IsSingleLine", "No"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenOneItemPerOrder_AndRecordIdentifierSourceNotEqualToOrderNumberExternalFieldColumnName()
        {
            orderNumberExternalColumnName = "AnotherName";
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.IsSingleLine", "No"));
        }

        [Fact]
        public void CollectTelemetry_UploadDriverNameIsSetFromDataSourceService()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e=>e.AddProperty("Upload.Driver", uploadDriver));
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

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.Strategy", EnumHelper.GetDescription(sourceType)));
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomQuery)]
        [InlineData(OdbcColumnSourceType.Table)]
        public void CollectTelemetry_UploadColumnSourceTypeSetFromOdbcStore(OdbcColumnSourceType sourceType)
        {
            odbcStore.UploadColumnSourceType = (int)sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStore, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.ColumnSourceType", EnumHelper.GetDescription(sourceType)));
        }

        private void MockDataSourceService()
        {
            var uploadDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            uploadDataSource.Setup(d => d.Driver).Returns(() => uploadDriver);

            var importDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            importDataSource.Setup(d => d.Driver).Returns(() => importDriver);

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
            

            var fieldMapFactoryMock = mock.MockRepository.Create<Func<IOdbcFieldMap>>();
            fieldMapFactoryMock.Setup(f => f()).Returns(importFieldMapMock.Object);

            mock.Provide(fieldMapFactoryMock.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
