using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Enum defining Amazon SWA shipping restrictions
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [Flags]
    public enum AmazonSWAShippingRestrictionType
    {
        // Only allow Amazon carrier for Amazon orders
        [Description("Only Amazon orders are allowed.")]
        OnlyAmazon = 1,

        // Allow Amazon carrier for all orders
        [Description("Any order is allowed.")]
        AllOrders = 2
    }
}
