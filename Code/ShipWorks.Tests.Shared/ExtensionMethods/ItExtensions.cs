using System.Collections.Generic;
using System.Linq;
using Moq;

namespace ShipWorks.Tests.Shared.ExtensionMethods
{
    /// <summary>
    /// Custom parameter matchers
    /// </summary>
    public static class ItIs
    {
        /// <summary>
        /// Parameter is an IEnumerable that has the items in the specified order
        /// </summary>
        public static IEnumerable<T> Enumerable<T>(params T[] items) =>
            It.Is<IEnumerable<T>>(y => y.SequenceEqual(items));
    }
}
