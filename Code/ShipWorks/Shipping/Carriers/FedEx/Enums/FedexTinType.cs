using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// TIN method
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public enum FedexTINType
    {
        [Description("Business National")]
        BUSINESS_NATIONAL,

        [Description("Business State")]
        BUSINESS_STATE,

        [Description("Business Union")]
        BUSINESS_UNION,
          
        [Description("Personal National")]
        PERSONAL_NATIONAL,
        
        [Description("Personal State")]
        PERSONAL_STATE
    }
}
