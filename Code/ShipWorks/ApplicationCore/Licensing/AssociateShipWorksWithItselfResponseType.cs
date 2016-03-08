using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AssociateShipWorksWithItselfResponseType
    {
        [Description("Success")]
        Success = 0,

        [Description("Unknown")]
        UnknownError = 1,

        [Description("PO Box Not Allowed")]
        POBoxNotAllowed = 2
    }
}