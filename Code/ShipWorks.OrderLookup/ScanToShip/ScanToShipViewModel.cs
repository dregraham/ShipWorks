using System.Reflection;
using GalaSoft.MvvmLight;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// ScanToShip ViewModel
    /// </summary>
    public class ScanToShipViewModel : ViewModelBase, IScanToShipViewModel
    {
        private const int PackTabIndex = 0;
        private const int ShipTabIndex = 1;
        private int selectedTab;
        private object searchViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel, IScanPackViewModel scanScanPackViewModel, OrderLookupSearchViewModel orderLookupSearchViewModel)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
            SelectedTab = 0;
        }

        /// <summary>
        /// OrderLookup ViewModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public MainOrderLookupViewModel OrderLookupViewModel { get; }

        /// <summary>
        /// ScanPack ViewModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IScanPackViewModel ScanPackViewModel { get; }

        /// <summary>
        /// OrderLookupSearch ViewModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; }

        /// <summary>
        /// OrderLookupSearch ViewModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object SearchViewModel
        {
            get => searchViewModel;
            set => Set(ref searchViewModel, value);
        }

        /// <summary>
        /// IsPackTabActive
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsPackTabActive => selectedTab == PackTabIndex;

        /// <summary>
        /// Returns the numeric value of the selected tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                Set(ref selectedTab, value);

                if (value == PackTabIndex)
                {
                    SearchViewModel = ScanPackViewModel;
                }
                else if(value == ShipTabIndex)
                {
                    SearchViewModel = OrderLookupSearchViewModel;
                }
            }
        }
    }
}
