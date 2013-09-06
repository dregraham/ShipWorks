using System.Collections.Generic;
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

        [TestMethod]
        public void Combine_ReturnsEmpty_WithNullList()
        {
            string result = ((IEnumerable<string>)null).Combine();
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Combine_ReturnsEmpty_WithEmptyList()
        {
            string result = (new List<string>()).Combine();
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Combine_ReturnsCombinedStrings_WithValidList()
        {
            List<string> list = new List<string> {"foo", "bar"};
            string result = list.Combine();
            Assert.AreEqual("foobar", result);
        }

        [TestMethod]
        public void Combine_ReturnsDelimitedCombinedStrings_WithValidList()
        {
            List<string> list = new List<string> { "foo", "bar" };
            string result = list.Combine(", ");
            Assert.AreEqual("foo, bar", result);
        }

        [TestMethod]
        public void Combine_ReturnsEmpty_WithNullCharList()
        {
            string result = ((IEnumerable<string>)null).Combine();
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Combine_ReturnsEmpty_WithEmptyCharList()
        {
            string result = (new List<char>()).Combine();
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Combine_ReturnsCombinedStrings_WithValidCharList()
        {
            List<char> list = new List<char> { 'a', 'b' };
            string result = list.Combine();
            Assert.AreEqual("ab", result);
        }

        [TestMethod]
        public void Combine_ReturnsDelimitedCombinedStrings_WithValidCharList()
        {
            List<char> list = new List<char> { 'a', 'b' };
            string result = list.Combine(", ");
            Assert.AreEqual("a, b", result);
        }
    }
}
