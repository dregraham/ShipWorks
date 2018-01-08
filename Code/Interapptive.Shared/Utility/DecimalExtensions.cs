using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Decimal type extension methods
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Check to see if the decimal is an int
        /// </summary>
        public static bool IsInt(this decimal value) =>
            Math.Abs(value % 1) == 0;

        /// <summary>
        /// Clamp a value to an allowable range
        /// </summary>
        public static decimal Clamp(this decimal value, decimal left, decimal right)
        {
            var (min, max) = left > right ?
                (right, left) :
                (left, right);

            if (value < min)
            {
                return min;
            }

            return Math.Min(value, max);
        }
    }
}