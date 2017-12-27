using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Orders.Split
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderSplitGatewayTest
    {
        private readonly DataContext context;
        private readonly ThreeDCartStoreEntity threeDCartStore;

        public OrderSplitGatewayTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            threeDCartStore = CreateThreeDCartStore();
        }

        [Fact]
        public void CanSplit_ReturnsFalse_WhenProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = true))
                .Save();

            var testObject = context.Mock.Create<OrderSplitGateway>();

            Assert.False(testObject.CanSplit(context.Order.OrderID));
        }

        [Fact]
        public void CanSplit_ReturnsTrue_WhenNoShipments()
        {
            var testObject = context.Mock.Create<OrderSplitGateway>();

            Assert.True(testObject.CanSplit(context.Order.OrderID));
        }

        [Fact]
        public void CanSplit_ReturnsTrue_WhenNotProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = false))
                .Save();

            var testObject = context.Mock.Create<OrderSplitGateway>();

            Assert.True(testObject.CanSplit(context.Order.OrderID));
        }

        [Theory]
        [InlineData("1234-1", 0, "")]
        [InlineData("1234-2", 1, "")]
        [InlineData("1234-4", 3, "")]
        [InlineData("1234", 101, "")]
        [InlineData("1234-1-1", 0, "-1")]
        [InlineData("1234-2-2", 1, "-2")]
        [InlineData("1234-2-3", 2, "-2")]
        public async Task GetNextOrderNumber_ReturnsCorrectValue(string expectedNextOrderNumberComplete, int numberOfExistingOrdersToCreate, string existingOrderPostfix)
        {
            Modify.Order(context.Order)
                .WithOrderNumber(1234, "", existingOrderPostfix)
                .Save();

            string existingOrderNumberComplete = context.Order.OrderNumberComplete;

            for (int i = 1; i <= numberOfExistingOrdersToCreate; i++)
            {
                OrderEntity additionalOrder = Create.Order(context.Store, context.Customer)
                    .WithOrderNumber(1234, "", $"{existingOrderPostfix}-{i}")
                    .Save();

                Modify.Order(additionalOrder)
                    .WithOrderSearch(o => o.Set(os => os.OrderNumber = context.Order.OrderNumber)
                        .Set(os => os.OriginalOrderID = context.Order.OrderID)
                        .Set(os => os.OrderID = context.Order.OrderID)
                        .Set(os => os.StoreID = context.Store.StoreID)
                        .Set(os => os.IsManual = false)
                        .Set(os => os.OrderNumberComplete = additionalOrder.OrderNumberComplete))
                    .Save();
            }

            var testObject = context.Mock.Create<OrderSplitGateway>();

            string calculatedNextOrderNumber = await testObject.GetNextOrderNumber(context.Order.OrderID, existingOrderNumberComplete);

            Assert.Equal(expectedNextOrderNumberComplete, calculatedNextOrderNumber);
        }

        [Fact]
        public async Task LoadOrder_PopulatesOrderFully()
        {
            ThreeDCartOrderEntity expectedOrder = CreateThreeDCartOrder(threeDCartStore, 1, 1, 2, "", false);

            Modify.Shipment(expectedOrder.Shipments[0]).AsPostal(x => x.AsUsps()).Save();

            var testObject = context.Mock.Create<OrderSplitGateway>();
            OrderEntity loadedOrder = await testObject.LoadOrder(expectedOrder.OrderID);

            Assert.Equal(expectedOrder.OrderID, loadedOrder.OrderID);
            Assert.Equal(expectedOrder.OrderNumber, loadedOrder.OrderNumber);
            Assert.Equal(expectedOrder.OrderNumberComplete, loadedOrder.OrderNumberComplete);

            Assert.Equal(expectedOrder.Store.StoreID, loadedOrder.Store.StoreID);

            Assert.Equal(expectedOrder.OrderItems.Count, loadedOrder.OrderItems.Count);
            Assert.Equal(expectedOrder.OrderItems.First().OrderItemAttributes.Count, loadedOrder.OrderItems.First().OrderItemAttributes.Count);

            Assert.Equal(expectedOrder.Shipments.Count, loadedOrder.Shipments.Count);
            Assert.Equal(expectedOrder.Shipments.First().Ups.Packages.Count, loadedOrder.Shipments.First().Ups.Packages.Count);
            Assert.Equal(expectedOrder.Shipments.First().CustomsItems.Count, loadedOrder.Shipments.First().CustomsItems.Count);
            Assert.Equal(expectedOrder.Shipments.First().ValidatedAddress.Count, loadedOrder.Shipments.First().ValidatedAddress.Count);

            Assert.Equal(expectedOrder.Notes.Count, loadedOrder.Notes.Count);
            Assert.Equal(expectedOrder.OrderPaymentDetails.Count, loadedOrder.OrderPaymentDetails.Count);
            Assert.Equal(expectedOrder.OrderSearch.Count, loadedOrder.OrderSearch.Count);
        }

        private ThreeDCartStoreEntity CreateThreeDCartStore()
        {
            return Create.Store<ThreeDCartStoreEntity>(StoreTypeCode.ThreeDCart)
                .Set(x => x.StoreUrl, "https://shipworks.3dcartstores.com")
                .Set(x => x.ApiUserKey, "12288569944166522122885699441665")
                .Set(x => x.TimeZoneID, "Central Standard Time")
                .Set(x => x.StatusCodes, @"<StatusCodes><StatusCode><Code>1</Code><Name>New and Fresh</Name></StatusCode></StatusCodes>")
                .Set(x => x.DownloadModifiedNumberOfDaysBack, 7)
                .Set(x => x.RestUser, true)
                .Save();
        }

        private ThreeDCartOrderEntity CreateThreeDCartOrder(ThreeDCartStoreEntity store, int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            var order = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i =>
                {
                    i.Set(x => x.ThreeDCartShipmentID, item2Root * 100L);
                    i.Set(x => x.Name, "Foo");
                    i.Set(x => x.UnitPrice, 1.8M);
                    i.Set(x => x.Quantity, 3);
                    i.Set(x => x.Weight, 2.5);
                    i.Set(x => x.HarmonizedCode, "HAR123");
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.1");
                        ia.Set(x => x.UnitPrice, 1.23M);
                    });
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.2");
                        ia.Set(x => x.UnitPrice, 2.59M);
                    });
                })
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, item1Root * 100L)
                    .Set(x => x.Quantity, item1Root))
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, item2Root * 100L)
                    .Set(x => x.Quantity, item2Root))
                .WithNote(i => i.Set(x => x.Text, "Foo"))
                .WithPaymentDetail(i => i.Set(x => x.Label, "Foo").Set(x => x.Value = "3"))
                .WithOrderSearch(o => o.Set(os => os.OrderNumber = context.Order.OrderNumber)
                    .Set(os => os.OriginalOrderID = context.Order.OrderID)
                    .Set(os => os.OrderID = context.Order.OrderID)
                    .Set(os => os.StoreID = context.Store.StoreID)
                    .Set(os => os.IsManual = false)
                    .Set(os => os.OrderNumberComplete = context.Order.OrderNumberComplete))
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Set(x => x.ThreeDCartOrderID, orderRoot * 10000)
                .Set(o => o.OnlineStatus, EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.New))
                .Save();

            Create.Shipment(order)
                .AsUps(builder => builder.WithPackage(pBuilder => pBuilder.Set(p =>
                {
                    p.Weight = 10;
                    p.DimsWidth = 10;
                    p.DimsHeight = 10;
                    p.DimsLength = 10;
                })))
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, false)
                .Save();

            return order;
        }
    }
}