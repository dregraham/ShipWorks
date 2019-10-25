using System;
using System.Reflection;
using System.Windows.Input;
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
                                   IUserSession userSession,
                                   ILicenseService licenseService)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
            this.userSession = userSession;
            this.licenseService = licenseService;

            shipmentModel = orderLookupViewModel.ShipmentModel;
            ScanPackViewModel.OrderVerified += OnOrderVerified;
            shipmentModel.ShipmentLoadedComplete += OnShipmentLoadedComplete;

            OrderLookupSearchViewModel.ResetCommand = new RelayCommand(Reset);
            ScanPackViewModel.ResetCommand = new RelayCommand(Reset);

            SelectedTab = IsWarehouseCustomer ? PackTabIndex : ShipTabIndex;
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
        /// Clear the order in both tabs
        /// </summary>
        private void Reset()
        {
            if (IsPackTabActive)
            {
                new TrackedEvent("PickAndPack.ResetClicked").Dispose();
            }

            OrderLookupSearchViewModel.ShipmentModel.Unload();
            OrderLookupSearchViewModel.ClearOrderError(OrderClearReason.Reset);
            OrderLookupSearchViewModel.OrderNumber = string.Empty;
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
                SelectedTab = ShipTabIndex;
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
                SelectedTab = shipmentModel.SelectedOrder.Verified? ShipTabIndex : PackTabIndex;
            }
        }

        /// <summary>
        /// Is the customer allowed to use warehouse features
        /// </summary>
        private bool IsWarehouseCustomer => licenseService.CheckRestriction(EditionFeature.Warehouse, null) ==
                                            EditionRestrictionLevel.None;

        /// <summary>
        /// Does the user have the auto advance setting enabled
        /// </summary>
        private bool ShouldAutoAdvance => IsWarehouseCustomer && userSession?.Settings?.ScanToShipAutoAdvance == true;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ScanPackViewModel.OrderVerified -= OnOrderVerified;
            shipmentModel.ShipmentLoadedComplete -= OnShipmentLoadedComplete;

            shipmentModel?.Dispose();
            OrderLookupViewModel?.Dispose();
            OrderLookupSearchViewModel?.Dispose();
        }
    }
}
