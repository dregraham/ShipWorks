using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Possible contexts for template
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TemplateInputContext
    {
        [Description("Automatic")]
        Auto = 0,

        [Description("Customer")]
        Customer = 1,

        [Description("Order")]
        Order = 2,

        [Description("Shipment")]
        Shipment = 3,

        [Description("Order Item")]
        OrderItem = 4
    }
}
