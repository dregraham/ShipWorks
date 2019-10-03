using System.Collections.ObjectModel;

namespace ShipWorks.Shipping.UI.Settings
{
    public class BestRateCarrier
    {
        public BestRateCarrier()
        {
            Accounts = new ObservableCollection<BestRateAccount>();
        }

        public string Name { get; set; }

        public ObservableCollection<BestRateAccount> Accounts { get; set; }
    }
}
