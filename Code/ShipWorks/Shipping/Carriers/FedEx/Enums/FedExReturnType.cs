using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// The various return service methods UPS offers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExReturnType
    {
        [Description("Email Return Label")]
        EmailReturnLabel = 0,

        [Description("Print Return Label")]
        PrintReturnLabel = 1
    }
}
