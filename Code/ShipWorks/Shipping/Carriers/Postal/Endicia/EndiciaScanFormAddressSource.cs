using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// What address should be printed on the generated SCAN form
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EndiciaScanFormAddressSource
    {
        [Description("Use the address of my online postage account")]
        Provider = 0,

        [Description("Use the account address from ShipWorks")]
        ShipWorks = 1
    }
}
