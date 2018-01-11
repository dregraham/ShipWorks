using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Guarantee type for FedEx freight
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExFreightGuaranteeType
    {
        [Description("None")]
        None,

        [Description("Morning")]
        Morning,

        [Description("Date")]
        Date
    }
}
