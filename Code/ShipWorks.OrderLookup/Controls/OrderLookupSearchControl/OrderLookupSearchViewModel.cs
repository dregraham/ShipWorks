using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// View model for the OrderLookupSearchControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupSearchViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupShipmentModel shipmentModel;
        private readonly IMessenger messenger;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private string orderNumber = string.Empty;
        private bool showCreateLabel = false;
        private string totalCost = string.Empty;
        private string searchErrorMessage = string.Empty;
        private bool searchError;

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderLookupSearchViewModel(IOrderLookupShipmentModel shipmentModel, IMessenger messenger)
        {
            this.shipmentModel = shipmentModel;
            this.messenger = messenger;
            shipmentModel.PropertyChanged += UpdateOrderNumber;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(Reset);
            CreateLabelCommand = new RelayCommand(CreateLabel);
            shipmentModel.OnSearchOrder += (s, e) => ClearOrderError();
        }

        /// <summary>
        /// Clears the order error
        /// </summary>
        private void ClearOrderError() 
        {
            SearchError = false;
            SearchErrorMessage = string.Empty;
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
        public IOrderLookupShipmentModel ShipmentModel => shipmentModel;

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
        /// Indicates whether or not an error has occured while searching.
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
        /// Update the order number when the order changes
        /// </summary>
        private void UpdateOrderNumber(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(shipmentModel.SelectedOrder))
            {
                if (shipmentModel.SelectedOrder == null)
                {
                    SearchErrorMessage = "No matching orders were found.";
                    SearchError = true;
                    OrderNumber = string.Empty;
                }
                else if (shipmentModel.ShipmentAdapter?.Shipment?.Voided == true)
                {
                    SearchErrorMessage = "The order's shipment has been voided.";
                    SearchError = true;
                    OrderNumber = shipmentModel.SelectedOrder.OrderNumberComplete;
                }
                else if (shipmentModel.ShipmentAdapter?.Shipment?.Processed == true)
                {
                    SearchErrorMessage = "The order's shipment has already been processed.";
                    SearchError = true;
                    OrderNumber = shipmentModel.SelectedOrder.OrderNumberComplete;
                }
                else
                {
                    SearchErrorMessage = string.Empty;
                    SearchError = false;
                    OrderNumber = shipmentModel.SelectedOrder.OrderNumberComplete;
                }
            }
            else if (e.PropertyName == nameof(shipmentModel.ShipmentAdapter))
            {
                ShowCreateLabel = shipmentModel.ShipmentAdapter?.Shipment?.Processed == false;
            }
        }

        /// <summary>
        /// Get the order with the current order number
        /// </summary>
        private void GetOrder()
        {
            ClearOrderError();
            messenger.Send(new OrderLookupSearchMessage(this, OrderNumber));
        }

        /// <summary>
        /// Reset the order
        /// </summary>
        private void Reset()
        {
            shipmentModel.Unload();
            SearchErrorMessage = string.Empty;
            SearchError = false;
            OrderNumber = string.Empty;
        }

        /// <summary>
        /// Create a label for the current order
        /// </summary>
        private void CreateLabel()
        {
            throw new System.NotImplementedException();
        }
    }
}