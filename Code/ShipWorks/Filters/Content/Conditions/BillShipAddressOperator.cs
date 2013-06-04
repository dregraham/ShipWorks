using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Various operations that can be performed on an address condition
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BillShipAddressOperator
    {
        [Description("Shipping or Billing")]
        ShipOrBill = 0,

        [Description("Shipping and Billing")]
        ShipAndBill = 1,

        [Description("Shipping")]
        Ship = 2,

        [Description("Billing")]
        Bill = 3,

        [Description("Shipping and Billing are the Same")]
        ShipBillEqual = 4,

        [Description("Shipping and Billing are Different")]
        ShipBillNotEqual = 5
    }
}
