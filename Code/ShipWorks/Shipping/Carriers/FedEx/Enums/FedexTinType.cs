using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// TIN method
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedexTINType
    {
        [Description("")]
        DEFAULT = -1,

        [Description("Business National")]
        BUSINESS_NATIONAL = 0,

        [Description("Business State")]
        BUSINESS_STATE = 1,

        [Description("Business Union")]
        BUSINESS_UNION = 2,
          
        [Description("Personal National")]
        PERSONAL_NATIONAL = 3,
        
        [Description("Personal State")]
        PERSONAL_STATE = 4
    }
}
