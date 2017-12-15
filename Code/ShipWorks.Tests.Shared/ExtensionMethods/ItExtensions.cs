using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

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
        public static string Json(Func<JObject, bool> test) =>
            It.Is<string>(x => test(JObject.Parse(x)));

        /// <summary>
        /// Parameter is a specific EntityField
        /// </summary>
        public static EntityField2 Field(EntityField2 field) =>
            It.Is<EntityField2>(y => y.Name == field.Name);
    }
}
