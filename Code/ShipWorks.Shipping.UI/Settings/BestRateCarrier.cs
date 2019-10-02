using System.Collections.ObjectModel;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.UI.Settings
{
    public class BestRateCarrier
    {
        public string Name { get; set; }

        public ShipmentTypeCode ShipmentType { get; set; }

        public ObservableCollection<BestRateAccount> Accounts { get; set; }

        public bool IsActive { get; set; }
    }
}
