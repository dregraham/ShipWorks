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

        /// <summary>
        /// Converts the value in kilograms to pounds.
        /// </summary>
        public static decimal ConvertFromKilogramsToPounds(this decimal value) => value * 2.20462262m;
    }
}