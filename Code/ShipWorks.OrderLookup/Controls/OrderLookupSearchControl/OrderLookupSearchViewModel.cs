using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.OrderLookup.ScanToShip;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// View model for the OrderLookupSearchControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupSearchViewModel : ViewModelBase, IOrderLookupSearchViewModel
    {
        private readonly IMessenger messenger;
        private readonly ISecurityContext securityContext;
        private string orderNumber = string.Empty;
        private bool showCreateLabel = false;
        private string searchMessage = string.Empty;
        private bool showSearchMessage;
        private ScanToShipTab selectedTab;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSearchViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IMessenger messenger,
            ISecurityContext securityContext)
        {
            ShipmentModel = shipmentModel;
            this.messenger = messenger;
            this.securityContext = securityContext;

            shipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            GetOrderCommand = new RelayCommand(GetOrder);
            CreateLabelCommand = new RelayCommand(CreateLabel);
            ResetCommand = new RelayCommand(Reset);
            shipmentModel.OnSearchOrder += (s, e) => ClearSearchMessage(OrderClearReason.NewSearch);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderNumber
        {
            get => orderNumber;
            set => Set(ref orderNumber, value);
        }

        /// <summary>
        /// ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Error message to display when a error occurs while searching
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SearchMessage
        {
            get => searchMessage;
            set => Set(ref searchMessage, value);
        }

        /// <summary>
        /// Indicates whether or not an error has occurred while searching.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowSearchMessage
        {
            get => SelectedTab != ScanToShipTab.PackTab && showSearchMessage;
            set => Set(ref showSearchMessage, value);
        }

        /// <summary>
        /// Show the create label button?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowCreateLabel
        {
            get => SelectedTab != ScanToShipTab.PackTab && showCreateLabel;
            set => Set(ref showCreateLabel, value);
        }

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
        /// Command to create a label
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CreateLabelCommand { get; set; }

        /// <summary>
        /// Currently selected Scan to Ship tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ScanToShipTab SelectedTab
        {
            get => selectedTab;
            set
            {
                if (Set(ref selectedTab, value))
                {
                    // Since these properties depend on the SelectedTab, update them
                    RaisePropertyChanged(nameof(ShowCreateLabel));
                    RaisePropertyChanged(nameof(ShowSearchMessage));
                }
            }
        }

        /// <summary>
        /// Clears the order error
        /// </summary>
        public void ClearSearchMessage(OrderClearReason reason)
        {
            if (reason == OrderClearReason.NewSearch)
            {
                ShowSearchMessage = true;
                SearchMessage = "Loading order...";
            }
            else if (reason == OrderClearReason.ErrorLoadingOrder)
            {
                ShowSearchMessage = true;
                SearchMessage = "There was an error loading the shipment. Please try again.";
            }
            else
            {
                ShowSearchMessage = false;
                SearchMessage = string.Empty;
            }
        }

        /// <summary>
        /// Update when the shipment model changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder))
            {
                if (ShipmentModel.SelectedOrder == null)
                {
                    SearchMessage = "No matching orders were found.";
                    ShowSearchMessage = true;
                    OrderNumber = string.Empty;
                }
                else if (ShipmentModel.ShipmentAdapter?.Shipment?.Voided == true)
                {
                    SearchMessage = "This order's shipment has been voided.";
                    ShowSearchMessage = true;
                    OrderNumber = ShipmentModel.SelectedOrder.OrderNumberComplete;
                }
                else
                {
                    ClearSearchMessage(OrderClearReason.Reset);
                    OrderNumber = ShipmentModel.SelectedOrder.OrderNumberComplete;
                }
            }

            if (e.PropertyName == nameof(ShipmentModel.ShipmentAdapter) ||
                e.PropertyName == nameof(ShipmentModel.SelectedOrder))
            {
                ShowCreateLabel = ShipmentModel?.SelectedOrder != null &&
                                  ShipmentModel?.ShipmentAdapter?.Shipment != null &&
                                  !ShipmentModel.ShipmentAdapter.Shipment.Processed &&
                                  securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, ShipmentModel.SelectedOrder.OrderID);
            }
        }

        /// <summary>
        /// Get the order with the current order number
        /// </summary>
        private void GetOrder()
        {
            ClearSearchMessage(OrderClearReason.NewSearch);
            ShipmentModel.TotalCost = 0;
            ShowCreateLabel = false;
            messenger.Send(new OrderLookupSearchMessage(this, OrderNumber));
        }

        /// <summary>
        /// Create a label for the current order
        /// </summary>
        private void CreateLabel() => ShipmentModel.CreateLabel().Forget();

        /// <summary>
        /// Clear the order in both tabs
        /// </summary>
        private void Reset()
        {
            if (SelectedTab == ScanToShipTab.PackTab)
            {
                TrackedEvent.SendSingleEvent("PickAndPack.ResetClicked");
            }

            ShipmentModel.Unload();
            ClearSearchMessage(OrderClearReason.Reset);
            OrderNumber = string.Empty;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
