using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Groupon
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Groupon")]
    public class GrouponDownloadTest : IDisposable
    {
        private readonly DataContext context;
        private readonly GrouponStoreEntity store;
        private readonly long downloadLogID;
        private readonly DateTime utcNow;
        private readonly HttpArchiveReplayServer replayServer;

        public GrouponDownloadTest(DatabaseFixture db)
        {
            replayServer = new HttpArchiveReplayServer("Platforms.Groupon.HttpRequests")
            {
                ErrorStatusCode = 412
            };
            utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>()
                    .SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<IGrouponWebClientConfiguration>()
                    .SetupGet(x => x.Endpoint)
                    .Returns(replayServer.TranslateUrl("https://scm.commerceinterface.com/api/v2"));
                mock.Override<ILogEntryFactory>();
            });

            store = Create.Store<GrouponStoreEntity>(StoreTypeCode.Groupon)
                .Set(x => x.SupplierID, "Foo")
                .Set(x => x.Token, "Bar")
                .Save();

            var statusCreator = Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.StatusText, "status")
                .Set(x => x.IsDefault, true);

            statusCreator.Set(x => x.StatusTarget, 0).Save();
            statusCreator.Set(x => x.StatusTarget, 1).Save();

            StatusPresetManager.CheckForChanges();

            downloadLogID = Create.Entity<DownloadEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.ComputerID = context.Computer.ComputerID)
                .Set(x => x.UserID = context.User.UserID)
                .Set(x => x.InitiatedBy = (int) DownloadInitiatedBy.User)
                .Set(x => x.Started = utcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int) DownloadResult.Unfinished)
                .Save().DownloadID;
        }

        [Fact]
        public async Task Download_CreatesOrder()
        {
            using (var webApp = replayServer.Start("Download_CreatesOrder.har"))
            {
                var downloader = IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<IStoreDownloader>(StoreTypeCode.Groupon, TypedParameter.From<StoreEntity>(store));

                using (DbConnection connection = SqlSession.Current.OpenConnection())
                {
                    await downloader.Download(context.Mock.Create<IProgressReporter>(), downloadLogID, connection);
                }

                var order = await GetOrderWithItems("GG-ZPKZ-WSTB-337S-VKMY");
                var product = order.OrderItems.OfType<IGrouponOrderItemEntity>().Single();

                Assert.Equal(1, order.OrderNumber);
                Assert.Equal("1 Memorial Dr.", order.BillStreet1);
                Assert.Equal("1 Memorial Dr.", order.ShipStreet1);
                Assert.Equal("Widget A", product.Name);
            }
        }

        [Fact]
        public async Task Download_MergesTwoOrders_WithExistingParentID()
        {
            using (var webApp = replayServer.Start("Download_CombinesOrder.har"))
            {
                var downloader = IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<IStoreDownloader>(StoreTypeCode.Groupon, TypedParameter.From<StoreEntity>(store));

                using (DbConnection connection = SqlSession.Current.OpenConnection())
                {
                    await downloader.Download(context.Mock.Create<IProgressReporter>(), downloadLogID, connection);
                }

                var childOrder1 = await GetOrderWithItems("GG-ABCD-WSTB-337S-VKMY");
                var childOrder2 = await GetOrderWithItems("GG-WXYZ-WSTB-337S-VKMY");

                Assert.Null(childOrder1);
                Assert.Null(childOrder2);

                var mergedOrder = await GetOrderWithItems("GG-ZPKZ-WSTB-337S-VKMY");
                var itemNames = mergedOrder.OrderItems.Select(x => x.Name).ToList();

                Assert.Contains("Widget A", itemNames);
                Assert.Contains("Widget B", itemNames);
                Assert.Contains("Widget C", itemNames);
            }
        }

        /// <summary>
        /// Get the max order id for the given store
        /// </summary>
        private static async Task<GrouponOrderEntity> GetOrderWithItems(string orderID)
        {
            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Order
                        .Where(GrouponOrderFields.GrouponOrderID == orderID)
                        .WithPath(OrderEntity.PrefetchPathOrderItems);
                return await sqlAdapter.FetchFirstAsync(query) as GrouponOrderEntity;
            }
        }

        public void Dispose() => context.Dispose();
    }
}