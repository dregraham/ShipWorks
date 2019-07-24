using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// View model for the ScanPackControl
    /// </summary>
    [Component(SingleInstance = true)]
    public class ScanPackViewModel : ViewModelBase, IScanPackViewModel, IDropTarget
    {
        private readonly IOrderLookupOrderIDRetriever orderIDRetriever;
        private readonly IOrderLoader orderLoader;
        private readonly IScanPackItemFactory scanPackItemFactory;
        private readonly IMessenger messenger;
        private ObservableCollection<ScanPackItem> itemsToScan;
        private ObservableCollection<ScanPackItem> packedItems;
        private string scanHeader;
        private string scanImageUrl;
        private string scanFooter;
        private string orderNumber;
        private ScanPackState state;
        private bool error;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackViewModel(IOrderLookupOrderIDRetriever orderIDRetriever, IOrderLoader orderLoader,
                                 IScanPackItemFactory scanPackItemFactory, IMessenger messenger)
        {
            this.orderIDRetriever = orderIDRetriever;
            this.orderLoader = orderLoader;
            this.scanPackItemFactory = scanPackItemFactory;
            this.messenger = messenger;

            ItemsToScan = new ObservableCollection<ScanPackItem>();
            PackedItems = new ObservableCollection<ScanPackItem>();

            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(ResetClicked);

            Update();
        }

        /// <summary>
        /// Order number for order being packed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderNumber
        {
            get => orderNumber;
            set => Set(ref orderNumber, value);
        }

        /// <summary>
        /// Items that still need to be scanned
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ScanPackItem> ItemsToScan
        {
            get => itemsToScan;
            set => Set(ref itemsToScan, value);
        }

        /// <summary>
        /// Items that have been scanned
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ScanPackItem> PackedItems
        {
            get => packedItems;
            set => Set(ref packedItems, value);
        }

        /// <summary>
        /// Header for the scan panel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ScanHeader
        {
            get => scanHeader;
            set => Set(ref scanHeader, value);
        }

        /// <summary>
        /// Image for the scan panel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ScanImageUrl
        {
            get => scanImageUrl;
            set => Set(ref scanImageUrl, value);
        }

        /// <summary>
        /// Footer for the scan panel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ScanFooter
        {
            get => scanFooter;
            set => Set(ref scanFooter, value);
        }

        /// <summary>
        /// Footer for the scan panel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ScanPackState State
        {
            get => state;
            set => Set(ref state, value);
        }

        /// <summary>
        /// Whether or not an error occured during the Scan and Pack process
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Error
        {
            get => error;
            set => Set(ref error, value);
        }

        #region SearchControl Properties

        /// <summary>
        /// Don't show the Create Label button in the search control
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowCreateLabel => false;

        /// <summary>
        /// Command for getting orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetOrderCommand { get; set; }

        /// <summary>
        /// Command for resetting the order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// False so that search control doesn't leave a space for error message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SearchError => false;

        #endregion

        /// <summary>
        /// Load an order from an order number
        /// </summary>
        public async Task Load(string scannedText)
        {
            Error = false;

            if (State == ScanPackState.ListeningForOrderScan || State == ScanPackState.OrderVerified)
            {
                ItemsToScan.Clear();
                PackedItems.Clear();

                OrderEntity order = await GetOrder(scannedText).ConfigureAwait(true);

                if (order == null)
                {
                    ScanHeader = "No matching orders were found.";
                    Error = true;
                    ScanFooter = string.Empty;
                    OrderNumber = string.Empty;

                    return;
                }

                await Load(order).ConfigureAwait(true);
            }
			else if(State == ScanPackState.OrderLoaded || State == ScanPackState.ScanningItems)
            {
                ScanPackItem itemScanned = GetScanPackItem(scannedText, ItemsToScan);

                if (itemScanned != null)
                {
                    ProcessItemScan(itemScanned, ItemsToScan, PackedItems);
                }
                else
                {
                    ScanPackItem packedItem = GetScanPackItem(scannedText, PackedItems);

                    ScanHeader = packedItem == null ?
                        "Last scan did not match. Scan another item to continue." :
                        "Item has already been packed";

                    Error = true;
                }
            }
        }
        
        /// <summary>
        /// Load the given order
        /// </summary>
        public async Task Load(OrderEntity order)
        {
            ItemsToScan.Clear();
            OrderNumber = order.OrderNumberComplete;

            if (order.OrderItems.Any())
            {
                (await scanPackItemFactory.Create(order).ConfigureAwait(true)).ForEach(ItemsToScan.Add);

                State = ScanPackState.OrderLoaded;

                Update();
            }
            else
            {
                ScanHeader = "This order does not contain any items";
                ScanFooter = "Scan another order to continue";
            }

            messenger.Send(new OrderLookupLoadOrderMessage(this, order));
        }

        /// <summary>
        /// Handler for drag over events
        /// </summary>
        public void DragOver(IDropInfo dropInfo)
        {
            ScanPackItem sourceItem = dropInfo.Data as ScanPackItem;
            IEnumerable<ScanPackItem> targetItems = dropInfo.TargetCollection as IEnumerable<ScanPackItem>;

            if (sourceItem != null && targetItems != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Handler for drop events
        /// </summary>
        public void Drop(IDropInfo dropInfo)
        {
            Error = false;

            ScanPackItem sourceItem = dropInfo.Data as ScanPackItem;
            ObservableCollection<ScanPackItem> sourceItems = dropInfo.DragInfo.SourceCollection as ObservableCollection<ScanPackItem>;
            ObservableCollection<ScanPackItem> targetItems = dropInfo.TargetCollection as ObservableCollection<ScanPackItem>;

            if (sourceItem != null && sourceItems != null && targetItems != null)
            {
                // Don't do anything if dropped onto the same list
                if (!sourceItems.Equals(targetItems))
                {
                    ProcessItemScan(sourceItem, sourceItems, targetItems);
                }
            }
        }

        /// <summary>
        /// Get an order with an order number matching the scanned text
        /// </summary>
        private async Task<OrderEntity> GetOrder(string scannedOrderNumber)
        {
            ScanHeader = "Loading order...";
            ScanFooter = string.Empty;

            TelemetricResult<long?> orderID = await orderIDRetriever
                .GetOrderID(scannedOrderNumber, string.Empty, string.Empty, string.Empty).ConfigureAwait(true);

            OrderEntity order = null;
            if (orderID.Value.HasValue)
            {
                ShipmentsLoadedEventArgs loadedOrders = await orderLoader
                    .LoadAsync(new[] {orderID.Value.Value}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite)
                    .ConfigureAwait(true);

                order = loadedOrders?.Shipments?.FirstOrDefault()?.Order;
            }

            return order;
        }

        /// <summary>
        /// Get the order with the current order number
        /// </summary>
        private void GetOrder()
        {
            Reset(false);

            messenger.Send(new OrderLookupSearchMessage(this, OrderNumber));
        }

        /// <summary>
        /// Clear the order as the user clicked reset
        /// </summary>
        private void ResetClicked()
        {
            messenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.Reset));
        }

        /// <summary>
        /// Reset the view model
        /// </summary>
        public void Reset() => Reset(true);

        /// <summary>
        /// Reset the order
        /// </summary>
        private void Reset(bool resetOrderNumber)
        {
            if (resetOrderNumber)
            {
                OrderNumber = string.Empty;
            }

            ItemsToScan.Clear();
            PackedItems.Clear();

            State = ScanPackState.ListeningForOrderScan;

            Error = false;

            Update();
        }

        /// <summary>
        /// Check both lists for scanned item and update quantities accordingly
        /// </summary>
        private void ProcessItemScan(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems, ObservableCollection<ScanPackItem> targetItems)
        {
            // If someone has a product of qty of 1.3, the first scan will move 1 to packed and leave .3, the second scan qill pack .3
            double quantityPacked;
            // Update source list
            if (sourceItem.Quantity > 1)
            {
                sourceItem.Quantity--;
                quantityPacked = 1;
            }
            else
            {
                quantityPacked = sourceItem.Quantity;
                sourceItems.Remove(sourceItem);
            }

            // Update target list
            ScanPackItem matchingTargetItem = targetItems.FirstOrDefault(x => x.Upc == sourceItem.Upc && x.Sku == sourceItem.Sku);
            if (matchingTargetItem == null)
            {
                targetItems.Add(new ScanPackItem(sourceItem.Name, sourceItem.ImageUrl, quantityPacked, sourceItem.Upc, sourceItem.Sku));
            }
            else
            {
                matchingTargetItem.Quantity += quantityPacked;
            }

            Update();
        }

        /// <summary>
        /// Update properties
        /// </summary>
        private void Update()
        {
            double scannedItemCount = PackedItems.Select(GetUnitCount).Sum();
            double totalItemCount = ItemsToScan.Select(GetUnitCount).Sum() + scannedItemCount;

            // No order scanned yet
            if (totalItemCount.IsEquivalentTo(0))
            {
                ScanHeader = "Ready to scan and pack";
                ScanFooter = "Scan an order barcode to begin";
            }
            else
            {
                // Order has been scanned, still items left to scan
                if (ItemsToScan.Any())
                {
                    ScanHeader = "Scan an item to pack";
                    ScanFooter = $"{scannedItemCount} of {totalItemCount} items have been scanned";

                    State = PackedItems.Any() ? ScanPackState.ScanningItems : ScanPackState.OrderLoaded;
                }
                else
                {
                    // Order has been scanned, all items have been scanned
                    ScanHeader = "This order has been verified!";
                    ScanFooter = "Scan a new order to continue";

                    State = ScanPackState.OrderVerified;
                }
            }
        }

        /// <summary>
        /// Given a value less than 1, return 1. Given a decimal number, round up.
        /// </summary>
        private static double GetUnitCount(ScanPackItem x)
        {
            return Math.Ceiling(Math.Max(x.Quantity, 1));
        }

        /// <summary>
        /// Search the items in the given list for a upc matching the scanned text first, if none found, search the items
        /// again for a sku matching the scanned text
        /// </summary>
        private ScanPackItem GetScanPackItem(string scannedText, ObservableCollection<ScanPackItem> listToSearch) =>
            listToSearch.FirstOrDefault(x => x.Upc == scannedText) ??
            listToSearch.FirstOrDefault(x => x.Sku == scannedText);
    }
}
