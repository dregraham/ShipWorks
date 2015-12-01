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
    }
}