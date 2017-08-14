using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CombineOrderNumberCompleteSearchProviderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly GenericModuleStoreEntity store;

        public CombineOrderNumberCompleteSearchProviderTest(DatabaseFixture db)
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
        [InlineData(0, 0, null,         0)]
        [InlineData(1, 1, "12345-1-OS", 0)]
        [InlineData(2, 2, "12345-1-OS", 0)]
        [InlineData(0, 0, null, 1)]
        [InlineData(1, 0, null, 1)]
        [InlineData(2, 1, "12345-1-OS", 1)]
        public async Task GetCombinedOnlineOrderIdentifiers_ReturnsCorrectValues_WhenCombined(int numberToCreate, int expectedCount, string expectedFirstResult, int manualOrders)
        {
            var storeTypeCodes = EnumHelper.GetEnumList<StoreTypeCode>()
                .Select(x => x.Value)
                .Where(x => x == StoreTypeCode.GenericModule || x == StoreTypeCode.GenericFile);

            foreach (var storeTypeCode in storeTypeCodes)
            {
                store.StoreTypeCode = storeTypeCode;
                CombineOrderNumberCompleteSearchProvider searchProvider = new CombineOrderNumberCompleteSearchProvider();

                OrderEntity order = Create.Order(context.Store, context.Customer)
                    .WithOrderNumber(12345)
                    .Set(o => o.CombineSplitStatus, CombineSplitStatusType.Combined)
                    .Save();

                CreateOrderSearchEntities(order, numberToCreate, manualOrders);

                var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                Assert.Equal(expectedCount, results?.Count());
                Assert.Equal(expectedFirstResult, results?.FirstOrDefault());
            }
        }

        [Theory]
        [InlineData(0, 1, "12345", 0)]
        [InlineData(1, 1, "12345", 0)]
        [InlineData(2, 1, "12345", 0)]
        [InlineData(0, 1, "12345", 1)]
        [InlineData(1, 1, "12345", 1)]
        [InlineData(2, 1, "12345", 1)]
        public async Task GetCombinedOnlineOrderIdentifiers_ReturnsCorrectValues_WhenNotCombined(int numberToCreate, int expectedCount, string expectedFirstResult, int manualOrders)
        {
            var storeTypeCodes = EnumHelper.GetEnumList<StoreTypeCode>()
                .Select(x => x.Value)
                .Where(x => x == StoreTypeCode.GenericModule || x == StoreTypeCode.GenericFile);

            foreach (var storeTypeCode in storeTypeCodes)
            {
                store.StoreTypeCode = storeTypeCode;
                CombineOrderNumberCompleteSearchProvider searchProvider = new CombineOrderNumberCompleteSearchProvider();

                OrderEntity order = Create.Order(context.Store, context.Customer)
                    .WithOrderNumber(12345)
                    .Set(o => o.CombineSplitStatus, CombineSplitStatusType.None)
                    .Save();

                CreateOrderSearchEntities(order, numberToCreate, manualOrders);

                var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                Assert.Equal(expectedCount, results?.Count());
                Assert.Equal(expectedFirstResult, results?.FirstOrDefault());
            }
        }

        private void CreateOrderSearchEntities(IOrderEntity order, int numberTotalToCreate, int numberManualToCreate)
        {
            for (int i = 1; i < numberTotalToCreate - numberManualToCreate + 1; i++)
            {
                CreateOrder(order, i, false);
            }

            for (int i = 1; i < numberManualToCreate + 1; i++)
            {
                CreateOrder(order, i, true);
            }
        }

        private void CreateOrder(IOrderEntity order, int orderNumber, bool isManual)
        {
            Create.Entity<OrderSearchEntity>()
                .Set(os => os.Store, store)
                .Set(os => os.OrderNumber, orderNumber)
                .Set(os => os.OrderNumberComplete, $"{order.OrderNumberComplete}-{orderNumber}-OS")
                .Set(os => os.IsManual, isManual)
                .Set(os => os.OrderID, order.OrderID)
                .Save();
        }

        public void Dispose() => context.Dispose();
    }
}
