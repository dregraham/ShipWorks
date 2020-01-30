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

        private ScanToShipTab selectedTab;
        private bool isOrderProcessed;
        private bool isOrderVerified;
        private bool showOrderVerificationError;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel,
                                   IScanPackViewModel scanScanPackViewModel,
                                   OrderLookupSearchViewModel searchViewModel,
                                   IUserSession userSession)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            SearchViewModel = searchViewModel;
            this.userSession = userSession;

            shipmentModel = orderLookupViewModel.ShipmentModel;
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

                UpdateOrderVerificationError();
            }
        }

        /// <summary>
        /// Has the current order been verified
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsOrderVerified
        {
            get => isOrderVerified;
            set => Set(ref isOrderVerified, value);
        }

        /// <summary>
        /// Has the current order been processed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsOrderProcessed
        {
            get => isOrderProcessed;
            set => Set(ref isOrderProcessed, value);
        }

        /// <summary>
        /// Whether or not to show the verification error
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowOrderVerificationError
        {
            get => showOrderVerificationError;
            set => Set(ref showOrderVerificationError, value);
        }

        /// <summary>
        /// Show the order verification error when appropriate
        /// </summary>
        public void UpdateOrderVerificationError()
        {
            // If we have an unverified order, we're not on the pack tab, and require validation is on, show the verification error
            if (shipmentModel?.SelectedOrder?.Verified == false &&
                !IsPackTabActive &&
                userSession?.Settings?.RequireVerificationToShip == true)
            {
                ShowOrderVerificationError = true;
            }
            else
            {
                ShowOrderVerificationError = false;
            }
        }

        /// <summary>
        /// Reset the state
        /// </summary>
        public void Reset()
        {
            ShowOrderVerificationError = false;
            IsOrderVerified = false;
            IsOrderProcessed = false;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            shipmentModel?.Dispose();
            OrderLookupViewModel?.Dispose();
            SearchViewModel?.Dispose();
        }
    }
}
