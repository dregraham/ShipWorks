using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.AmeriCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "AmeriCommerceStore")]
    public class AmeriCommerceStoreTypeTest : IDisposable
    {
        private readonly DataContext context;
        private const StoreTypeCode storeTypeCode = StoreTypeCode.AmeriCommerce;
        private readonly AmeriCommerceStoreEntity store;

        public AmeriCommerceStoreTypeTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();

            });

            store = Create.Store<AmeriCommerceStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Magento Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.AmeriCommerce)
                .Save();
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        public async Task AmeriCommerceStoreType_GetCombinedOnlineOrderIdentifiers_ReturnsCorrectValues(int expectedCount, int expectedFirstResult)
        {
            store.StoreTypeCode = storeTypeCode;

            OrderEntity order = Create.Order(context.Store, context.Customer)
                .WithOrderNumber(12345)
                .Save();

            CreateOrderSearchEntities(order.OrderID, expectedCount);

            CombineOrderNumberSearchProvider searchProvider = new CombineOrderNumberSearchProvider();
            var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            Assert.Equal(expectedCount, results?.Count());
            Assert.Equal(expectedFirstResult, results?.FirstOrDefault());
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
