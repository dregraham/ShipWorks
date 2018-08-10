using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Enum defining Amazon prime shipping restrictions
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [Flags]
    public enum AmazonPrimeShippingRestrictionType
    {
        // Only allow Amazon carrier for prime orders
        [Description("Only Amazon Prime Orders are allowed.")]
        OnlyPrime = 1,

        // Allow Amazon carrier for all Amazon orders
        [Description("Any Amazon Order is allowed.")]
        AllOrders = 2
    }
}
