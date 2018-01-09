using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions on all objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert an object to a generic result
        /// </summary>
        /// <typeparam name="T">Type of object to convert</typeparam>
        /// <param name="value">Value that should be converted to a GenericResult</param>
        /// <returns>GenericResult that's successful when value is not null, otherwise a failed GenericResult</returns>
        [SuppressMessage("ShipWorks", "SW0002")]
        public static GenericResult<T> ToResult<T>(this T value) where T : class =>
            value.ToResult(() => new ArgumentNullException(nameof(value)));

        /// <summary>
        /// Convert an object to a generic result
        /// </summary>
        /// <typeparam name="T">Type of object to convert</typeparam>
        /// <param name="value">Value that should be converted to a GenericResult</param>
        /// <param name="whenNull"></param>
        /// <returns>GenericResult that's successful when value is not null, otherwise a failed GenericResult</returns>
        [SuppressMessage("ShipWorks", "SW0002")]
        public static GenericResult<T> ToResult<T>(this T value, Func<Exception> whenNull) where T : class =>
            value == null ? GenericResult.FromError<T>(whenNull()) : value;
    }
}
