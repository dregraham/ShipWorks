﻿using Moq;

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
        /// Any long
        /// </summary>
        public static int AnyInt => It.IsAny<int>();
    }
}
