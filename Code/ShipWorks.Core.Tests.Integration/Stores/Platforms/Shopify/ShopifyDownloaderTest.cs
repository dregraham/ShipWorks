using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Shopify")]
    public class ShopifyDownloaderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly IProgressReporter progress;
        private readonly DownloadEntity downloadEntity;
        private readonly ShopifyStoreEntity shopifyStore;
        private List<JToken> orders;
        DateTime updatedAt = DateTime.Parse("2017-04-03T15:30:28-05:00").ToUniversalTime();
        Mock<IShopifyWebClient> webClient = new Mock<IShopifyWebClient>();

        public ShopifyDownloaderTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);
            DateTime updatedBefore = updatedAt.AddSeconds(-1);
            DateTime updatedAfter = updatedAt.AddSeconds(1);

            CreateOrders();

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();

                webClient.Setup(w => w.GetServerCurrentDateTime()).Returns(DateTime.UtcNow);

                webClient.Setup(w => w.GetOrderCount(It.IsInRange(updatedAfter, DateTime.MaxValue, Moq.Range.Inclusive),
                                                     It.IsInRange(DateTime.MinValue, updatedBefore, Moq.Range.Inclusive)))
                                                     .Returns(0);
                webClient.Setup(w => w.GetOrderCount(It.IsInRange(DateTime.MinValue, updatedAt, Moq.Range.Inclusive),
                                                     It.IsInRange(updatedAt, DateTime.MaxValue, Moq.Range.Inclusive)))
                    .Returns(7);

                webClient.SetupSequence(w => w.GetOrders(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns(new List<JToken> { orders[0], orders[1] })
                    .Returns(new List<JToken> { orders[2], orders[3] })
                    .Returns(new List<JToken> { orders[4], orders[5] })
                    .Returns(new List<JToken> { orders[6]})
                    .Returns(new List<JToken> { orders[0], orders[1] })
                    .Returns(new List<JToken> { orders[2], orders[3] })
                    .Returns(new List<JToken> { orders[4], orders[5] })
                    .Returns(new List<JToken> { orders[6] });

                webClient.Setup(w => w.GetProduct(It.IsAny<long>())).Returns(GetProduct);
                mock.Provide(webClient);

                var webClientFactory = mock.CreateMock<Func<ShopifyStoreEntity, IProgressReporter, IShopifyWebClient>>();
                webClientFactory.Setup(f => f(It.IsAny<ShopifyStoreEntity>(), It.IsAny<IProgressReporter>()))
                    .Returns(webClient.Object);
                mock.Provide(webClientFactory.Object);
            });

            shopifyStore = Create.Store<ShopifyStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Shopify Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.Shopify)
                .Set(x => x.ShopifyShopUrlName, "https://www.com")
                .Save();

            context.Mock.Provide<StoreEntity>(shopifyStore);
            context.Mock.Provide<ShopifyStoreEntity>(shopifyStore);

            downloadEntity = Create.Entity<DownloadEntity>()
                .Set(d => d.StoreID, shopifyStore.StoreID)
                .Set(d => d.ComputerID, context.Computer.ComputerID)
                .Set(d => d.UserID, context.User.UserID)
                .Set(d => d.InitiatedBy, 0)
                .Set(d => d.Started, DateTime.Now)
                .Set(d => d.Result, 0)
                .Save();

            var statusCreator = Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID, shopifyStore.StoreID)
                .Set(x => x.StatusText, "status")
                .Set(x => x.IsDefault, true);

            statusCreator.Set(x => x.StatusTarget, 0).Save();
            statusCreator.Set(x => x.StatusTarget, 1).Save();

            StatusPresetManager.CheckForChanges();

            progress = context.Mock.Mock<IProgressReporter>().Object;
            StatusPresetManager.CheckForChanges();
            LogSession.Initialize();
        }

        [Fact]
        public async Task ShopifyTwoRestDownloader_Download_WithSameOnlineLastModified_DownloadsOrders()
        {
            var testObject = context.Mock.Create<ShopifyDownloader>();

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    await testObject.Download(progress, downloadEntity.DownloadID, con);
                }
            }

            int numberOfOrdersInDb;
            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Order
                    .Select(OrderFields.OrderID.Count())
                    .Where(OrderFields.StoreID == shopifyStore.StoreID)
                    .AndWhere(OrderFields.OnlineLastModified == "2017-04-03 20:30:28.0000000");
                numberOfOrdersInDb = await sqlAdapter.FetchScalarAsync<int>(query);
            }

            Assert.Equal(7, numberOfOrdersInDb);
        }

        [Fact]
        public async Task ShopifyTwoRestDownloader_Download_DoesntMissOrdersAfterCrashInMidDownloadWithOrdersOfSameOnlineLastModified()
        {
            int numberOfOrdersInDb;
            List<JToken> crashingPage = GetOrdersPageThatCrashes();

            webClient.SetupSequence(w => w.GetOrders(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new List<JToken> { orders[0], orders[1] })
                .Returns(new List<JToken> { crashingPage[0], crashingPage[1] })
                .Returns(new List<JToken> { orders[2], orders[3] })
                .Returns(new List<JToken> { orders[4], orders[5] })
                .Returns(new List<JToken> { orders[6] });

            var testObject = context.Mock.Create<ShopifyDownloader>();

            try
            {
                using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                {
                    using (DbConnection con = SqlSession.Current.OpenConnection())
                    {
                        await testObject.Download(progress, downloadEntity.DownloadID, con);
                    }
                }
            }
            catch
            {
                using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
                {
                    var query = new QueryFactory().Order
                        .Select(OrderFields.OrderID.Count())
                        .Where(OrderFields.StoreID == shopifyStore.StoreID)
                        .AndWhere(OrderFields.OnlineLastModified == "2017-04-03 20:30:28.0000000");
                    numberOfOrdersInDb = await sqlAdapter.FetchScalarAsync<int>(query);
                    Assert.Equal(2, numberOfOrdersInDb);
                }
            }

            // Setup to try again with good orders.
            webClient.SetupSequence(w => w.GetOrders(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new List<JToken> { orders[0], orders[1] })
                .Returns(new List<JToken> { orders[2], orders[3] })
                .Returns(new List<JToken> { orders[4], orders[5] })
                .Returns(new List<JToken> { orders[6] });

            testObject = context.Mock.Create<ShopifyDownloader>();

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    await testObject.Download(progress, downloadEntity.DownloadID, con);
                }
            }

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Order
                    .Select(OrderFields.OrderID.Count())
                    .Where(OrderFields.StoreID == shopifyStore.StoreID)
                    .AndWhere(OrderFields.OnlineLastModified == "2017-04-03 20:30:28.0000000");
                numberOfOrdersInDb = await sqlAdapter.FetchScalarAsync<int>(query);
            }

            Assert.Equal(7, numberOfOrdersInDb);
        }

        private void CreateOrders()
        {
            orders = new List<JToken>();
            orders.AddRange(ParseOrderResponse("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetOrdersPage1.txt"));
            orders.AddRange(ParseOrderResponse("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetOrdersPage2.txt"));
            orders.AddRange(ParseOrderResponse("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetOrdersPage3.txt"));
            orders.AddRange(ParseOrderResponse("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetOrdersPage4.txt"));
        }

        private JToken GetProduct()
        {
            return JToken.Parse(ResourceUtility.ReadString("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetProduct.txt"));
        }

        private List<JToken> GetOrdersPageThatCrashes()
        {
            return ParseOrderResponse("ShipWorks.Core.Tests.Integration.Stores.Platforms.Shopify.Resources.GetOrdersPage_Crash.txt");
        }

        private List<JToken> ParseOrderResponse(string resourcePath)
        {
            var ordersArray = JToken.Parse(ResourceUtility.ReadString(resourcePath))
                .SelectToken("orders");

            return ordersArray.ToList();
        }

        public void Dispose() => context.Dispose();
    }
}
