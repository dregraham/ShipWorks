using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Rakuten
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Rakuten")]
    public class RakutenDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly RakutenOrdersResponse firstBatch;
        private readonly DbConnection dbConnection;
        private readonly RakutenDownloader testObject;
        private readonly DateTime utcNow;
        private readonly IDownloadEntity downloadLog;
        private readonly Mock<IRakutenWebClient> client;
        private readonly RakutenOrder order;
        private readonly Mock<RakutenShipment> mockRakutenShipment;

        public RakutenDownloaderTest(DatabaseFixture db)
        {
            utcNow = DateTime.UtcNow;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Override<IRakutenWebClient>();
                });

            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();
            mockRakutenShipment = mock.Mock<RakutenShipment>();

            var store = Create.Store<RakutenStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Rakuten Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.Rakuten)
                // Encrypted "refreshToken"
                .Set(x => x.AuthKey = "717TxeCurhOsOo6942NICQ==")
                .Set(x => x.MarketplaceID = "us")
                .Set(x => x.ShopURL = "d-shipworkstest")
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

            downloadLog = Create.Entity<DownloadEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.ComputerID = context.Computer.ComputerID)
                .Set(x => x.UserID = context.User.UserID)
                .Set(x => x.InitiatedBy = (int) DownloadInitiatedBy.User)
                .Set(x => x.Started = utcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int) DownloadResult.Unfinished)
                .Save();

            order = CreateOrderData();
            firstBatch = new RakutenOrdersResponse()
            {
                Orders = new List<RakutenOrder>() { order }
            };

            client = mock.Mock<IRakutenWebClient>();
            client.SetupSequence(c => c.GetOrders(It.IsAny<IRakutenStoreEntity>(), It.IsAny<DateTime>()))
                .Returns(firstBatch)
                .Returns(new RakutenOrdersResponse());

            dbConnection = SqlSession.Current.OpenConnection();
            testObject = mock.Create<RakutenDownloader>(TypedParameter.From<StoreEntity>(store));
        }

        [Fact]
        public async Task Download_SetsProgressDetailWithOrderCount()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLog, dbConnection);
            mockProgressReporter.VerifySet(r => r.Detail = $"Done");
        }

        [Fact]
        public async Task Download_SetsOrderNotes_WhenOrderContainsNotes()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            NoteEntity[] note;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                var meta = new LinqMetaData(adapter);
                note = meta.Note.ToArray();
            }

            Assert.Equal("This is a public note", note[0].Text);
            Assert.Equal("This is a shopper comment", note[1].Text);
        }

        private RakutenOrder CreateOrderData()
        {
            return new RakutenOrder()
            {

                OrderItems = new List<RakutenOrderItem>()
                {
                    CreateOrderItem()
                },
                OrderNumber = "1564164564-16546465465-65454654654",
                OrderStatus = "",
                OrderTotal = 0,
                LastModifiedDate = utcNow.AddDays(-5),
                OrderDate = utcNow.AddDays(-5),
                ShopperComment = "This is a shopper comment",
                MerchantMemo = "This is a public note",
                Shipping = CreateShipment(),
                Payment = CreatePayment(),
                BuyerName = "",
                BuyerPhoneNumber = "",
                AnonymizedEmailAddress = "",
            };
        }

        private RakutenOrderItem CreateOrderItem()
        {
            var item = mock.Create<RakutenOrderItem>();
            item.SKU = "";
            item.BaseSku = "";
            item.Discount = 0;
            item.ItemTotal = 0;
            item.OrderItemID = "";
            item.Quantity = 0;
            item.UnitPrice = 0;
            item.Name = new Dictionary<string, string>() { { "en_us", "en_us" } };
            return item;
        }
        private RakutenPayment CreatePayment()
        {
            var payment = mock.Create<RakutenPayment>();
            payment.OrderPaymentID = "5465465465";
            payment.PaymentStatus = "";
            payment.PayAmount = "";
            payment.PointAmount = "";
            payment.PaymentDate = utcNow.AddDays(-5);
            return payment;
        }
        private RakutenShipment CreateShipment()
        {
            var address = CreateAddress();
            var shipping = mock.Create<RakutenShipment>();
            shipping.DeliveryAddress = address;
            shipping.InvoiceAddress = address;
            shipping.OrderPackageID = "";
            shipping.ShippingMethod = "";
            shipping.ShippingStatus = "";
            shipping.ShippingFee = 0;

            return shipping;
        }
        private RakutenAddress CreateAddress()
        {
            var address = mock.Create<RakutenAddress>();
            address.Name = "Test Fake";
            address.Address1 = "";
            address.Address2 = "";
            address.CityName = "";
            address.StateCode = "";
            address.PostalCode = "";
            address.CountryCode = "";
            address.PhoneNumber = "";

            return address;
        }
        public void Dispose()
        {
            mock?.Dispose();
            context?.Dispose();
            dbConnection?.Dispose();
        }
    }
}