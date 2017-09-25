using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Parameter is a json string that matches the test
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static string Json(Func<JObject, bool> test) =>
            It.Is<string>(x => test(JObject.Parse(x)));
    }
}
