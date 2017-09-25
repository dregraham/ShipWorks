using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Represents a range of values
    /// </summary>
    public static class Range
    {
        /// <summary>
        /// Create a range from the given value
        /// </summary>
        public static Range<T> From<T>(T start, T end) where T : IComparable =>
            new Range<T>(start, end);

        /// <summary>
        /// Create a range from the given value
        /// </summary>
        public static Range<T> To<T>(this T start, T end) where T : IComparable =>
            new Range<T>(start, end);
    }

    /// <summary>
    /// Represents a range of values
    /// </summary>
    public class Range<T> where T : IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Range(T start, T end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Start value
        /// </summary>
        public T Start { get; }

        /// <summary>
        /// End value
        /// </summary>
        public T End { get; }

        /// <summary>
        /// Does the range include the specified value
        /// </summary>
        public bool Contains(T value) =>
            value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
    }
}
