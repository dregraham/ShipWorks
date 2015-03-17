using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;


namespace ShipWorks.Tests.Interapptive.Shared.Collections
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void SplitIntoChunksOf_ReturnsChunksOfSpecifiedSize()
        {
            var sequence = Enumerable.Range(0, 30);
            var chunks = sequence.SplitIntoChunksOf(10);
            Assert.AreEqual(3, chunks.Count());
            Assert.AreEqual(10, chunks.ElementAt(0).Count());
            Assert.AreEqual(10, chunks.ElementAt(1).Count());
            Assert.AreEqual(10, chunks.ElementAt(2).Count());
        }

        [TestMethod]
        public void SplitIntoChunksOf_LastChunkContainsRemainderOfItems_WhenCollectionCannotBeEvenlyDividedByChunkSize()
        {
            var sequence = Enumerable.Range(0, 14);
            var chunks = sequence.SplitIntoChunksOf(5);
            Assert.AreEqual(3, chunks.Count());
            Assert.AreEqual(5, chunks.ElementAt(0).Count());
            Assert.AreEqual(5, chunks.ElementAt(1).Count());
            Assert.AreEqual(4, chunks.ElementAt(2).Count());
        }

        [TestMethod]
        public void SplitIntoChunksOf_ItemsAreAddedToExpectedChunk()
        {
            var sequence = Enumerable.Range(0, 10);
            var chunks = sequence.SplitIntoChunksOf(3);
            Assert.AreEqual(0, chunks.ElementAt(0).ElementAt(0));
            Assert.AreEqual(1, chunks.ElementAt(0).ElementAt(1));
            Assert.AreEqual(2, chunks.ElementAt(0).ElementAt(2));
            Assert.AreEqual(3, chunks.ElementAt(1).ElementAt(0));
            Assert.AreEqual(4, chunks.ElementAt(1).ElementAt(1));
            Assert.AreEqual(5, chunks.ElementAt(1).ElementAt(2));
            Assert.AreEqual(6, chunks.ElementAt(2).ElementAt(0));
            Assert.AreEqual(7, chunks.ElementAt(2).ElementAt(1));
            Assert.AreEqual(8, chunks.ElementAt(2).ElementAt(2));
            Assert.AreEqual(9, chunks.ElementAt(3).ElementAt(0));
        }

        [TestMethod]
        public void SplitIntoChunksOf_ReturnsEmptyCollection_WhenSourceIsEmpty()
        {
            var chunks = (new List<int>()).SplitIntoChunksOf(5);
            Assert.AreEqual(0, chunks.Count());
        }

        [TestMethod]
        public void SplitIntoChunksOf_ThrowsIfSourceIsNull()
        {
            try
            {
                EnumerableExtensions.SplitIntoChunksOf<int>(null, 100).Count();
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("source", ex.ParamName);
            }
        }

        [TestMethod]
        public void SplitIntoChunksOf_ThrowsIfChunkSizeIsLessThanOne()
        {
            try
            {
                (new List<int>()).SplitIntoChunksOf(0).Count();
                Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual("size", ex.ParamName);
            }
        }

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

        [TestMethod]
        public void Except_ReturnsElementsThatDoNotMatch_BasedOnProperty()
        {
            List<string> list = new List<string> {"foo", "bar", "baz"};
            List<string> result = list.Except(new List<string> {"hat"}, x => x[1]).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("foo", result[0]);
        }
    }
}
