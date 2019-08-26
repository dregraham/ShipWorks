using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Warehouse
{
    public class OdbcStoreDtoFactoryTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IStoreDtoHelpers> storeDtoHelper;
        private readonly OdbcStoreDtoFactory testObject;

        private const string warehouseId = "warehouse id";

        public OdbcStoreDtoFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var configurationEntity = mock.CreateMock<IConfigurationEntity>();
            
            configurationEntity.SetupGet(e => e.WarehouseID).Returns(warehouseId);

            mock.Mock<IConfigurationData>()
                .Setup(d => d.FetchReadOnly())
                .Returns(configurationEntity.Object);

            storeDtoHelper = mock.Mock<IStoreDtoHelpers>();
            testObject = mock.Create<OdbcStoreDtoFactory>();
        }

        [Fact]
        public async Task Create_SetsNameAndStoreType()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() { StoreName = "foo", StoreTypeCode = StoreTypeCode.Odbc };

            await testObject
                .Create(store)
                .ConfigureAwait(false);

            storeDtoHelper.Verify(s => s.PopulateCommonData(store, It.IsAny<OdbcStore>()));
        }

        [Fact]
        public async Task Create_SetsWarehouseID_WhenShouldUploadWarehouseOrdersIsTrue()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() { StoreName = "foo", StoreTypeCode = StoreTypeCode.Odbc, ShouldUploadWarehouseOrders = true };

            var result = await testObject
                .Create(store)
                .ConfigureAwait(false);

            Assert.Equal(warehouseId, ((OdbcStore) result).OrderImportingWarehouseId);
        }

        [Fact]
        public async Task Create_DoesNotSetWarehouseID_WhenShouldUploadWarehouseOrdersIsTrue()
        {
            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                StoreName = "foo",
                StoreTypeCode = StoreTypeCode.Odbc,
                ShouldUploadWarehouseOrders = false
            };

            var result = await testObject
                .Create(store)
                .ConfigureAwait(false);

            Assert.True(string.IsNullOrEmpty(((OdbcStore) result).OrderImportingWarehouseId));
        }
    }
}