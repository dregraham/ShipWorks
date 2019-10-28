using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// View model for the ScanPackControl
    /// </summary>
    public class ScanPackViewModel : ViewModelBase, IScanPackViewModel, IDropTarget
    {
        public event EventHandler OrderVerified;

        private readonly IOrderLookupOrderIDRetriever orderIDRetriever;
        private readonly IOrderLoader orderLoader;
        private readonly IScanPackItemFactory scanPackItemFactory;
        private readonly IVerifiedOrderService verifiedOrderService;
        private readonly IOrderLookupAutoPrintService orderLookupAutoPrintService;
        private ObservableCollection<ScanPackItem> itemsToScan;
        private ObservableCollection<ScanPackItem> packedItems;
        private string scanHeader;
        private string scanFooter;
        private ScanPackState state;
        private bool error;
        private bool enabled;
        private OrderEntity orderBeingPacked;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackViewModel(
            IOrderLookupOrderIDRetriever orderIDRetriever,
            IOrderLoader orderLoader,
            IScanPackItemFactory scanPackItemFactory,
            IVerifiedOrderService verifiedOrderService,
            IOrderLookupAutoPrintService orderLookupAutoPrintService)
        {
            this.orderIDRetriever = orderIDRetriever;
            this.orderLoader = orderLoader;
            this.scanPackItemFactory = scanPackItemFactory;
            this.verifiedOrderService = verifiedOrderService;
            this.orderLookupAutoPrintService = orderLookupAutoPrintService;

            ItemsToScan = new ObservableCollection<ScanPackItem>();
            PackedItems = new ObservableCollection<ScanPackItem>();

            Update();
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
        /// Learn More
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Uri DisabledLearnMoreUri => new Uri("https://www.shipworks.com/pricing-plans/");

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
        /// Current state of the view model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ScanPackState State
        {
            get => state;
            set
            {
                Set(ref state, value);

                if (value == ScanPackState.OrderVerified)
                {
                    OrderVerified?.Invoke(this, EventArgs.Empty);
                }
            }
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

        /// <summary>
        /// Is Scan Pack enabled
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Enabled
        {
            get => enabled;
            set => Set(ref enabled, value);
        }

        /// <summary>
        /// Can the view accept focus
        /// </summary>
        public Func<bool> CanAcceptFocus { get; set; }

        /// <summary>
        /// Process the scanned text based on the current state
        /// </summary>
        public async Task ProcessScan(string scannedText)
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
                }
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
                    // only play the sound if a user is logged in
                    // this will ensure that the sound does not play during unit tests
                    if (UserSession.IsLoggedOn)
                    {
                        SystemSounds.Asterisk.Play();
                    }

                    ScanPackItem packedItem = GetScanPackItem(scannedText, PackedItems);

                    using (TrackedEvent trackedEvent = new TrackedEvent("PickAndPack.ItemNotFound"))
                    {
                        if (packedItem == null)
                        {
                            trackedEvent.AddProperty("Reason", "NotPacked");
                            ScanHeader = "Last scan did not match. Scan another item to continue.";
                        }
                        else
                        {
                            trackedEvent.AddProperty("Reason", "AlreadyPacked");
                            ScanHeader = "Item has already been packed";
                        }
                    }

                    Error = true;
                }
            }
        }

        /// <summary>
        /// Load the given order
        /// </summary>
        public async Task LoadOrder(OrderEntity order)
        {
            orderBeingPacked = order;
            ItemsToScan.Clear();
            PackedItems.Clear();

            if (orderBeingPacked.OrderItems.Any())
            {
                var items = await scanPackItemFactory.Create(orderBeingPacked).ConfigureAwait(true);
                if (order.Verified)
                {
                    items.ForEach(PackedItems.Add);
                }
                else
                {
                    items.ForEach(ItemsToScan.Add);
                }

                State = ScanPackState.OrderLoaded;

                Update();
            }
            else
            {
                ScanHeader = "This order does not contain any items";
                ScanFooter = "Scan another order to continue";
            }
        }

        /// <summary>
        /// Reset the view model
        /// </summary>
        public void Reset()
        {
            orderBeingPacked = null;
            ItemsToScan.Clear();
            PackedItems.Clear();

            State = ScanPackState.ListeningForOrderScan;

            Error = false;

            Update();
        }

        /// <summary>
        /// Handler for drag over events
        /// </summary>
        public void DragOver(IDropInfo dropInfo)
        {
            ScanPackItem sourceItem = dropInfo.Data as ScanPackItem;
            IEnumerable<ScanPackItem> targetItems = dropInfo.TargetCollection as IEnumerable<ScanPackItem>;

            if (State != ScanPackState.OrderVerified && sourceItem != null && targetItems != null)
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

            // Don't do anything if order is already verified or item is dropped onto the same list
            if (State != ScanPackState.OrderVerified &&
                sourceItem != null &&
                sourceItems != null &&
                targetItems != null &&
                !sourceItems.Equals(targetItems))
            {
                ProcessItemScan(sourceItem, sourceItems, targetItems);
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
            ScanPackItem matchingTargetItem = targetItems.SingleOrDefault(x => x.OrderItemID == sourceItem.OrderItemID);

            if (matchingTargetItem == null)
            {
                targetItems.Add(new ScanPackItem(sourceItem.OrderItemID, sourceItem.Name, sourceItem.ImageUrl, quantityPacked, sourceItem.ItemUpc, sourceItem.ItemCode, sourceItem.ProductUpc, sourceItem.Sku));
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
                    ScanFooter = $"{scannedItemCount} of {totalItemCount} items have been scanned";

                    if (PackedItems.Any())
                    {
                        ScanHeader = "Verified! Scan another item to continue.";
                        State = ScanPackState.ScanningItems;
                    }
                    else
                    {
                        ScanHeader = "Scan an item to pack";
                        State = ScanPackState.OrderLoaded;
                    }
                }
                else
                {
                    verifiedOrderService.Save(orderBeingPacked);

                    orderLookupAutoPrintService.AutoPrintShipment(orderBeingPacked.OrderID, orderBeingPacked.OrderNumberComplete);

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
            listToSearch.FirstOrDefault(x => x.ProductUpc?.Equals(scannedText, StringComparison.InvariantCultureIgnoreCase) ?? false) ??
            listToSearch.FirstOrDefault(x => x.ItemUpc?.Equals(scannedText, StringComparison.InvariantCultureIgnoreCase) ?? false) ??
            listToSearch.FirstOrDefault(x => x.Sku?.Equals(scannedText, StringComparison.InvariantCultureIgnoreCase) ?? false) ??
            listToSearch.FirstOrDefault(x => x.ItemCode?.Equals(scannedText, StringComparison.InvariantCulture) ?? false);
    }
}
