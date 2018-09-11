using System.ComponentModel;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookupControl
{
    public class OrderLookupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private IOrderLookupDataService dataService;

        public OrderLookupViewModel(IOrderLookupDataService dataService)
        {
            this.dataService = dataService;
            
            OrderLookupSearchViewModel = new OrderLookupSearchViewModel(dataService);
        }

        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }
    }
}
