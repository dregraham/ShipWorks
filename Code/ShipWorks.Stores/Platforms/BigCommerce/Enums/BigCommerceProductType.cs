using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BigCommerce.Enums
{
    // Enum that represents the types of products
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BigCommerceProductType
    {
        [Description("Physical")]
        [ApiValue("physical")]
        Physical = 0,

        [Description("Digital")]
        [ApiValue("digital")]
        Digital = 1,

        [Description("Gift Certificate")]
        [ApiValue("giftcertificate")]
        GiftCertificate = 2
    }
}
