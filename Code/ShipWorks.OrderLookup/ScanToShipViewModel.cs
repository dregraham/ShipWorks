using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;

namespace ShipWorks.OrderLookup
{
    [Component(RegistrationType.Self)]
    public class ScanToShipViewModel
    {
        public ScanToShipViewModel(MainOrderLookupViewModel orderLookupViewModel, IScanPackViewModel scanScanPackViewModel, OrderLookupSearchViewModel orderLookupSearchViewModel)
        {
            OrderLookupViewModel = orderLookupViewModel;
            ScanPackViewModel = scanScanPackViewModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
        }

        public MainOrderLookupViewModel OrderLookupViewModel { get; }
        public IScanPackViewModel ScanPackViewModel { get; }
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; }
    }
}
