﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
﻿using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Valid shipment charge types for a ups shipment
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsShipmentChargeType
    {
        [Description("Bill Shipper")]
        BillShipper = 0,

        [Description("Bill Receiver")]
        BillReceiver = 1,

        [Description("Bill Third Party")]
        BillThirdParty = 2
    }
}
