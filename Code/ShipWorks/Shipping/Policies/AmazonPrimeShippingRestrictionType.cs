using System;
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
        OnlyPrime = 1,

        // Allow Amazon carrier for all Amazon orders
        AllOrders = 2
    }
}
