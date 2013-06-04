using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// The various return service methods UPS offers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsReturnServiceType
    {
        [Description("Electronic Return Label")]
        ElectronicReturnLabel = 0,

        [Description("Print Return Label")]
        PrintReturnLabel = 1,

        [Description("UPS Print and Mail")]
        PrintAndMail = 2,

        [Description("Returns Plus, 1 Attempt")]
        ReturnPlus1 = 3,

        [Description("Returns Plus, 3 Attempts")]
        ReturnPlus3 = 4
    }
}
