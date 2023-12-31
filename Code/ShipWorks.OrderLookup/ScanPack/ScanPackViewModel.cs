﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// View model for the ScanPackControl
    /// </summary>
    public class ScanPackViewModel : ViewModelBase, IScanPackViewModel, IDropTarget
    {
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
            IScanPackItemFactory scanPackItemFactory,
            IVerifiedOrderService verifiedOrderService,
            IOrderLookupAutoPrintService orderLookupAutoPrintService)
        {
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
        /// Process a scan for verifying a line item
        /// </summary>
        public void ProcessItemScan(string scannedText)
        {
            ScanPackItem itemScanned = GetScanPackItem(scannedText, ItemsToScan);

            if (itemScanned != null)
            {
                ProcessItemScan(itemScanned, ItemsToScan, PackedItems);
            }
            else
            {
                ScanPackItem packedItem = GetScanPackItem(scannedText, PackedItems);

                using (TrackedEvent trackedEvent = new TrackedEvent("PickAndPack.ItemNotFound"))
                {
                    if (packedItem == null)
                    {
                        trackedEvent.AddProperty("Reason", "NotPacked");
                        HandleError("Last scan did not match. Scan another item to continue.");
                    }
                    else
                    {
                        trackedEvent.AddProperty("Reason", "AlreadyPacked");
                        HandleError("Item has already been packed");
                    }
                }
            }
        }

        /// <summary>
        /// Load the given order
        /// </summary>
        public async Task LoadOrder(OrderEntity order)
        {
            Error = false;
            
            orderBeingPacked = order;
            ItemsToScan.Clear();
            PackedItems.Clear();
            if (order == null)
            {
                HandleError("No matching orders were found.");
                ScanFooter = string.Empty;
            }
            else if (orderBeingPacked.OrderItems.Any())
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
            if (State != ScanPackState.OrderVerified && dropInfo.Data is ScanPackItem &&
                dropInfo.TargetCollection is IEnumerable<ScanPackItem>)
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
            // Don't do anything if order is already verified or item is dropped onto the same list
            if (State != ScanPackState.OrderVerified &&
                dropInfo.Data is ScanPackItem sourceItem &&
                dropInfo.DragInfo.SourceCollection is ObservableCollection<ScanPackItem> sourceItems &&
                dropInfo.TargetCollection is ObservableCollection<ScanPackItem> targetItems &&
                !sourceItems.Equals(targetItems))
            {
                ProcessItemScan(sourceItem, sourceItems, targetItems);
            }
        }

        /// <summary>
        /// Check both lists for scanned item and update quantities accordingly
        /// </summary>
        private void ProcessItemScan(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems,
            ObservableCollection<ScanPackItem> targetItems)
        {
            var result = Result.FromSuccess();

            if (sourceItem.IsBundle)
            {
                result = ProcessBundleScan(sourceItem, sourceItems, targetItems);
            }
            else
            {
                UpdateItemInCollections(sourceItem, sourceItems, targetItems);
                
                // Is part of bundle
                if (sourceItem.ParentSortIdentifier.HasValue)
                {
                    UpdateBundleInCollections(sourceItem, sourceItems, targetItems);
                }
            }

            if (result.Success)
            {
                Update(true);
                Error = false;

                ItemsToScan = new ObservableCollection<ScanPackItem>(ItemsToScan.OrderBy(x => x.SortIdentifier));
                PackedItems = new ObservableCollection<ScanPackItem>(PackedItems.OrderBy(x => x.SortIdentifier));
            }
            else
            {
                HandleError(result.Message);
            }
        }

        /// <summary>
        /// Update each collection for a scanned item
        /// </summary>
        private void UpdateItemInCollections(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems,
            ObservableCollection<ScanPackItem> targetItems)
        {
            // If someone has a product of qty of 1.3, the first scan will move 1 to packed and leave .3, the second scan will pack .3
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
            ScanPackItem matchingTargetItem =
                targetItems.SingleOrDefault(x => x.SortIdentifier == sourceItem.SortIdentifier);

            if (matchingTargetItem == null)
            {
                ScanPackItem targetItem = sourceItem.Copy();
                targetItem.Quantity = quantityPacked;
                targetItems.Add(targetItem);
            }
            else
            {
                matchingTargetItem.Quantity += quantityPacked;
            }
        }

        /// <summary>
        /// Updates the bundle in each collection as needed when an item in the bundle is scanned
        /// </summary>
        private void UpdateBundleInCollections(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems,
            ObservableCollection<ScanPackItem> targetItems)
        {
            var bundle =
                sourceItems.First(x => x.SortIdentifier == sourceItem.ParentSortIdentifier);

            // Check if there are any other items left to pack in the bundle,
            // if not, remove the bundle from the source
            if (sourceItems.Any(x => x.ParentSortIdentifier == sourceItem.ParentSortIdentifier))
            {
                bundle.IsBundleComplete = false;
            }
            else
            {
                bundle.IsBundleComplete = true;
                sourceItems.Remove(bundle);
            }

            if (!targetItems.Contains(bundle))
            {
                targetItems.Add(bundle);
            }
        }

        /// <summary>
        /// Processes the scan for a bundle 
        /// </summary>
        private Result ProcessBundleScan(ScanPackItem sourceItem, ObservableCollection<ScanPackItem> sourceItems,
            ObservableCollection<ScanPackItem> targetItems)
        {
            if (!sourceItem.IsBundleComplete)
            {
                return Result.FromError("Cannot scan incomplete bundles");
            }

            var bundle = sourceItems.Where(x =>
                x.SortIdentifier == sourceItem.SortIdentifier ||
                x.ParentSortIdentifier == sourceItem.SortIdentifier).ToList();

            foreach (var item in bundle)
            {
                sourceItems.Remove(item);
                targetItems.Add(item);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Update properties
        /// </summary>
        private void Update(bool itemScanned = false)
        {
            double scannedItemCount = PackedItems.Where(x => !x.IsBundle).Select(GetUnitCount).Sum();
            double totalItemCount = ItemsToScan.Where(x => !x.IsBundle).Select(GetUnitCount).Sum() + scannedItemCount;

            // No order scanned yet
            if (totalItemCount.IsEquivalentTo(0))
            {
                ScanHeader = "Scan an order barcode to begin";
                ScanFooter = string.Empty;
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
                    if (itemScanned)
                    {
                        verifiedOrderService.Save(orderBeingPacked, true);
                        orderLookupAutoPrintService.AutoPrintShipment(orderBeingPacked.OrderID,
                            orderBeingPacked.OrderNumberComplete);
                    }

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
        /// <remarks>
        /// Priority order is
        /// 1. individual items
        /// 2. bundles not in progress
        /// 2. bundles in progress
        /// 3. items in bundles
        /// </remarks>
        private ScanPackItem GetScanPackItem(string scannedText, ObservableCollection<ScanPackItem> listToSearch)
        {
            var matches = listToSearch.Where(x => x.IsMatch(scannedText)).ToList();

            // Try getting item that is not in a bundle first
            var nonBundledItems = matches.Where(x => x.ParentSortIdentifier == null).ToList();
            if (nonBundledItems.Any())
            {
                var item = nonBundledItems.FirstOrDefault(x => !x.IsBundle);
                if (item != null)
                {
                    // individual item
                    return item;
                }

                // bundle
                return nonBundledItems.FirstOrDefault(x => x.IsBundleComplete) ?? nonBundledItems.FirstOrDefault();
            }

            // bundled item
            return matches.FirstOrDefault();
        }

        /// <summary>
        /// Handle the given error
        /// </summary>
        private void HandleError(string errorMessage)
        {
            // only play the sound if a user is logged in
            // this will ensure that the sound does not play during unit tests
            if (UserSession.IsLoggedOn)
            {
                SystemSounds.Asterisk.Play();
            }

            ScanHeader = errorMessage;

            Error = true;
        }
    }
}
