using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Magento")]
    public class MagentoTwoRestDownloaderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly IProgressReporter progress;
        private readonly DownloadEntity downloadEntity;
        private readonly MagentoStoreEntity magentoStore;
        private Order order;
        private OrdersResponse ordersResponse;

        public MagentoTwoRestDownloaderTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);

            CreateOrderResponse();

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();

                Mock<IMagentoTwoRestClient> restClient = new Mock<IMagentoTwoRestClient>();
                restClient.SetupSequence(w => w.GetOrders(It.IsAny<DateTime?>(), It.IsAny<int>())).Returns(ordersResponse).Returns(new OrdersResponse() { TotalCount = 0, Orders = new List<Order>() });
                restClient.Setup(w => w.GetOrder(It.IsAny<long>())).Returns(order);

                var webClientFactory = mock.CreateMock<Func<MagentoStoreEntity, IMagentoTwoRestClient>>();
                webClientFactory.Setup(f => f(It.IsAny<MagentoStoreEntity>()))
                    .Returns(restClient.Object);
                mock.Provide(webClientFactory.Object);
            });

            magentoStore = Create.Store<MagentoStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Magento Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.Magento)
                .Set(x => x.ModuleUrl, "https://www.com")
                .Save();

            context.Mock.Provide<StoreEntity>(magentoStore);

            downloadEntity = Create.Entity<DownloadEntity>()
                .Set(d => d.StoreID, magentoStore.StoreID)
                .Set(d => d.ComputerID, context.Computer.ComputerID)
                .Set(d => d.UserID, context.User.UserID)
                .Set(d => d.InitiatedBy, 0)
                .Set(d => d.Started, DateTime.Now)
                .Set(d => d.Result, 0)
                .Save();

            var statusCreator = Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID, magentoStore.StoreID)
                .Set(x => x.StatusText, "status")
                .Set(x => x.IsDefault, true);

            statusCreator.Set(x => x.StatusTarget, 0).Save();
            statusCreator.Set(x => x.StatusTarget, 1).Save();

            StatusPresetManager.CheckForChanges();

            progress = context.Mock.Mock<IProgressReporter>().Object;
            StatusPresetManager.CheckForChanges();
        }

        [Fact]
        public async Task MagentoTwoRestDownloader_Download_DownloadsOrder()
        {
            var testObject = context.Mock.Create<MagentoTwoRestDownloader>();

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {

                    await testObject.Download(progress, downloadEntity.DownloadID, con).ConfigureAwait(false);
                }
            }

            int numberOfOrdersInDb;
            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Order
                    .Select(OrderFields.OrderID.Count())
                    .Where(OrderFields.StoreID == magentoStore.StoreID);
                numberOfOrdersInDb = await sqlAdapter.FetchScalarAsync<int>(query);
            }

            Assert.Equal(2, numberOfOrdersInDb);
        }

        private void CreateOrderResponse()
        {
            order = new Order()
            {
                CreatedAt = DateTime.Now.AddDays(-2).ToString("G"),
                CustomerId = 3,
                DiscountAmount = 0,
                DiscountDescription = "None",
                EntityId = 3,
                GrandTotal = 0.0,
                IncrementId = "3",
                Status = "New",
                UpdatedAt = DateTime.Now.AddDays(-1).ToString("G"),
                ExtensionAttributes = new ExtensionAttributes() { ShippingAssignments = new List<ShippingAssignment>() },
                StatusHistories = new List<StatusHistory>() { new StatusHistory() { Comment = "note comment", CreatedAt = DateTime.Now.ToString("G") } },
                Payment = new Payment() { Method = "CC" },
                BillingAddress = new BillingAddress()
                {
                    City = "ST. Louis",
                    CountryId = "USA",
                    Email = "me@me.com",
                    Firstname = "John",
                    Lastname = "Doe",
                    Postcode = "63102",
                    Street = new List<string> { "1 s memorial drive" },
                    Telephone = "3145551212"
                },
                Items = new List<Item>
                {
                    new Item()
                    {
                        ItemId = 2,
                        Name = "Product 1",
                        Price = 0,
                        QtyOrdered = 1,
                        Sku = "P1",
                        Weight = 0.5
                    }
                }
            };

            Order order2 = order.ShallowCopy();
            order2.EntityId++;
            order2.IncrementId = order2.EntityId.ToString();

            ordersResponse = new OrdersResponse() { Orders = new[] { order, order2 }, TotalCount = 2 };
        }

        public void Dispose() => context.Dispose();
    }
}
