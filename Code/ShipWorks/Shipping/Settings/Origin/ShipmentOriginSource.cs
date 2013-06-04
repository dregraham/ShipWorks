using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// The source of the origin address
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentOriginSource
    {
        /// <summary>
        /// Address is pulled from the store address
        /// </summary>
        Store = 0,

        /// <summary>
        /// Address is entered manually
        /// </summary>
        Other = 1,

        /// <summary>
        /// Address is pulled from the shipping account.  This is carrier specific, and not supported by all carriers.
        /// </summary>
        Account = 2

        //
        // Note: Any value over 1000 is assumed to be the ID of an ShippingOrigin
        //
    }
}
