using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    public class OrderLookupSearchViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupDataService dataService;
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly PropertyChangedHandler handler;
        private string orderNumber = string.Empty;
        private string totalCost = string.Empty;
        private string searchErrorMessage = string.Empty;
        private bool searchError = false;

        public OrderLookupSearchViewModel(IOrderLookupDataService dataService)
        {
            this.dataService = dataService;
            dataService.PropertyChanged += UpdateOrder;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            GetOrderCommand = new RelayCommand(GetOrder);
            ResetCommand = new RelayCommand(Reset);
            CreateLabelCommand = new RelayCommand(CreateLabel);
        }

        private void UpdateOrder(object sender, PropertyChangedEventArgs e)
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

        public string OrderNumber
        {
            get => orderNumber;
            set => handler.Set(nameof(OrderNumber), ref orderNumber, value);
        }
        
        public string TotalCost
        {
            get => totalCost;
            set => handler.Set(nameof(TotalCost), ref totalCost, value);
        }

        public string SearchErrorMessage
        {
            get => searchErrorMessage;
            set => handler.Set(nameof(SearchErrorMessage), ref searchErrorMessage, value);
        }

        public bool SearchError
        {
            get => searchError;
            set => handler.Set(nameof(SearchError), ref searchError, value);
        }

        public ICommand GetOrderCommand { get; set; }

        public ICommand ResetCommand { get; set; }

        public ICommand CreateLabelCommand { get; set; }
        
        private void GetOrder()
        {
            throw new System.NotImplementedException();
        }
        
        private void Reset()
        {
            throw new System.NotImplementedException();
        }

        private void CreateLabel()
        {
            throw new System.NotImplementedException();
        }
    }
}