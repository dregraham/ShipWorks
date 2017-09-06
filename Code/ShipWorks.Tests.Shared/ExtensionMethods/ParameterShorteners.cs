using System;
using Moq;

namespace ShipWorks.Tests.Shared.ExtensionMethods
{
    /// <summary>
    /// Shorten the standard It.Is[T] methods
    /// </summary>
    public static class ParameterShorteners
    {
        /// <summary>
        /// Any string
        /// </summary>
        public static string AnyString => It.IsAny<string>();

        /// <summary>
        /// Any bool
        /// </summary>
        public static bool AnyBool => It.IsAny<bool>();

        /// <summary>
        /// Any long
        /// </summary>
        public static long AnyLong => It.IsAny<long>();

        /// <summary>
        /// Any int
        /// </summary>
        public static int AnyInt => It.IsAny<int>();

        /// <summary>
        /// Any date
        /// </summary>
        public static DateTime AnyDate => It.IsAny<DateTime>();

        /// <summary>
        /// Any object
        /// </summary>
        public static object AnyObject => It.IsAny<object>();
    }
}
