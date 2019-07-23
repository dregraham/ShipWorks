﻿using System.Collections;
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
        private ObservableCollection<ScanPackItem> scannedItems;
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
            ScannedItems = new ObservableCollection<ScanPackItem>();

            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(() => Reset(true));

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
        public ObservableCollection<ScanPackItem> ScannedItems
        {
            get => scannedItems;
            set => Set(ref scannedItems, value);
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

            if (State == ScanPackState.ListeningForOrderScan)
            {
                OrderEntity order = await GetOrder(scannedText).ConfigureAwait(true);

                if (order == null)
                {
                    ScanHeader = "No matching orders were found.";
                    Error = true;
                    ScanFooter = string.Empty;
                    OrderNumber = string.Empty;

                    return;
                }

                Load(order);
            }
			else if(State == ScanPackState.ListeningForItemScan)
            {
                ScanPackItem itemScanned = ItemsToScan.FirstOrDefault(x => x.Sku == scannedText);

                if (itemScanned != null)
                {
                    ProcessItemScan(itemScanned, ItemsToScan, ScannedItems);
                }
            }
        }

        
        /// <summary>
        /// Load the given order
        /// </summary>
        public void Load(OrderEntity order)
        {
            ItemsToScan.Clear();

            scanPackItemFactory.Create(order).ForEach(ItemsToScan.Add);

            State = ScanPackState.ListeningForItemScan;

            Update();
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
            OrderNumber = scannedOrderNumber;
            ScanHeader = "Loading Order...";
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
        /// Reset the order
        /// </summary>
        private void Reset(bool resetOrderNumber)
        {
            if (resetOrderNumber)
            {
                OrderNumber = string.Empty;
            }

            ItemsToScan.Clear();
            ScannedItems.Clear();

            State = ScanPackState.ListeningForOrderScan;

            Update();
        }

        /// <summary>
        /// Check both lists for scanned item and update quantities accordingly
        /// </summary>
        private void ProcessItemScan(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems, ObservableCollection<ScanPackItem> targetItems)
        {
            // Update source list
            if (sourceItem.Quantity > 1)
            {
                sourceItem.Quantity--;
            }
            else
            {
                sourceItems.Remove(sourceItem);
            }

            // Update target list
            ScanPackItem matchingTargetItem = targetItems.FirstOrDefault(x => x.Sku == sourceItem.Sku);
            if (matchingTargetItem == null)
            {
                targetItems.Add(new ScanPackItem(sourceItem.Name, sourceItem.ImageUrl, 1, sourceItem.Sku));
            }
            else
            {
                matchingTargetItem.Quantity++;
            }

            Update();
        }

        /// <summary>
        /// Update properties
        /// </summary>
        private void Update()
        {
            double scannedItemCount = ScannedItems.Select(x => x.Quantity).Sum();
            double totalItemCount = ItemsToScan.Select(x => x.Quantity).Sum() + scannedItemCount;

            // No order scanned yet
            if (totalItemCount.IsEquivalentTo(0))
            {
                ScanHeader = "Ready to Scan and Pack";
                ScanFooter = "Scan an Order Barcode to Begin";
            }
            else
            {
                // Order has been scanned, still items left to scan
                if (ItemsToScan.Any())
                {
                    ScanHeader = "Scan an Item to Pack";
                    ScanFooter = $"{scannedItemCount} of {totalItemCount} items have been scanned";
                }
                else
                {
                    // Order has been scanned, all items have been scanned
                    ScanHeader = "This order has been verified!";
                    ScanFooter = "Scan a new order to continue";
                }
            }
        }
    }
}
