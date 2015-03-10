using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.AddressValidation
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStoreSettingType
    {
        [Description("Automatically fix addresses")] 
        ValidateAndApply = 0,

        [Description("Notify me of suggestions")] 
        ValidateAndNotify = 1,

        [Description("Manual Validation Only")] 
        ManualValidationOnly = 2,

        [Description("Validation Disabled")] 
        ValidationDisabled = 3
    }
}
