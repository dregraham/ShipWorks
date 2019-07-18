﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class ScanPackViewModel : ViewModelBase, IScanPackViewModel
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
        private bool scanningItems = false;
        private string orderNumber;

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
            ResetCommand = new RelayCommand(Reset);

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
        /// Load an order from an order number
        /// </summary>
        public async Task Load(string scannedText)
        {
            if (!scanningItems)
            {
                OrderNumber = scannedText;

                TelemetricResult<long?> orderID = await orderIDRetriever
                    .GetOrderID(scannedText, string.Empty, string.Empty, string.Empty).ConfigureAwait(true);

                ShipmentsLoadedEventArgs loadedOrders = await orderLoader
                    .LoadAsync(new[] {orderID.Value.Value}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite)
                    .ConfigureAwait(true);

                OrderEntity order = loadedOrders.Shipments.FirstOrDefault().Order;

                ItemsToScan.Clear();

                scanPackItemFactory.Create(order).ForEach(ItemsToScan.Add);

                scanningItems = true;

                Update();
            }
        }

        /// <summary>
        /// Get the order with the current order number
        /// </summary>
        private void GetOrder()
        {
            messenger.Send(new OrderLookupSearchMessage(this, OrderNumber));
        }

        /// <summary>
        /// Reset the order
        /// </summary>
        private void Reset()
        {
            OrderNumber = string.Empty;

            ItemsToScan.Clear();
            ScannedItems.Clear();

            scanningItems = false;

            Update();
        }
        
        /// <summary>
        /// Update properties
        /// </summary>
        private void Update()
        {
            int totalItemCount = ItemsToScan.Count + ScannedItems.Count;
            
            // No order scanned yet
            if (totalItemCount == 0)
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
                    ScanFooter = $"{ScannedItems.Count} of {totalItemCount} items have been scanned";
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
