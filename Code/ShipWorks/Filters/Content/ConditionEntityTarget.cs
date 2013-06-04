using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Enumerates the possible target subjects of a condition.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ConditionEntityTarget
    {
        [Description("Customer")] 
        Customer,

        [Description("Order")] 
        Order,

        [Description("Shipment")]
        Shipment,

        [Description("OrderItem")] 
        OrderItem,

        [Description("OrderCharge")]
        OrderCharge,

        [Description("Note")]
        Note,

        [Description("Email")]
        Email,

        [Description("Printed")]
        Printed,

        [Description("PaymentDetail")]
        PaymentDetail
    }
}
