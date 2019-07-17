using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.SingleScan.ScanPack
{
    /// <summary>
    /// View model for the ScanPackControl
    /// </summary>
    [Component]
    public class ScanPackViewModel : ViewModelBase, IScanPackViewModel
    {
        private ObservableCollection<ScanPackItem> itemsToScan;
        private ObservableCollection<ScanPackItem> scannedItems;
        private string scanHeader;
        private string scanImageUrl;
        private string scanFooter;

        /// <summary>
        /// Contstructor
        /// </summary>
        public ScanPackViewModel()
        {
        }

        /// <summary>
        /// Items that still need to be scanned
        /// </summary>
        public ObservableCollection<ScanPackItem> ItemsToScan
        {
            get => itemsToScan;
            set => Set(ref itemsToScan, value);
        }

        /// <summary>
        /// Items that have been scanned
        /// </summary>
        public ObservableCollection<ScanPackItem> ScannedItems
        {
            get => scannedItems;
            set => Set(ref scannedItems, value);
        }

        /// <summary>
        /// The total number of items in the current order.
        /// </summary>
        public int TotalItemCount => ItemsToScan.Count + ScannedItems.Count;

        /// <summary>
        /// Header for the scan panel
        /// </summary>
        public string ScanHeader
        {
            get => scanHeader;
            set => Set(ref scanHeader, value);
        }

        /// <summary>
        /// Image for the scan panel
        /// </summary>
        public string ScanImageUrl
        {
            get => scanImageUrl;
            set => Set(ref scanImageUrl, value);
        }

        /// <summary>
        /// Footer for the scan panel
        /// </summary>
        public string ScanFooter
        {
            get => scanFooter;
            set => Set(ref scanFooter, value);
        }

        /// <summary>
        /// Load an order from an order number
        /// </summary>
        public void Load(string orderNumber)
        {

        }

        /// <summary>
        /// Update properties
        /// </summary>
        public void Update()
        {
            // No order scanned yet
            if (TotalItemCount == 0)
            {
                ScanHeader = "Ready to Scan and Pack";
                ScanFooter = "Scan an Order to Begin";
            }
            else
            {
                // Order has been scanned, still items left to scan
                if (ItemsToScan.Any())
                {
                    ScanHeader = "Scan an Item to Pack";
                    ScanFooter = $"{ScannedItems.Count} of {TotalItemCount} items have been scanned";
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
