using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace ShipWorks.OrderLookup.ScanPack.ScanPack
{
    public class ScanPackViewModel : ViewModelBase
    {
        private ObservableCollection<SingleScan.ScanPack.ScanPackItem> itemsToScan;
        private ObservableCollection<SingleScan.ScanPack.ScanPackItem> scannedItems;
        private string scanHeader;
        private string scanImageUrl;
        private string scanFooter;

        public ObservableCollection<SingleScan.ScanPack.ScanPackItem> ItemsToScan
        {
            get => itemsToScan;
            set => Set(ref itemsToScan, value);
        }

        public ObservableCollection<SingleScan.ScanPack.ScanPackItem> ScannedItems
        {
            get => scannedItems;
            set => Set(ref scannedItems, value);
        }

        public int TotalItemCount => ItemsToScan.Count + ScannedItems.Count;

        public string ScanHeader
        {
            get => scanHeader;
            set => Set(ref scanHeader, value);
        }

        public string ScanImageUrl
        {
            get => scanImageUrl;
            set => Set(ref scanImageUrl, value);
        }

        public string ScanFooter
        {
            get => scanFooter;
            set => Set(ref scanFooter, value);
        }

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
