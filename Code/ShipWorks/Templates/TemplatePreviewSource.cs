using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Templates
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TemplatePreviewSource
    {
        [Description("Customer")]
        Customer = 0,

        [Description("Order")]
        Order = 1,

        [Description("Shipment")]
        Shipment = 2,
    }
}
