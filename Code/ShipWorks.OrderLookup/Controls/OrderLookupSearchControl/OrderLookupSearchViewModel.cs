using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// View model for the OrderLookupSearchControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupSearchViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IMessenger messenger;
        private readonly ISecurityContext securityContext;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDisposable subscription;
        private string orderNumber = string.Empty;
        private bool showCreateLabel = false;
        private string searchErrorMessage = string.Empty;
        private bool searchError;
        private bool enabled;
        private bool scanPackTabActive;
        private bool scanPackAllowed;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSearchViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IMessenger messenger,
            ISecurityContext securityContext)
        {
            this.ShipmentModel = shipmentModel;
            this.messenger = messenger;
            this.securityContext = securityContext;

            shipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(Reset);
            CreateLabelCommand = new RelayCommand(CreateLabel);
            shipmentModel.OnSearchOrder += (s, e) => ClearOrderError(OrderClearReason.NewSearch);

            subscription = messenger
                .OfType<OrderLookupClearOrderMessage>()
                .Where(x =>
                    x.Reason == OrderClearReason.Reset ||
                    x.Reason == OrderClearReason.NewSearch ||
                    x.Reason == OrderClearReason.ErrorLoadingOrder)
                .Subscribe(x => ClearOrderError(x.Reason));
        }

        /// <summary>
        /// Clears the order error
        /// </summary>
        private void ClearOrderError(OrderClearReason reason)
        {
            if (reason == OrderClearReason.NewSearch)
            {
                SearchError = true;
                SearchErrorMessage = "Loading Order...";
            }
            else if (reason == OrderClearReason.ErrorLoadingOrder)
            {
                SearchError = true;
                SearchErrorMessage = "There was an error loading the shipment. Please try again.";
            }
            else
            {
                SearchError = false;
                SearchErrorMessage = string.Empty;
            }
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderNumber
        {
            get => orderNumber;
            set => handler.Set(nameof(OrderNumber), ref orderNumber, value);
        }

        /// <summary>
        /// ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Show the create label button?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowCreateLabel
        {
            get => showCreateLabel;
            set => handler.Set(nameof(ShowCreateLabel), ref showCreateLabel, value);
        }

        /// <summary>
        /// Error message to display when a error occurs while searching
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SearchErrorMessage
        {
            get => searchErrorMessage;
            set => handler.Set(nameof(SearchErrorMessage), ref searchErrorMessage, value);
        }

        /// <summary>
        /// Indicates whether or not an error has occurred while searching.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SearchError
        {
            get => searchError;
            set => handler.Set(nameof(SearchError), ref searchError, value);
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
        /// Command to create a label
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Enabled
        {
            get => enabled;
            private set => handler.Set(nameof(Enabled), ref enabled, value);
        }

        /// <summary>
        /// Is ScanPackAllowed
        /// </summary>
        public bool ScanPackAllowed
        {
            get => scanPackAllowed;
            set
            {
                scanPackAllowed = value;
                DetermineEnabled();
            }
        }

        /// <summary>
        /// Is ScanPackTabActive
        /// </summary>
        public bool ScanPackTabActive
        {
            get => scanPackTabActive;
            set
            {
                scanPackTabActive = value;
                DetermineEnabled();
            }
        }

        /// <summary>
        /// Disable if on scanPackAllowed and scanpack is restricted
        /// </summary>
        private void DetermineEnabled()
        {
            Enabled = scanPackAllowed || !ScanPackTabActive;
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
                    SearchErrorMessage = "No matching orders were found.";
                    SearchError = true;
                    OrderNumber = string.Empty;
                }
                else if (ShipmentModel.ShipmentAdapter?.Shipment?.Voided == true)
                {
                    SearchErrorMessage = "This order's shipment has been voided.";
                    SearchError = true;
                    OrderNumber = ShipmentModel.SelectedOrder.OrderNumberComplete;
                }
                else if (ShipmentModel.ShipmentAdapter?.Shipment?.Processed == true)
                {
                    SearchErrorMessage = "This order's shipment has been processed.";
                    SearchError = true;
                    OrderNumber = ShipmentModel.SelectedOrder.OrderNumberComplete;
                }
                else
                {
                    ClearOrderError(OrderClearReason.Reset);
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
            ClearOrderError(OrderClearReason.NewSearch);
            ShipmentModel.TotalCost = 0;
            ShowCreateLabel = false;
            messenger.Send(new OrderLookupSearchMessage(this, OrderNumber));
        }

        /// <summary>
        /// Reset the order
        /// </summary>
        private void Reset()
        {
            ShipmentModel.Unload();
            ClearOrderError(OrderClearReason.Reset);
            OrderNumber = string.Empty;
        }

        /// <summary>
        /// Create a label for the current order
        /// </summary>
        private void CreateLabel() =>
            ShipmentModel.CreateLabel().Forget();

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscription.Dispose();
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}