using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Decimal type extension methods
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Checks to see if the decimal is equivalent to another decimal with a variance of .001
        /// </summary>
        public static bool IsEquivalentTo(this decimal value, decimal number) =>
            Math.Abs(value - number) < .001M;

    }
}