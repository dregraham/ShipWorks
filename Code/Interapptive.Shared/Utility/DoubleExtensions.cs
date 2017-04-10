using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Double type extension methods
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Checks to see if the double is equivalent to another double with a variance of .001
        /// </summary>
        public static bool IsEquivalentTo(this double value, double number) =>
            Math.Abs(value - number) < .001;

        /// <summary>
        /// Check to see if the double is an int
        /// </summary>
        public static bool IsInt(this double value) =>
            Math.Abs(value % 1) <= Double.Epsilon * 100;

        /// <summary>
        /// Clamp a value to an allowable range
        /// </summary>
        public static double Clamp(this double value, double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("Min must be less than max");
            }

            if (value < min)
            {
                return min;
            }

            return Math.Min(value, max);
        }
    }
}