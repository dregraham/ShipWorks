using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CreditCardTypeInternal
    {
        [ApiValue("1")]
        [Description("Visa")]
        Visa = 1,

        [ApiValue("2")]
        [Description("MasterCard")]
        MasterCard = 2,

        [ApiValue("3")]
        [Description("AmericanExpress")]
        AmericanExpress = 3,

        [ApiValue("4")]
        [Description("Discover")]
        Discover = 4,
    }
}
