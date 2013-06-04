using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Etsy.Enums
{
    /// <summary>
    /// Our representation of an Etsy Order Status.  If shipped and paid, Complete. Else Open.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EtsyOrderStatus
    {
        [Description("Open")]
        Open = 0,

        [Description("Complete")]
        Complete = 1,

        [Description("Not Found")]
        NotFound=2
    }
}
