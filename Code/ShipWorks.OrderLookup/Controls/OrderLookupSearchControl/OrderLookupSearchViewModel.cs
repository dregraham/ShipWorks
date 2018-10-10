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
        /// Total cost of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TotalCost
        {
            get => totalCost;
            set => handler.Set(nameof(TotalCost), ref totalCost, value);
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
                else
                {
                    SearchErrorMessage = string.Empty;
                    SearchError = false;
                    OrderNumber = shipmentModel.SelectedOrder.OrderNumberComplete;
                }
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