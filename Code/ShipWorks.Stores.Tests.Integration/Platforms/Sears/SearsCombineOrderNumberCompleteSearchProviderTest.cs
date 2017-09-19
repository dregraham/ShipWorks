using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Sears
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Sears")]
    public class SearsCombineOrderNumberCompleteSearchProviderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly SearsStoreEntity store;

        public SearsCombineOrderNumberCompleteSearchProviderTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();

            });

            store = Create.Store<SearsStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Sears Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.Sears)
                .Set(x => x.SearsEmail, "me@me.com")
                .Set(x => x.Password, "pwd")
                .Set(x => x.SecretKey, "SecretKey")
                .Set(x => x.SellerID, "SellerID")
                .Save();
        }

        [Theory]
        [InlineData(CombineSplitStatusType.None, 0, 1, "12345")]
        [InlineData(CombineSplitStatusType.None, 1, 1, "12345")]
        [InlineData(CombineSplitStatusType.Combined, 1, 1, "12345-OS")]
        [InlineData(CombineSplitStatusType.None, 2, 1, "12345")]
        [InlineData(CombineSplitStatusType.Combined, 2, 2, "12345-OS")]
        public async Task GetCombinedOnlineOrderIdentifiers_ReturnsCorrectValues(CombineSplitStatusType combineSplitStatusType,
            int numberToCreate, int expectedCount, string expectedFirstResult)
        {
            SearsCombineOrderSearchProvider searchProvider = IoC.UnsafeGlobalLifetimeScope.Resolve<SearsCombineOrderSearchProvider>();

            SearsOrderEntity order = Create.Order<SearsOrderEntity>(context.Store, context.Customer)
                .WithOrderNumber(12345)
                .Set(x => x.PoNumber, "12345")
                .Set(x => x.PoNumberWithDate, "1234520170824")
                .Set(x => x.LocationID, 1)
                .Set(x => x.Commission, 1)
                .Set(x => x.CustomerPickup, false)
                .Set(x => x.CombineSplitStatus, combineSplitStatusType)
                .Save();

            CreateOrderSearchEntities(order, numberToCreate);

            var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            Assert.Equal(expectedCount, results?.Count());
            Assert.Equal(expectedFirstResult, results?.FirstOrDefault().PoNumber);
        }

        private void CreateOrderSearchEntities(IOrderEntity order, int numberToCreate)
        {
            for (int i = 1; i < numberToCreate + 1; i++)
            {
                Create.Entity<OrderSearchEntity>()
                    .Set(os => os.Store, store)
                    .Set(os => os.OrderNumber, i)
                    .Set(os => os.OrderNumberComplete, i.ToString())
                    .Set(os => os.IsManual, false)
                    .Set(os => os.OrderID, order.OrderID)
                    .Save();

                Create.Entity<SearsOrderSearchEntity>()
                    .Set(os => os.PoNumber, $"{order.OrderNumberComplete}-OS")
                    .Set(os => os.OriginalOrderID, order.OrderID)
                    .Set(os => os.OrderID, order.OrderID)
                    .Save();
            }
        }

        public void Dispose() => context.Dispose();
    }
}
