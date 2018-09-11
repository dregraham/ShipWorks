using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// View model for the OrderLookupSearchControl
    /// </summary>
    public class OrderLookupSearchViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupDataService dataService;
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly PropertyChangedHandler handler;
        private string orderNumber = string.Empty;
        private string totalCost = string.Empty;
        private string searchErrorMessage = string.Empty;
        private bool searchError;

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderLookupSearchViewModel(IOrderLookupDataService dataService)
        {
            this.dataService = dataService;
            dataService.PropertyChanged += UpdateOrderNumber;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(Reset);
            CreateLabelCommand = new RelayCommand(CreateLabel);
        }

        /// <summary>
        /// Update the order number when the order changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateOrderNumber(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order")
            {
                if (dataService.Order == null)
                {
                    SearchErrorMessage = "No matching orders were found";
                    SearchError = true;
                    OrderNumber = string.Empty;
                }
                else
                {
                    SearchErrorMessage = string.Empty;
                    SearchError = false;
                    OrderNumber = dataService.Order.OrderNumberComplete;
                }
            }
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        public string OrderNumber
        {
            get => orderNumber;
            set => handler.Set(nameof(OrderNumber), ref orderNumber, value);
        }
        
        /// <summary>
        /// Total cost of the shipment
        /// </summary>
        public string TotalCost
        {
            get => totalCost;
            set => handler.Set(nameof(TotalCost), ref totalCost, value);
        }

        /// <summary>
        /// Error message to display when a error occurs while searching
        /// </summary>
        public string SearchErrorMessage
        {
            get => searchErrorMessage;
            set => handler.Set(nameof(SearchErrorMessage), ref searchErrorMessage, value);
        }

        /// <summary>
        /// Indicates whether or not an error has occured while searching.
        /// </summary>
        public bool SearchError
        {
            get => searchError;
            set => handler.Set(nameof(SearchError), ref searchError, value);
        }

        /// <summary>
        /// Command for getting orders
        /// </summary>
        public ICommand GetOrderCommand { get; set; }

        /// <summary>
        /// Command for resetting the order
        /// </summary>
        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// Command to create a label
        /// </summary>
        public ICommand CreateLabelCommand { get; set; }
        
        /// <summary>
        /// Get the order with the current order number 
        /// </summary>
        private void GetOrder()
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Reset the order
        /// </summary>
        private void Reset()
        {
            throw new System.NotImplementedException();
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