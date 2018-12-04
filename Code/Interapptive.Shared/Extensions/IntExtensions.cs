using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Int type extension methods
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Clamp a value to an allowable range
        /// </summary>
        public static int Clamp(this int value, int left, int right)
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