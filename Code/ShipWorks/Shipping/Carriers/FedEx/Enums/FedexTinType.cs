using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// TIN method
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedexTinType
    {
        [Description("BUSINESS NATIONAL")]
        BUSINESS_NATIONAL,

        [Description("BUSINESS STATE")]
        BUSINESS_STATE,

        [Description("BUSINESS UNION")]
        BUSINESS_UNION,
            
        [Description("PERSONAL NATIONAL")]
        PERSONAL_NATIONAL,
            
        [Description("PERSONAL STATE")]
        PERSONAL_STATE 
    }
}
