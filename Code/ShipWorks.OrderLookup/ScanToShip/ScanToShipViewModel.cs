using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
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
        private readonly ILicenseService licenseService;
        private readonly IOrderLookupShipmentModel shipmentModel;

        private ScanToShipTab selectedTab;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel,
                                   IScanPackViewModel scanScanPackViewModel,
                                   OrderLookupSearchViewModel searchViewModel,
                                   IUserSession userSession,
                                   ILicenseService licenseService)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            SearchViewModel = searchViewModel;
            this.userSession = userSession;
            this.licenseService = licenseService;

            shipmentModel = orderLookupViewModel.ShipmentModel;
            ScanPackViewModel.OrderVerified += OnOrderVerified;
            shipmentModel.ShipmentLoadedComplete += OnShipmentLoadedComplete;

            SelectedTab = (int) (licenseService.IsHub ? ScanToShipTab.PackTab : ScanToShipTab.ShipTab);
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
        public IOrderLookupSearchViewModel SearchViewModel { get; }

        /// <summary>
        /// IsPackTabActive
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsPackTabActive => selectedTab == ScanToShipTab.PackTab;

        /// <summary>
        /// Returns the numeric value of the selected tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int SelectedTab
        {
            get => (int) selectedTab;
            set
            {
                Set(ref selectedTab, (ScanToShipTab) value);

                SearchViewModel.SelectedTab = (ScanToShipTab) value;
            }
        }

        /// <summary>
        /// When an order has been verified,
        /// </summary>
        private void OnOrderVerified(object sender, EventArgs e)
        {
            // If the selected order is null, that means we are still in the process of loading the order, so don't
            // change tabs yet. It will get picked up by the shipment loaded event.
            if (ShouldAutoAdvance && shipmentModel.SelectedOrder != null && IsPackTabActive)
            {
                SelectedTab = (int) ScanToShipTab.ShipTab;
            }
        }

        /// <summary>
        /// When a shipment is loaded, if we are currently on the pack tab and the order has already been verified,
        /// switch to ship tab.
        /// </summary>
        private void OnShipmentLoadedComplete(object sender, EventArgs e)
        {
            if (ShouldAutoAdvance)
            {
                SelectedTab = (int) (shipmentModel.SelectedOrder.Verified ? ScanToShipTab.ShipTab : ScanToShipTab.PackTab);
            }
        }

        /// <summary>
        /// Does the user have the auto advance setting enabled
        /// </summary>
        private bool ShouldAutoAdvance => licenseService.IsHub && userSession?.Settings?.ScanToShipAutoAdvance == true;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ScanPackViewModel.OrderVerified -= OnOrderVerified;
            shipmentModel.ShipmentLoadedComplete -= OnShipmentLoadedComplete;

            shipmentModel?.Dispose();
            OrderLookupViewModel?.Dispose();
            SearchViewModel?.Dispose();
        }
    }
}
