using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericFile;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcStoreTypeTest
    {
        private readonly OdbcStoreEntity store;

        public OdbcStoreTypeTest()
        {
            store = new OdbcStoreEntity {TypeCode = (int) StoreTypeCode.Odbc};
        }

        [Fact]
        public void TypeCode_IsOdbc()
        {
            var testObject = new OdbcStoreType(store, null);

            Assert.Equal(StoreTypeCode.Odbc, testObject.TypeCode);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsGenericFileOrderIdentifier()
        {
            var testObject = new OdbcStoreType(store, null);
            var order = new OrderEntity() {OrderNumber = 42};
            OrderIdentifier orderIdentifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<GenericFileOrderIdentifier>(orderIdentifier);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsOdbcStoreEntity()
        {
            var testObject = new OdbcStoreType(store, null);
            var newStore = testObject.CreateStoreInstance();

            Assert.IsType<OdbcStoreEntity>(newStore);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsStoreWithEmptyConnectionString()
        {
            var testObject = new OdbcStoreType(store, null);
            var newStore = testObject.CreateStoreInstance();

            Assert.Empty(((OdbcStoreEntity) newStore).ConnectionString);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsStoreWithEmptyMap()
        {
            var testObject = new OdbcStoreType(store, null);
            var newStore = testObject.CreateStoreInstance();

            Assert.Empty(((OdbcStoreEntity) newStore).Map);
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenColumnIsOnlineStatus()
        {
            var testObject = new OdbcStoreType(store, null);

            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }

        [Fact]
        public void GridOnlineColumnSupported_ReturnsTrue_WhenColumnIsLastModified()
        {
            var testObject = new OdbcStoreType(store, null);

            Assert.True(testObject.GridOnlineColumnSupported(OnlineGridColumnSupport.LastModified));
        }
    }
}
