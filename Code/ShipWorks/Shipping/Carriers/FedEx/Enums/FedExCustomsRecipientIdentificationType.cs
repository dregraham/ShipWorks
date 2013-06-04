using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// The types of identification for customs.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCustomsRecipientIdentificationType
    {
        [Description("None")]
        None = 0, 

        [Description("Passport")]
        Passport = 1,

        [Description("Individual")]
        Individual = 2,

        [Description("Company")]
        Company = 3
    }
}
