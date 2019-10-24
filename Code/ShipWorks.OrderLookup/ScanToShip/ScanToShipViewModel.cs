using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// ScanToShip ViewModel
    /// </summary>
    public class ScanToShipViewModel : ViewModelBase, IScanToShipViewModel, IDisposable
    {
        private readonly IUserSession userSession;
        private readonly IOrderLookupShipmentModel shipmentModel;
        private const int PackTabIndex = 0;
        private const int ShipTabIndex = 1;
        private int selectedTab;
        private object searchViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel,
                                   IScanPackViewModel scanScanPackViewModel,
                                   OrderLookupSearchViewModel orderLookupSearchViewModel,
                                   IUserSession userSession)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
            this.userSession = userSession;

            shipmentModel = orderLookupViewModel.ShipmentModel;
            ScanPackViewModel.OrderVerified += OrderVerified;
            shipmentModel.ShipmentLoaded += ShipmentLoaded;
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
                else if (value == ShipTabIndex)
                {
                    SearchViewModel = OrderLookupSearchViewModel;
                }
            }
        }

        /// <summary>
        /// When an order has been verified,
        /// </summary>
        private void OrderVerified(object sender, EventArgs e)
        {
            // If the selected order is null, that means we are still in the process of loading the order, so don't
            // change tabs yet. It will get picked up by the shipment loaded event.
            if (userSession?.Settings?.ScanToShipAutoAdvance == true &&
                shipmentModel.SelectedOrder != null && IsPackTabActive)
            {
                SelectedTab = ShipTabIndex;
            }
        }

        /// <summary>
        /// When a shipment is loaded, if we are currently on the pack tab and the order has already been verified,
        /// switch to ship tab.
        /// </summary>
        private void ShipmentLoaded(object sender, EventArgs e)
        {
            if (userSession?.Settings?.ScanToShipAutoAdvance == true &&
                IsPackTabActive && ScanPackViewModel.State == ScanPackState.OrderVerified)
            {
                SelectedTab = ShipTabIndex;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ScanPackViewModel.OrderVerified -= OrderVerified;
            shipmentModel.ShipmentLoaded -= ShipmentLoaded;

            shipmentModel?.Dispose();
            OrderLookupViewModel?.Dispose();
            OrderLookupSearchViewModel?.Dispose();
        }
    }
}
