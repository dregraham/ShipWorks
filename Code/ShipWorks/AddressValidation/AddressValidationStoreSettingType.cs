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
        [Description("Automaticly Validate and Apply")] 
        ValidateAndApply = 0,

        [Description("Automatically Validate and Notify")] 
        ValidateAndNotify = 1,

        [Description("Manual Validation Only")] 
        ManualValidationOnly = 2,

        [Description("ValidationDisabled")] 
        ValidationDisabled = 3
    }
}
