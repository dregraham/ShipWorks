using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class ScanPackViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ScanPackViewModel testObject;
        private readonly Mock<IOrderLookupOrderIDRetriever> orderIdRetriever;
        private readonly Mock<IScanPackItemFactory> itemFactory;
        private readonly Mock<IVerifiedOrderService> verifiedOrderService;
        private readonly Mock<IOrderLookupAutoPrintService> autoPrintService;

        public ScanPackViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderIdRetriever = mock.Mock<IOrderLookupOrderIDRetriever>();
            itemFactory = mock.Mock<IScanPackItemFactory>();
            verifiedOrderService = mock.Mock<IVerifiedOrderService>();
            autoPrintService = mock.Mock<IOrderLookupAutoPrintService>();

            testObject = mock.Create<ScanPackViewModel>();
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public void ProcessItemScan_DoesNotClearsItemLists_WhenListeningForItems(ScanPackState state)
        {
            SetupOrderNumberNotFound();

            testObject.State = state;
            testObject.ItemsToScan.Add(new ScanPackItem(1, "a", "b", 1, false, null, true, "c", "d", "e", "f"));
            testObject.PackedItems.Add(new ScanPackItem(2, "1", "2", 3, false, null, true, "4", "5", "6", "7"));

            testObject.ProcessItemScan("foo");

            Assert.NotEmpty(testObject.ItemsToScan);
            Assert.NotEmpty(testObject.PackedItems);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public void ProcessScan_MovesItemToPackedItems_WhenScannedTextMatchesItemInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            Assert.True(testObject.PackedItems.Any(x => x.SortIdentifier == 1));
            Assert.True(testObject.ItemsToScan
                            .FirstOrDefault(x => x.SortIdentifier==1).Quantity
                            .IsEquivalentTo(1));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public void ProcessItemScan_SetsError_WhenListeningForItemsAndScannedTextDoesNotMatchItemInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("bad scan");

            Assert.True(testObject.Error);
            Assert.Equal("Last scan did not match. Scan another item to continue.", testObject.ScanHeader);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public void ProcessItemScan_SetsError_WhenScannedTextDoesNotMatchItemInItemsToScanButMatchesItemInPackedItems(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem packedItem = new ScanPackItem(2, "packedName", "packedImage", 2, false, null, true, "packedItemUpc","packedItemCode", "packedProductUpc", "packedSku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);
            testObject.PackedItems.Add(packedItem);

            testObject.ProcessItemScan("packedItemUpc");

            Assert.True(testObject.Error);
            Assert.Equal("Item has already been packed", testObject.ScanHeader);
        }

        [Theory]
        [InlineData(ScanPackState.ListeningForOrderScan)]
        [InlineData(ScanPackState.OrderVerified)]
        public async Task LoadOrder_LoadsOrderItemsIntoItemsToScan_WhenOrderContainsItems(ScanPackState state)
        {
            OrderEntity order = SetupLoadOrder(state);

            await testObject.LoadOrder(order).ConfigureAwait(false);

            Assert.True(testObject.ItemsToScan.Any(x => x.Barcodes.Any(b => b.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase))));
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

        [Fact]
        public void Reset_ClearsItemLists()
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");
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
        public void ProcessItemScan_SetsStateToScanningItems_WhenPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            Assert.Equal(ScanPackState.ScanningItems, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SetsStateToOrderVerified_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 1, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            Assert.Equal(ScanPackState.OrderVerified, testObject.State);
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessScan_SavesVerifiedOrder_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 1, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            verifiedOrderService.Verify(x => x.Save(It.IsAny<OrderEntity>(), true));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessItemScan_DelegatesToAutoPrintService_WhenItemsToScanIsEmptyAndPackedItemsIsNotEmpty(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(3, "name", "image", 1, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            var order = new OrderEntity() { OrderID = 1 };
            order.ChangeOrderNumber("abcd111");
            await testObject.LoadOrder(order);
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            autoPrintService.Verify(a => a.AutoPrintShipment(order.OrderID, order.OrderNumberComplete));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessItemScan_ProcessesItemScan_WhenScannedTextMatchesProductUpcInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("productUpc");

            Assert.True(testObject.PackedItems.Any(x => x.Barcodes.Any(b=>b.Equals("productUpc", StringComparison.InvariantCultureIgnoreCase))));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessItemScan_ProcessesItemScan_WhenScannedTextMatchesItemUpcInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            Assert.True(testObject.PackedItems.Any(x => x.Barcodes.Any(b=>b.Equals("itemUpc", StringComparison.InvariantCultureIgnoreCase))));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public async Task ProcessItemScan_ProcessesItemScan_WhenScannedTextMatchesSkuInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("sku");

            Assert.True(testObject.PackedItems.Any(x => x.Barcodes.Any(b=>b.Equals("sku", StringComparison.InvariantCultureIgnoreCase))));
        }

        [Theory]
        [InlineData(ScanPackState.OrderLoaded)]
        [InlineData(ScanPackState.ScanningItems)]
        public void ProcessItemScan_SetsError_WhenListeningForItemsAndScannedTextDoesNotMatchesProductUpcItemUpcOrSkuInItemsToScan(ScanPackState state)
        {
            ScanPackItem item = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            testObject.State = state;
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("nope");

            Assert.Empty(testObject.PackedItems);
        }

        # region BundleTests

        [Fact]
        public async Task ProcessItemScan_MovesBundleAndItems_WhenBundleIsScanned()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, true, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 2, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);

            testObject.ProcessItemScan("bundleSku");

            Assert.DoesNotContain(bundle, testObject.ItemsToScan);
            Assert.DoesNotContain(bundledItem, testObject.ItemsToScan);
            
            Assert.Contains(bundle, testObject.PackedItems);
            Assert.Contains(bundledItem, testObject.PackedItems);
        }
        
        [Fact]
        public async Task ProcessItemScan_DoesNotMoveBundleAndItems_WhenBundleIsScannedAndBundleIsIncomplete()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 2, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);

            testObject.ProcessItemScan("bundleSku");

            Assert.Contains(bundle, testObject.ItemsToScan);
            Assert.Contains(bundledItem, testObject.ItemsToScan);
            
            Assert.DoesNotContain(bundle, testObject.PackedItems);
            Assert.DoesNotContain(bundledItem, testObject.PackedItems);
        }
        
        [Fact]
        public async Task ProcessItemScan_HasError_WhenBundleIsScannedAndBundleIsIncomplete()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 2, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);

            testObject.ProcessItemScan("bundleSku");
            
            Assert.True(testObject.Error);
            Assert.Equal("Can not scan bundle with items that have been scanned.", testObject.ScanHeader);
        }
        
        [Fact]
        public async Task ProcessItemScan_RemovesBundleAndItemFromItemsToScan_WhenBundleItemIsScannedAndNoItemsLeftToPackForBundle()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 1, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);

            testObject.ProcessItemScan("itemUpc");

            Assert.DoesNotContain(bundle, testObject.ItemsToScan);
            Assert.DoesNotContain(bundledItem, testObject.ItemsToScan);
        }
        
        [Fact]
        public async Task ProcessItemScan_RemovesItemOnlyFromItemsToScan_WhenBundleItemIsScannedAndItemsLeftToPackForBundle()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 1, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem bundledItem2 = new ScanPackItem(3, "Bundled Item2", "image", 1, false, 1, true, "itemUpc2", "itemCode2", "productUpc2", "sku2");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);
            testObject.ItemsToScan.Add(bundledItem2);

            testObject.ProcessItemScan("itemUpc");

            Assert.Contains(bundle, testObject.ItemsToScan);
            Assert.DoesNotContain(bundledItem, testObject.ItemsToScan);
        }
        
        [Fact]
        public async Task ProcessItemScan_AddsBundleAndItemToItemsPacked_WhenFirstItemInBundleIsScanned()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 1, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem bundledItem2 = new ScanPackItem(3, "Bundled Item2", "image", 1, false, 1, true, "itemUpc2", "itemCode2", "productUpc2", "sku2");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);
            testObject.ItemsToScan.Add(bundledItem2);

            testObject.ProcessItemScan("itemUpc");

            Assert.Contains(testObject.PackedItems, x => x.SortIdentifier == 1);
            Assert.Contains(testObject.PackedItems, x => x.SortIdentifier == 2);
        }
        
        [Fact]
        public async Task ProcessItemScan_SetsBundleToComplete_WhenLastItemInBundleIsScanned()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 1, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem bundledItem2 = new ScanPackItem(3, "Bundled Item2", "image", 1, false, 1, true, "itemUpc2", "itemCode2", "productUpc2", "sku2");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);
            testObject.ItemsToScan.Add(bundledItem2);

            testObject.ProcessItemScan("itemUpc");

            var packedBundle = testObject.PackedItems.First(x => x.SortIdentifier == 1);
            Assert.False(packedBundle.IsBundleComplete);
            
            testObject.ProcessItemScan("itemUpc2");
            
            packedBundle = testObject.PackedItems.First(x => x.SortIdentifier == 1);
            Assert.True(packedBundle.IsBundleComplete);
        }
        
        [Fact]
        public async Task ProcessItemScan_MovesNonBundledItemBeforeBundledItem()
        {
            ScanPackItem bundle = new ScanPackItem(1, "Bundle", "image", 1, true, null, false, "bundleUpc", "bundleCode", "bundleSku");
            ScanPackItem bundledItem = new ScanPackItem(2, "Bundled Item", "image", 1, false, 1, true, "itemUpc", "itemCode", "productUpc", "sku");
            ScanPackItem item = new ScanPackItem(3, "Bundled Item", "image", 1, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            await testObject.LoadOrder(new OrderEntity() { OrderID = 4 });
            testObject.ItemsToScan.Add(bundle);
            testObject.ItemsToScan.Add(bundledItem);
            testObject.ItemsToScan.Add(item);

            testObject.ProcessItemScan("itemUpc");

            Assert.Contains(bundle, testObject.ItemsToScan);
            Assert.Contains(bundledItem, testObject.ItemsToScan);
            Assert.DoesNotContain(item, testObject.ItemsToScan);
            
            Assert.DoesNotContain(testObject.PackedItems, x => x.SortIdentifier == 1);
            Assert.DoesNotContain(testObject.PackedItems, x => x.SortIdentifier == 2);
            Assert.Contains(testObject.PackedItems, x => x.SortIdentifier == 3);
        }

        # endregion
        
        private void SetupOrderNumberNotFound()
        {
            orderIdRetriever.Setup(x => x.GetOrderID("foo", string.Empty, string.Empty, string.Empty))
                .ReturnsAsync(new TelemetricResult<long?>("Order"));
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

            ScanPackItem scanPackItem = new ScanPackItem(1, "name", "image", 2, false, null, true, "itemUpc", "itemCode", "productUpc", "sku");

            itemFactory.Setup(x => x.Create(order)).ReturnsAsync(new List<ScanPackItem> {scanPackItem});

            return order;
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
