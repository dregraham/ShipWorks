using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Determines how address validation should be performed for a store
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStoreSettingType
    {
        [Description("Automatically fix addresses")]
        [ApiValue("auto")]
        ValidateAndApply = 0,

        [Description("Notify me of suggestions")]
        [ApiValue("notify")]
        ValidateAndNotify = 1,

        [Description("Manual validation only")]
        [ApiValue("manual")]
        ManualValidationOnly = 2,

        [Description("Disabled")]
        [ApiValue("disabled")]
        ValidationDisabled = 3
    }
}
