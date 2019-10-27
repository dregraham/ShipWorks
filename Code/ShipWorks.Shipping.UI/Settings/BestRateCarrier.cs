using System.Collections.ObjectModel;
using System.Reflection;

namespace ShipWorks.Shipping.UI.Settings
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
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
