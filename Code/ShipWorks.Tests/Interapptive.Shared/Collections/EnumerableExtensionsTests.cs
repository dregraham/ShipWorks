using Interapptive.Shared.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace ShipWorks.Tests.Interapptive.Shared.Collections
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void CanRepeatSequence()
        {
            var sequence = new[] { 1, 2, 5 };       //Arthurian sequence

            CollectionAssert.AreEqual(
                new[] { 1, 2, 5, 1, 2, 5, 1, 2, 5 },
                sequence.Repeat(3).ToArray()
            );
        }
    }
}
