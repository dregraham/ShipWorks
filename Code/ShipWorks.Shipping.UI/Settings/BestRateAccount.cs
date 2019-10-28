using System.Reflection;

namespace ShipWorks.Shipping.UI.Settings
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class BestRateAccount
    {
        public long AccountID { get; set; }

        public string AccountDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
