using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Stores.Tests.Integration.Platforms.GenericModule
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "GenericModuleStoreType")]
    public class GenericModuleStoreTypeTest : IDisposable
    {
        private readonly DataContext context;
        private readonly GenericModuleStoreEntity store;

        public GenericModuleStoreTypeTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();
                
            });

            store = Create.Store<GenericModuleStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Magento Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.GenericModule)
                .Set(x => x.ModuleUrl, "https://www.com")
                .Save();
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "1")]
        [InlineData(2, "1")]
        public async Task GenericModuleStoreType_GetCombinedOnlineOrderIdentifiers_ReturnsCorrectValues(int expectedCount, string expectedFirstResult)
        {
            var storeTypeCodes = EnumHelper.GetEnumList<StoreTypeCode>()
                .Select(x => x.Value)
                .Where(x => x != StoreTypeCode.Invalid);

            foreach (var storeTypeCode in storeTypeCodes)
            {
                store.StoreTypeCode = storeTypeCode;
                StoreType storeType = StoreTypeManager.GetType(storeTypeCode);


                if (storeType is GenericModuleStoreType)
                {
                    CombineOrderNumberCompleteSearchProvider searchProvider = new CombineOrderNumberCompleteSearchProvider();
                    IOrderEntity order = Create.Order(context.Store, context.Customer)
                        .WithOrderNumber(12345)
                        .Save();

                    CreateOrderSearchEntities(order.OrderID, expectedCount);

                    var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                    Assert.Equal(expectedCount, results?.Count());
                    Assert.Equal(expectedFirstResult, results?.FirstOrDefault());
                }
            }
        }

        private void CreateOrderSearchEntities(long orderID, int numberToCreate)
        {
            for (int i = 1; i < numberToCreate + 1; i++)
            {
                Create.Entity<OrderSearchEntity>()
                    .Set(os => os.Store, store)
                    .Set(os => os.OrderNumber, i)
                    .Set(os => os.OrderNumberComplete, i.ToString())
                    .Set(os => os.IsManual, false)
                    .Set(os => os.OrderID, orderID)
                    .Save();
            }
        }

        public void Dispose() => context.Dispose();
    }
}
