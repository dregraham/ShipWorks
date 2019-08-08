using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class ScanPackViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ScanPackViewModel testObject;
        private readonly Mock<IOrderLookupOrderIDRetriever> orderIdRetriever;
        private readonly Mock<IOrderLoader> orderLoader;
        private readonly Mock<IScanPackItemFactory> itemFactory;
        private readonly Mock<IMessenger> messenger;
        private readonly Mock<IVerifiedOrderService> verifiedOrderService;
        private readonly Mock<IOrderLookupAutoPrintService> autoPrintService;

        public ScanPackViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderIdRetriever = mock.Mock<IOrderLookupOrderIDRetriever>();
            orderLoader = mock.Mock<IOrderLoader>();
            itemFactory = mock.Mock<IScanPackItemFactory>();
            messenger = mock.Mock<IMessenger>();
            verifiedOrderService = mock.Mock<IVerifiedOrderService>();
            autoPrintService = mock.Mock<IOrderLookupAutoPrintService>();

            testObject = mock.Create<ScanPackViewModel>();
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task ProcessScan_ClearsItemLists_WhenListeningForOrders(ScanPackState state)
        {
            SetupOrderNumberNotFound();

            testObject.State = state;
            testObject.ItemsToScan.Add(new ScanPackItem(1, "a", "b", 1, "c", "d", "e", "f"));
            testObject.PackedItems.Add(new ScanPackItem(2, "1", "2", 3, "4", "5", "6", "7"));

            await testObject.ProcessScan("foo").ConfigureAwait(false);

            Assert.Empty(testObject.ItemsToScan);
            Assert.Empty(testObject.PackedItems);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_DoesNotClearsItemLists_WhenListeningForItems(ScanPackState state)
        {
            SetupOrderNumberNotFound();

            testObject.State = state;
            testObject.ItemsToScan.Add(new ScanPackItem(1, "a", "b", 1, "c", "d", "e", "f"));
            testObject.PackedItems.Add(new ScanPackItem(2, "1", "2", 3, "4", "5", "6", "7"));

            await testObject.ProcessScan("foo").ConfigureAwait(false);

            Assert.NotEmpty(testObject.ItemsToScan);
            Assert.NotEmpty(testObject.PackedItems);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsError_WhenScannedOrderNumberIsNotFound(ScanPackState state)
        {
            SetupOrderNumberNotFound();

            testObject.State = state;

            await testObject.ProcessScan("foo").ConfigureAwait(false);

            Assert.True(testObject.Error);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task ProcessScan_LoadsOrder_WhenScannedOrderNumberIsFound(ScanPackState state)
        {
            SetupOrderNumberFound();

            testObject.State = state;

            await testObject.ProcessScan("foo").ConfigureAwait(false);

            Assert.True(testObject.ItemsToScan.Any(x => x.ItemUpc == "itemUpc"));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_MovesItemToPackedItems_WhenScannedTextMatchesItemInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            Assert.True(testObject.PackedItems.Any(x => x.ItemUpc.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase)));
            Assert.True(testObject.ItemsToScan
                            .FirstOrDefault(x => x.ItemUpc.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase)).Quantity
                            .IsEquivalentTo(1));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsError_WhenListeningForItemsAndScannedTextDoesNotMatchItemInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("bad scan").ConfigureAwait(false);

            Assert.True(testObject.Error);
            Assert.Equal("Last scan did not match. Scan another item to continue.", testObject.ScanHeader);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsError_WhenScannedTextDoesNotMatchItemInItemsToScanButMatchesItemInPackedItems(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem packedItem = new ScanPackItem(2, "packedName", "packedImage", 2, "packedItemUpc","packedItemCode", "packedProductUpc", "packedSku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);
            testObject.PackedItems.Add(packedItem);

            await testObject.ProcessScan("packedItemUpc").ConfigureAwait(false);

            Assert.True(testObject.Error);
            Assert.Equal("Item has already been packed", testObject.ScanHeader);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_SetsOrderNumberToOrdersOrderNumberComplete(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);

            await testObject.LoadOrder(order).ConfigureAwait(false);

            Assert.Equal(order.OrderNumberComplete, testObject.OrderNumber);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_LoadsOrderItemsIntoItemsToScan_WhenOrderContainsItems(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);

            await testObject.LoadOrder(order).ConfigureAwait(false);

            Assert.True(testObject.ItemsToScan.Any(x => x.ItemUpc.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_SetsStateToOrderLoaded_WhenOrderContainsItems(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);

            await testObject.LoadOrder(order).ConfigureAwait(false);

            Assert.Equal(ScanPackState.OrderLoaded, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_DoesNotSetStateToOrderLoaded_WhenOrderDoesNotContainItems(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);
            order.OrderItems.Clear();

            await testObject.LoadOrder(order).ConfigureAwait(false);

            Assert.Equal(state, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_SendsOrderLookupLoadOrderMessage(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);

            await testObject.LoadOrder(order).ConfigureAwait(false);

            messenger.Verify(x => x.Send(It.IsAny<OrderLookupLoadOrderMessage>(), It.IsAny<string>()));
        }

        [Fact]
        public void GetOrderCommand_SendsOrderLookupSearchMessage()
        {
            testObject.GetOrderCommand.Execute(null);

            messenger.Verify(x => x.Send(It.IsAny<OrderLookupSearchMessage>(), It.IsAny<string>()));
        }

        [Fact]
        public void GetOrderCommand_DoesNotResetOrderNumber()
        {
            testObject.OrderNumber = "1";

            testObject.GetOrderCommand.Execute(null);

            Assert.Equal("1", testObject.OrderNumber);
        }

        [Fact]
        public void ResetCommand_SendsOrderLookupClearOrderMessage()
        {
            testObject.ResetCommand.Execute(null);

            messenger.Verify(x => x.Send(It.IsAny<OrderLookupClearOrderMessage>(), It.IsAny<string>()));
        }

        [Fact]
        public void Reset_ClearsOrderNumber()
        {
            testObject.OrderNumber = "1";

            testObject.Reset();

            Assert.Equal(string.Empty, testObject.OrderNumber);
        }

        [Fact]
        public void Reset_ClearsItemLists()
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");
            testObject.ItemsToScan.Add(item);
            testObject.PackedItems.Add(item);

            testObject.Reset();

            Assert.Empty(testObject.ItemsToScan);
            Assert.Empty(testObject.PackedItems);
        }

        [Fact]
        public void Reset_SetsStateToListeningForOrderScan()
        {
            testObject.State = ScanPackState.ScanningItems;

            testObject.Reset();

            Assert.Equal(ScanPackState.ListeningForOrderScan, testObject.State);
        }

        [Fact]
        public void Reset_SetsErrorToFalse()
        {
            testObject.Error = true;

            testObject.Reset();

            Assert.False(testObject.Error);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsStateToScanningItems_WhenPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            Assert.Equal(ScanPackState.ScanningItems, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsStateToOrderVerified_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 1, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            Assert.Equal(ScanPackState.OrderVerified, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SavesVerifiedOrder_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 1, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            verifiedOrderService.Verify(x => x.Save(It.IsAny<OrderEntity>()));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_DelegatesToAutoPrintService_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(3, "name", "image", 1, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            var order = new OrderEntity() { OrderID = 1 };
            order.ChangeOrderNumber("abcd111");
            await testObject.LoadOrder(order);
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            autoPrintService.Verify(a => a.AutoPrintShipment(order.OrderID, order.OrderNumberComplete));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_ProcessesItemScan_WhenScannedTextMatchesProductUpcInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("productUpc").ConfigureAwait(false);

            Assert.True(testObject.PackedItems.Any(x => x.ProductUpc.Equals("productUpc", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_ProcessesItemScan_WhenScannedTextMatchesItemUpcInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("itemUpc").ConfigureAwait(false);

            Assert.True(testObject.PackedItems.Any(x => x.ItemUpc.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_ProcessesItemScan_WhenScannedTextMatchesSkuInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("sku").ConfigureAwait(false);

            Assert.True(testObject.PackedItems.Any(x => x.Sku.Equals("sku", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsError_WhenListeningForItemsAndScannedTextDoesNotMatchesProductUpcItemUpcOrSkuInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            await testObject.ProcessScan("nope").ConfigureAwait(false);

            Assert.Empty(testObject.PackedItems);
        }

        private void SetupOrderNumberNotFound()
        {
            orderIdRetriever.Setup(x => x.GetOrderID("foo", string.Empty, string.Empty, string.Empty))
                .ReturnsAsync(new TelemetricResult<long?>("Order"));
        }

        private void SetupOrderNumberFound()
        {
            var result = new TelemetricResult<long?>("Order");
            result.SetValue(1);

            OrderEntity order = new OrderEntity(1)
            {
                OrderItems =
                {
                    new OrderItemEntity()
                    {
                        UPC = "itemUpc"
                    }
                }
            };

            ShipmentEntity shipment = new ShipmentEntity()
            {
                Order = order
            };
            ShipmentsLoadedEventArgs loadedShipment =
                new ShipmentsLoadedEventArgs(null, false, null, new List<ShipmentEntity> {shipment});

            ScanPackItem scanPackItem = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            orderIdRetriever.Setup(x => x.GetOrderID("foo", string.Empty, string.Empty, string.Empty))
                .ReturnsAsync(result);

            orderLoader.Setup(x => x.LoadAsync(new List<long> {1}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(loadedShipment);

            itemFactory.Setup(x => x.Create(order)).ReturnsAsync(new List<ScanPackItem> {scanPackItem});
        }

        private OrderEntity SetupLoadOrder(ScanPackState state)
        {
            testObject.State = state;

            OrderEntity order = new OrderEntity(1)
            {
                OrderItems =
                {
                    new OrderItemEntity()
                    {
                        UPC = "itemUpc",
                        SKU = "sku"

                    }
                }
            };

            ScanPackItem scanPackItem = new ScanPackItem(1, "name", "image", 2, "itemUpc", "itemCode", "productUpc", "sku");

            itemFactory.Setup(x => x.Create(order)).ReturnsAsync(new List<ScanPackItem> {scanPackItem});

            return order;
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
