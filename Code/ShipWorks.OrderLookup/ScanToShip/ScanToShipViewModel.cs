using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;

namespace ShipWorks.OrderLookup.ScanToShip
{
    [Component(RegistrationType.Self, SingleInstance = true)]
    public class ScanToShipViewModel
    {
        private const int PackTabIndex = 0;
        private const int ShipTabIndex = 1;

        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel, IScanPackViewModel scanScanPackViewModel, OrderLookupSearchViewModel orderLookupSearchViewModel)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
        }

        public MainOrderLookupViewModel OrderLookupViewModel { get; }
        public IScanPackViewModel ScanPackViewModel { get; }
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; }

        public bool IsPackTabActive => SelectedTab == PackTabIndex;

        public int SelectedTab { get; set; }
    }
}
