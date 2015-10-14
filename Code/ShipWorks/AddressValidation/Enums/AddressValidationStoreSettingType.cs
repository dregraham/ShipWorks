using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStoreSettingType
    {
        [Description("Automatically fix addresses")] 
        ValidateAndApply = 0,

        [Description("Notify me of suggestions")] 
        ValidateAndNotify = 1,

        [Description("Manual validation only")] 
        ManualValidationOnly = 2,

        [Description("Disabled")] 
        ValidationDisabled = 3
    }
}
