using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using HarSharp;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Moq;
using Owin;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Tests.Integration.Extensions;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BigCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "BigCommerce")]
    public class BigCommerceDownloadTest : IDisposable
    {
        private readonly DataContext context;
        private readonly BigCommerceStoreEntity store;
        private readonly long downloadLogID;
        private readonly DateTime utcNow;
        private HttpArchiveReplayServer replayServer;

        public BigCommerceDownloadTest(DatabaseFixture db)
        {
            replayServer = new HttpArchiveReplayServer("Platforms.BigCommerce.HttpRequests");
            utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();
            });

            store = Create.Store<BigCommerceStoreEntity>(StoreTypeCode.BigCommerce)
                .Set(x => x.ApiUrl, replayServer.TranslateUrl("https://api.bigcommerce.com/stores/vplh1lw/v2/"))
                .Set(x => x.OauthClientId, "Foo")
                .Set(x => x.OauthToken, "Bar")
                .Set(x => x.BigCommerceAuthentication, BigCommerceAuthenticationType.Oauth)
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
        public void Download_PopulatesStatusCodes()
        {
            using (var webApp = replayServer.Start("Download_PopulatesStatusCodes.har"))
            {
                var storeType = StoreTypeManager.GetType(store);
                var downloader = storeType.CreateDownloader();

                using (DbConnection connection = SqlSession.Current.OpenConnection())
                {
                    downloader.Download(context.Mock.Create<IProgressReporter>(), downloadLogID, connection);
                }

                using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
                {
                    var statuses = storeType.GetOnlineStatusChoices();
                    Assert.Equal(new[] { "Incomplete", "Pending", "Shipped", "Deleted" }, statuses);
                }
            }
        }

        [Fact]
        public void Download_CreatesOrder()
        {
            using (var webApp = replayServer.Start("Download_CreatesOrder.har"))
            {
                var storeType = StoreTypeManager.GetType(store);
                var downloader = storeType.CreateDownloader();

                using (DbConnection connection = SqlSession.Current.OpenConnection())
                {
                    downloader.Download(context.Mock.Create<IProgressReporter>(), downloadLogID, connection);
                }

                using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
                {
                    var orders = sqlAdapter.GetCollectionFromPredicate<OrderEntity>(x => x.Add(OrderFields.StoreID == store.StoreID));
                    var orderManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IOrderManager>();
                    IEnumerable<OrderEntity> fullOrders = orderManager.LoadOrders(
                        orders.OfType<IOrderEntity>().Select(x => x.OrderID),
                        ShipmentsLoader.FullOrderPrefetchPath.Value);

                    var order = fullOrders.OfType<IOrderEntity>().Single();
                    var product = order.OrderItems.OfType<IBigCommerceOrderItemEntity>().Single();

                    Assert.Equal(100, order.OrderNumber);
                    Assert.Equal("1 S Memorial Drive", order.BillStreet1);
                    Assert.Equal("1 S Memorial Drive", order.ShipStreet1);
                    Assert.Equal("Widget A", product.Name);
                }
            }
        }

        [Fact]
        public async Task UpdateOnlineStatus_ToPending_Succeeds()
        {
            using (var webApp = replayServer.Start("BigCommerce_UpdateStoreToPending.har"))
            {
                var storeType = StoreTypeManager.GetType(store);

                var order = Create.Order(store, context.Customer)
                    .Set(x => x.OrderNumber, 100)
                    .Set(x => x.OrderDate, DateTime.UtcNow)
                    .Save(); ;

                var updater = IoC.UnsafeGlobalLifetimeScope.Resolve<IBigCommerceOnlineUpdater>(TypedParameter.From(store));
                updater.UpdateOrderStatus(order.OrderID, BigCommerceConstants.OrderStatusCompleted);
            }
        }

        public void Dispose() => context.Dispose();
    }
}