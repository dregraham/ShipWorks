using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
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

        public OdbcStoreDtoFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

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
    }
}