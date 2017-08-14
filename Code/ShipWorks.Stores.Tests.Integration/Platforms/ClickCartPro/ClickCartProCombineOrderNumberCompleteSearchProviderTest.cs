using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.ClickCartPro;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ClickCartPro
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "ClickCartPro")]
    public class ClickCartProCombineOrderNumberCompleteSearchProviderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly GenericModuleStoreEntity store;

        public ClickCartProCombineOrderNumberCompleteSearchProviderTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();
                
            });

            store = Create.Store<GenericModuleStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A ClickCartPro Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.ClickCartPro)
                .Set(x => x.ModuleUrl, "https://www.com")
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
            ClickCartProCombineOrderNumberCompleteSearchProvider searchProvider = new ClickCartProCombineOrderNumberCompleteSearchProvider();

            ClickCartProOrderEntity order = Create.Order<ClickCartProOrderEntity>(context.Store, context.Customer)
                .WithOrderNumber(12345)
                .Set(x => x.ClickCartProOrderID, "12345")
                .Set(x => x.CombineSplitStatus, combineSplitStatusType)
                .Save();

            CreateOrderSearchEntities(order, numberToCreate);

            var results = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            Assert.Equal(expectedCount, results?.Count());
            Assert.Equal(expectedFirstResult, results?.FirstOrDefault());
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

                Create.Entity<ClickCartProOrderSearchEntity>()
                    .Set(os => os.ClickCartProOrderID,  $"{order.OrderNumberComplete}-OS")
                    .Set(os => os.OriginalOrderID, order.OrderID)
                    .Set(os => os.OrderID, order.OrderID)
                    .Save();
            }
        }

        public void Dispose() => context.Dispose();
    }
}
