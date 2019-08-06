using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Warehouse
{
    public class OdbcStoreDtoFactoryTest
    {
        [Fact]
        public async Task Create_SetsNameAndStoreType()
        {
            var testObject = new OdbcStoreDtoFactory();
            string storeName = "foo";
            int storeType = (int) StoreTypeCode.Odbc;

            var result = await testObject
                .Create(new OdbcStoreEntity() {StoreName = storeName, StoreTypeCode = StoreTypeCode.Odbc})
                .ConfigureAwait(false);
            
            Assert.Equal(storeName, result.Name);
            Assert.Equal(storeType, result.StoreType);
        }
        
    }
}