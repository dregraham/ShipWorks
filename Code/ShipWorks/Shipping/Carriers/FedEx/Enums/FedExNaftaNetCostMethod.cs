using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration to map to the FedEx NAFTA net cost method values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExNaftaNetCostMethod
    {
        [Description("Not calculated according to the net cost method")]
        NotCalculated = 0,

        [Description("Calculated according to the net cost method")]
        NetCostMethod = 1
    }
}
