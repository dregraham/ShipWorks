using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericFile;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcStoreTypeTest
    {
        private readonly AutoMock mock;
        private readonly OdbcStoreEntity store;

        public OdbcStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new OdbcStoreEntity { TypeCode = (int) StoreTypeCode.Odbc };
        }

        [Fact]
        public void TypeCode_IsOdbc()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            Assert.Equal(StoreTypeCode.Odbc, testObject.TypeCode);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsGenericFileOrderIdentifier()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));
            var order = new OrderEntity() { OrderNumber = 42 };
            OrderIdentifier orderIdentifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<AlphaNumericOrderIdentifier>(orderIdentifier);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsOdbcStoreEntity()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));
            var newStore = testObject.CreateStoreInstance();

            Assert.IsType<OdbcStoreEntity>(newStore);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsStoreWithEmptyConnectionString()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));
            var newStore = testObject.CreateStoreInstance();

            Assert.Empty(((OdbcStoreEntity) newStore).ImportConnectionString);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsStoreWithEmptyImportMap()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));
            var newStore = testObject.CreateStoreInstance();

            Assert.Empty(((OdbcStoreEntity) newStore).ImportMap);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsOdbcStoreEntity_WithImportStrategyByModifiedTime()
        {
            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            OdbcStoreEntity odbcStore = testObject.CreateStoreInstance() as OdbcStoreEntity;

            Assert.Equal((int) OdbcImportStrategy.ByModifiedTime, odbcStore.ImportStrategy);
        }

        [Fact]
        public void CreateStoreInstance_SetsShipmentUploadStrategyToDoNotUpload()
        {
            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            OdbcStoreEntity odbcStore = testObject.CreateStoreInstance() as OdbcStoreEntity;

            Assert.Equal((int) OdbcShipmentUploadStrategy.DoNotUpload, odbcStore.UploadStrategy);
        }

        [Fact]
        public void CreateStoreInstance_SetsUploadMapToEmptyString()
        {
            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            OdbcStoreEntity odbcStore = testObject.CreateStoreInstance() as OdbcStoreEntity;

            Assert.Empty(odbcStore.UploadMap);
        }

        [Fact]
        public void CreateStoreInstance_SetUploadColumnSourceTypeToTable()
        {
            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            OdbcStoreEntity odbcStore = testObject.CreateStoreInstance() as OdbcStoreEntity;

            Assert.Equal((int) OdbcColumnSourceType.Table, odbcStore.UploadColumnSourceType);
        }

        [Fact]
        public void CreateStoreInstance_SetsUploadColumnSourceToEmpty()
        {
            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            OdbcStoreEntity odbcStore = testObject.CreateStoreInstance() as OdbcStoreEntity;

            Assert.Empty(odbcStore.UploadColumnSource);
        }


        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenColumnIsOnlineStatus()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenColumnIsLastModified()
        {
            var testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.LastModified));
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsNull_WhenUploadStrategyIsDoNotUpload()
        {
            store.UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload;

            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            Assert.Null(testObject.CreateAddStoreWizardOnlineUpdateActionControl());
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsOnlineUpdateShipmentUpdateActionControl_WhenUploadStrategyIsNotDoNotUpload()
        {
            store.UploadStrategy = (int) OdbcShipmentUploadStrategy.UseImportDataSource;

            OdbcStoreType testObject = mock.Create<OdbcStoreType>(TypedParameter.From<StoreEntity>(store));

            Assert.IsAssignableFrom<OnlineUpdateShipmentUpdateActionControl>(testObject.CreateAddStoreWizardOnlineUpdateActionControl());
        }
    }
}
