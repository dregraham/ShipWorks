using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Decimal type extension methods
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Converts the value in kilograms to pounds.
        /// </summary>
        public static decimal ConvertFromKilogramsToPounds(this decimal value) => value * 2.20462262m;
    }
}