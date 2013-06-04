using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Various supported types of postal customs' content
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalCustomsContentType
    {
        [Description("Commercial Sample")]
        Sample = 0,

        [Description("Gift")]
        Gift = 1,
        
        [Description("Documents")]
        Documents = 2,

        [Description("Other")]
        Other = 3,

        [Description("Merchandise")]
        Merchandise = 4,

        [Description("Returned Goods")]
        ReturnedGoods = 5
    }
}
