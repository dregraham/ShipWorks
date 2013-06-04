using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Enums
{
    // Enum to search by order created date or modified date
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThreeDCartWebClientOrderDateSearchType
    {
        [Description("Created Date")]
        CreatedDate = 0,

        [Description("Modified Date")]
        ModifiedDate = 1
    }
}
