using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BestRateConsolidatePostalRates
    {
        [Description("No")]
        No = 0,
        [Description("Yes")]
        Yes = 1
    }
}