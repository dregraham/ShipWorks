using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions on nullable types
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// Match on a nullable value, applying the appropriate method base on whether a value exists or not
        /// </summary>
        public static TReturn Match<T, TReturn>(this Nullable<T> input, Func<T, TReturn> withValue, Func<TReturn> withNull) where T : struct =>
            input.HasValue ? withValue(input.Value) : withNull();

        /// <summary>
        /// Match on a nullable value, applying the appropriate method base on whether a value exists or not
        /// </summary>
        public static bool Match<T>(this Nullable<T> input, Action<T> withValue, Action withNull) where T : struct =>
            input.Match(x => { withValue(x); return true; }, () => { withNull(); return false; });
    }
}
