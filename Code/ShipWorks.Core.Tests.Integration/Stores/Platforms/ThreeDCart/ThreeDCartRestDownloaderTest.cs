using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shared.Database.Stores.Platforms.ThreeDCart
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "ThreeDCart")]
    public class ThreeDCartRestDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private long downloadLogID;
        private DbConnection dbConnection;
        private ThreeDCartRestDownloader testObject;
        private readonly ThreeDCartOrder threeDCartOrder;
        private DateTime orderDate = DateTime.Now;

        public ThreeDCartRestDownloaderTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();

            threeDCartOrder = new ThreeDCartOrder()
            {
                ShipmentList = new []
                {
                    new ThreeDCartShipment()
                    {
                        ShipmentCountry = "US",
                        ShipmentState = "MO"
                    }
                },
                QuestionList = new ThreeDCartQuestion[0],
                OrderItemList = new ThreeDCartOrderItem[0],
                OrderStatusID = 1,
                BillingCountry = "US",
                BillingState="MO", 
                OrderDate = orderDate
            };

            var client = mock.CreateMock<IThreeDCartRestWebClient>();
            mock.MockFunc<ThreeDCartStoreEntity, IThreeDCartRestWebClient>(client);
            client.Setup(c => c.GetOrderCount(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(1);
            client.SetupSequence(c => c.GetOrders(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] { threeDCartOrder })
                .Returns(new List<ThreeDCartOrder>());
        }

        private ThreeDCartStoreEntity CreateStore(DateTime? fixDate)
        {
            var store = Create.Store<ThreeDCartStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Channel Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.ThreeDCart)
                .Set(x => x.OrderIDUpgradeFixDate = fixDate)
                .Save();

            Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.StatusTarget = 0)
                .Set(x => x.StatusText = "status")
                .Set(x => x.IsDefault = true)
                .Save();

            Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.StatusTarget = 1)
                .Set(x => x.StatusText = "status")
                .Set(x => x.IsDefault = true)
                .Save();

            StatusPresetManager.CheckForChanges();
            
            downloadLogID = Create.Entity<DownloadEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.ComputerID = context.Computer.ComputerID)
                .Set(x => x.UserID = context.User.UserID)
                .Set(x => x.InitiatedBy = (int) DownloadInitiatedBy.User)
                .Set(x => x.Started = DateTime.UtcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int) DownloadResult.Unfinished)
                .Save().DownloadID;

            StoreManager.CheckForChanges();

            dbConnection = SqlSession.Current.OpenConnection();

            return store;
        }

        [Fact]
        public async Task LoadOrders_CreatesOrderWithPrefixAndNumber_WhenUpgradeDateIsNull()
        {
            var store = CreateStore(null);
            
            threeDCartOrder.InvoiceNumber = 12345;
            threeDCartOrder.InvoiceNumberPrefix = "12";

            testObject = mock.Create<ThreeDCartRestDownloader>(TypedParameter.From(store));
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            ThreeDCartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).ThreeDCartOrder.Single();
            }

            Assert.Equal("1212345", createdOrder.OrderNumberComplete);
        }

        [Fact]
        public async Task LoadOrders_CreatesOrderWithPrefixAndNumber_WhenUpgradeDateIsBeforeOrderDate()
        {
            var store = CreateStore(orderDate.AddDays(-1));

            threeDCartOrder.InvoiceNumber = 12345;
            threeDCartOrder.InvoiceNumberPrefix = "12";

            testObject = mock.Create<ThreeDCartRestDownloader>(TypedParameter.From(store));
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            ThreeDCartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).ThreeDCartOrder.Single();
            }

            Assert.Equal("1212345", createdOrder.OrderNumberComplete);
        }

        [Fact]
        public async Task LoadOrders_CreatesOrderWithPrefixAndNumberMissingPrefix_WhenUpgradeDateIsAfterOrderDate()
        {
            var store = CreateStore(orderDate.AddDays(1));

            threeDCartOrder.InvoiceNumber = 12345;
            threeDCartOrder.InvoiceNumberPrefix = "12";

            testObject = mock.Create<ThreeDCartRestDownloader>(TypedParameter.From(store));
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            ThreeDCartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).ThreeDCartOrder.Single();
            }

            Assert.Equal("12345", createdOrder.OrderNumberComplete);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}