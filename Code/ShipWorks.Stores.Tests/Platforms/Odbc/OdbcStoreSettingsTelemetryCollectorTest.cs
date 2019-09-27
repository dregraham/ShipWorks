﻿using System;
using System.Diagnostics.CodeAnalysis;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcStoreSettingsTelemetryCollectorTest : IDisposable
    {
        private readonly AutoMock mock;
        readonly OdbcStoreEntity odbcStoreEntity;
        private readonly OdbcStore odbcStore;
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
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            odbcStoreEntity = new OdbcStoreEntity()
            {
                ImportConnectionString = "not a null string",
                UploadConnectionString = "also not null"
            };

            odbcStore = new OdbcStore();
            mock.Mock<IOdbcStoreRepository>()
                .Setup(r => r.GetStore(odbcStoreEntity))
                .Returns(odbcStore);


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
        [SuppressMessage("SonarLint", "S112: Exception should not be thrown by user code",
            Justification = "General exception is just meant as a throwaway for testing")]
        public void CollectTelemetry_AddsErrorProperty_WhenExceptionOccurs()
        {
            // Throw an exception when the import driver property is added
            trackedDurationEventMock.Setup(e => e.AddProperty("Import.Driver", It.IsAny<string>())).Throws(new Exception("the message"));

            OdbcStoreSettingsTelemetryCollector testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Error", "the message"));
        }

        [Fact]
        public void CollectTelemetry_ImportDriverSetFromDataSourceService()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();

            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.Driver", ImportDriverName));
        }

        [Fact]
        public void CollectTelemetry_ImportDriverSetToNone_WhenImportConnectionStringIsEmpty()
        {
            odbcStoreEntity.ImportConnectionString = string.Empty;
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();

            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.Driver", "None"));
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomParameterizedQuery)]
        [InlineData(OdbcColumnSourceType.CustomQuery)]
        [InlineData(OdbcColumnSourceType.Table)]
        public void CollectTelemetry_ImportColumnSourceTypeSetFromOdbcStore(OdbcColumnSourceType sourceType)
        {
            odbcStoreEntity.ImportColumnSourceType = (int) sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.QueryType", EnumHelper.GetApiValue(sourceType)));
        }

        [Fact]
        public void CollectTelemetry_ImportColumnSourceTypeSetToNone_WhenDriveIsNone()
        {
            odbcStoreEntity.ImportConnectionString = string.Empty;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.QueryType", "None"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToUnknown_WhenNoOrderNumberEntry()
        {
            odbcStore.ImportOrderItemStrategy = (int) OdbcImportOrderItemStrategy.SingleLine;
            importFieldMapMock.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "OrderNumber"), true))
                .Returns(() => new IOdbcFieldMapEntry[0]);

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Unknown"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToYes_WhenOneItemPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Single line"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenTwoItemsPerOrder_AndRecordIdentifierSourceEqualsOrderNumberExternalFieldColumnName()
        {
            maxEntryIndex = 1;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Multi-line"));
        }

        [Fact]
        public void CollectTelemetry_IsSingleLineSetToNo_WhenOneItemPerOrder_AndRecordIdentifierSourceNotEqualToOrderNumberExternalFieldColumnName()
        {
            orderNumberExternalColumnName = "AnotherName";
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "Multi-line"));
        }

        [Fact]
        public void CollectTelemetry_OrderItemDataStructure_SetToNone_WhenImportDriveIsNone()
        {
            odbcStoreEntity.ImportConnectionString = string.Empty;
            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "None"));
        }

        [Fact]
        public void CollectTelemetry_OrderItemDataStructure_SetToNone_WhenMapHasNoEntries()
        {
            MockEmptyFieldMap();

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.OrderItemDataStructure", "None"));
        }

        [Theory]
        [InlineData(OdbcShipmentUploadStrategy.DoNotUpload)]
        [InlineData(OdbcShipmentUploadStrategy.UseImportDataSource)]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource)]
        public void CollectTelemetry_UploadStrategyTypeSetFromOdbcStore(OdbcShipmentUploadStrategy sourceType)
        {
            OdbcStore store = new OdbcStore();
            store.UploadStrategy = (int) sourceType;
            var storeRepo = mock.Mock<IOdbcStoreRepository>();

            storeRepo.Setup(s => s.GetStore(It.IsAny<OdbcStoreEntity>())).Returns(store);

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

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
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.Driver", expectedResult), Times.Once);
        }

        [Theory]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource, OdbcColumnSourceType.CustomQuery, "CustomQuery")]
        [InlineData(OdbcShipmentUploadStrategy.UseShipmentDataSource, OdbcColumnSourceType.CustomParameterizedQuery, "CustomParameterizedQuery")]
        [InlineData(OdbcShipmentUploadStrategy.UseImportDataSource, OdbcColumnSourceType.Table, "Table")]
        [InlineData(OdbcShipmentUploadStrategy.DoNotUpload, OdbcColumnSourceType.Table, "None")]
        public void CollectTelemetry_UploadColumnSourceTypeSetFromOdbcStore(OdbcShipmentUploadStrategy strategy, OdbcColumnSourceType sourceType, string expectedResult)
        {
            odbcStore.UploadStrategy = (int) strategy;
            odbcStore.UploadColumnSourceType = (int) sourceType;

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Upload.QueryType", expectedResult));
        }

        [Theory]
        [InlineData(OdbcImportStrategy.All)]
        [InlineData(OdbcImportStrategy.ByModifiedTime)]
        [InlineData(OdbcImportStrategy.OnDemand)]
        public void CollectTelemetry_ImportStrategyTypeSetFromOdbcStore(OdbcImportStrategy sourceType)
        {
            OdbcStore store = new OdbcStore();
            store.ImportStrategy = (int) sourceType;
            var storeRepo = mock.Mock<IOdbcStoreRepository>();

            storeRepo.Setup(s => s.GetStore(It.IsAny<OdbcStoreEntity>())).Returns(store);

            var testObject = mock.Create<OdbcStoreSettingsTelemetryCollector>();
            testObject.CollectTelemetry(odbcStoreEntity, trackedDurationEventMock.Object);

            trackedDurationEventMock.Verify(e => e.AddProperty("Import.Strategy", EnumHelper.GetApiValue(sourceType)));
        }

        private void MockDataSourceService()
        {
            var uploadDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            uploadDataSource.Setup(d => d.Driver).Returns(() => UploadDriverName);

            var importDataSource = mock.MockRepository.Create<IOdbcDataSource>();
            importDataSource.Setup(d => d.Driver).Returns(() => ImportDriverName);

            var dataSourceServiceMock = mock.Mock<IOdbcDataSourceService>();
            dataSourceServiceMock.Setup(s => s.GetUploadDataSource(odbcStoreEntity, false)).Returns(uploadDataSource.Object);
            dataSourceServiceMock.Setup(s => s.GetImportDataSource(odbcStoreEntity)).Returns(importDataSource.Object);
        }

        private void MockFieldMap()
        {
            var orderNumberExternalFieldMock = mock.MockRepository.Create<IExternalOdbcMappableField>();
            orderNumberExternalFieldMock.Setup(e => e.Column).Returns(() => new OdbcColumn(orderNumberExternalColumnName, "unknown"));

            var orderNumberFieldMapEntryMock = mock.MockRepository.Create<IOdbcFieldMapEntry>();
            orderNumberFieldMapEntryMock.Setup(e => e.ExternalField).Returns(() => orderNumberExternalFieldMock.Object);
            orderNumberFieldMapEntryMock.Setup(e => e.Index).Returns(() => maxEntryIndex);

            importFieldMapMock = mock.MockRepository.Create<IOdbcFieldMap>();
            importFieldMapMock.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "OrderNumber"), true))
                .Returns(() => new[] { orderNumberFieldMapEntryMock.Object });
            importFieldMapMock.Setup(m => m.RecordIdentifierSource).Returns(() => mapRecordIdentifierSource);
            importFieldMapMock.Setup(m => m.Entries).Returns(() => new[] { orderNumberFieldMapEntryMock.Object });


            fieldMapFactory = mock.MockRepository.Create<IOdbcFieldMapFactory>();
            fieldMapFactory.Setup(f => f.CreateEmptyFieldMap()).Returns(importFieldMapMock.Object);

            mock.Provide(fieldMapFactory.Object);
        }

        private void MockEmptyFieldMap()
        {
            importFieldMapMock = mock.MockRepository.Create<IOdbcFieldMap>();

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
