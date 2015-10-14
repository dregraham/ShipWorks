using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Xunit;
using System.Linq;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using Moq;
using System.Collections;

namespace ShipWorks.Tests.Interapptive.Shared.Collections
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void SplitIntoChunksOf_ReturnsChunksOfSpecifiedSize()
        {
            var sequence = Enumerable.Range(0, 30);
            var chunks = sequence.SplitIntoChunksOf(10);
            Assert.Equal(3, chunks.Count());
            Assert.Equal(10, chunks.ElementAt(0).Count());
            Assert.Equal(10, chunks.ElementAt(1).Count());
            Assert.Equal(10, chunks.ElementAt(2).Count());
        }

        [Fact]
        public void SplitIntoChunksOf_LastChunkContainsRemainderOfItems_WhenCollectionCannotBeEvenlyDividedByChunkSize()
        {
            var sequence = Enumerable.Range(0, 14);
            var chunks = sequence.SplitIntoChunksOf(5);
            Assert.Equal(3, chunks.Count());
            Assert.Equal(5, chunks.ElementAt(0).Count());
            Assert.Equal(5, chunks.ElementAt(1).Count());
            Assert.Equal(4, chunks.ElementAt(2).Count());
        }

        [Fact]
        public void SplitIntoChunksOf_ItemsAreAddedToExpectedChunk()
        {
            var sequence = Enumerable.Range(0, 10);
            var chunks = sequence.SplitIntoChunksOf(3);
            Assert.Equal(0, chunks.ElementAt(0).ElementAt(0));
            Assert.Equal(1, chunks.ElementAt(0).ElementAt(1));
            Assert.Equal(2, chunks.ElementAt(0).ElementAt(2));
            Assert.Equal(3, chunks.ElementAt(1).ElementAt(0));
            Assert.Equal(4, chunks.ElementAt(1).ElementAt(1));
            Assert.Equal(5, chunks.ElementAt(1).ElementAt(2));
            Assert.Equal(6, chunks.ElementAt(2).ElementAt(0));
            Assert.Equal(7, chunks.ElementAt(2).ElementAt(1));
            Assert.Equal(8, chunks.ElementAt(2).ElementAt(2));
            Assert.Equal(9, chunks.ElementAt(3).ElementAt(0));
        }

        [Fact]
        public void SplitIntoChunksOf_ReturnsEmptyCollection_WhenSourceIsEmpty()
        {
            var chunks = (new List<int>()).SplitIntoChunksOf(5);
            Assert.Equal(0, chunks.Count());
        }

        [Fact]
        public void SplitIntoChunksOf_ThrowsIfSourceIsNull()
        {
            try
            {
                EnumerableExtensions.SplitIntoChunksOf<int>(null, 100).Count();
                Assert.False(true, "ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("source", ex.ParamName);
            }
        }

        [Fact]
        public void SplitIntoChunksOf_ThrowsIfChunkSizeIsLessThanOne()
        {
            try
            {
                (new List<int>()).SplitIntoChunksOf(0).Count();
                Assert.False(true, "ArgumentOutOfRangeException should have been thrown.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.Equal("size", ex.ParamName);
            }
        }

        [Fact]
        public void CanRepeatSequence()
        {
            var sequence = new[] { 1, 2, 5 };       //Arthurian sequence

            Assert.Equal(
                new[] { 1, 2, 5, 1, 2, 5, 1, 2, 5 },
                sequence.Repeat(3).ToArray()
            );
        }

        [Fact]
        public void Combine_ReturnsEmpty_WithNullList()
        {
            string result = ((IEnumerable<string>)null).Combine();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Combine_ReturnsEmpty_WithEmptyList()
        {
            string result = (new List<string>()).Combine();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Combine_ReturnsCombinedStrings_WithValidList()
        {
            List<string> list = new List<string> {"foo", "bar"};
            string result = list.Combine();
            Assert.Equal("foobar", result);
        }

        [Fact]
        public void Combine_ReturnsDelimitedCombinedStrings_WithValidList()
        {
            List<string> list = new List<string> { "foo", "bar" };
            string result = list.Combine(", ");
            Assert.Equal("foo, bar", result);
        }

        [Fact]
        public void Combine_ReturnsEmpty_WithNullCharList()
        {
            string result = ((IEnumerable<string>)null).Combine();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Combine_ReturnsEmpty_WithEmptyCharList()
        {
            string result = (new List<char>()).Combine();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Combine_ReturnsCombinedStrings_WithValidCharList()
        {
            List<char> list = new List<char> { 'a', 'b' };
            string result = list.Combine();
            Assert.Equal("ab", result);
        }

        [Fact]
        public void Combine_ReturnsDelimitedCombinedStrings_WithValidCharList()
        {
            List<char> list = new List<char> { 'a', 'b' };
            string result = list.Combine(", ");
            Assert.Equal("a, b", result);
        }

        [Fact]
        public void Except_ReturnsElementsThatDoNotMatch_BasedOnProperty()
        {
            List<string> list = new List<string> {"foo", "bar", "baz"};
            List<string> result = list.Except(new List<string> {"hat"}, x => x[1]).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal("foo", result[0]);
        }

        [Fact]
        public void None_WithEmptyCollection_ReturnsTrue()
        {
            bool result = new List<string>().None();
            Assert.True(result);
        }

        [Fact]
        public void None_WithSingleItem_ReturnsFalse()
        {
            bool result = Enumerable.Range(0, 1).None();
            Assert.False(result);
        }

        [Fact]
        public void None_WithPredicateThatReturnsEmptyResults_ReturnsTrue()
        {
            bool result = Enumerable.Range(0, 100).None(x => x == 1000);
            Assert.True(result);
        }

        [Fact]
        public void None_WithPredicateThatReturnsOneResult_ReturnsFalse()
        {
            bool result = Enumerable.Range(0, 100).None(x => x == 20);
            Assert.False(result);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithFewerItems_ReturnsNegative()
        {
            int result = Enumerable.Range(0, 5).HasMoreOrLessThanCount(6);
            Assert.InRange(result, int.MinValue, -1);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithMoreItems_ReturnsPositive()
        {
            int result = Enumerable.Range(0, 5).HasMoreOrLessThanCount(4);
            Assert.InRange(result, 1, int.MaxValue);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithSameItems_ReturnsZero()
        {
            int result = Enumerable.Range(0, 5).HasMoreOrLessThanCount(5);
            Assert.Equal(0, result);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithFewerItemsAndGenericICollection_ReturnsNegativeUsingCount()
        {
            Mock<ICollection<string>> testObject = new Mock<ICollection<string>>();
            testObject.SetupGet(x => x.Count).Returns(2);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.True(testObject.Object.HasMoreOrLessThanCount(3) < 0);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithMoreItemsAndGenericICollection_ReturnsPositiveUsingCount()
        {
            Mock<ICollection<string>> testObject = new Mock<ICollection<string>>();
            testObject.SetupGet(x => x.Count).Returns(4);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.True(testObject.Object.HasMoreOrLessThanCount(3) > 0);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithEqualItemsAndGenericICollection_ReturnsZeroUsingCount()
        {
            Mock<ICollection<string>> testObject = new Mock<ICollection<string>>();
            testObject.SetupGet(x => x.Count).Returns(3);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.Equal(0, testObject.Object.HasMoreOrLessThanCount(3));
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithFewerItemsAndICollection_ReturnsNegativeUsingCount()
        {
            Mock<ICollection> testObject = new Mock<ICollection>();
            testObject.SetupGet(x => x.Count).Returns(2);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.True(testObject.Object.HasMoreOrLessThanCount(3) < 0);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithMoreItemsAndICollection_ReturnsPositiveUsingCount()
        {
            Mock<ICollection> testObject = new Mock<ICollection>();
            testObject.SetupGet(x => x.Count).Returns(4);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.True(testObject.Object.HasMoreOrLessThanCount(3) > 0);
        }

        [Fact]
        public void HasMoreOrLessThanCount_WithEqualItemsAndICollection_ReturnsZeroUsingCount()
        {
            Mock<ICollection> testObject = new Mock<ICollection>();
            testObject.SetupGet(x => x.Count).Returns(3);
            testObject.Setup(x => x.GetEnumerator()).Throws<InvalidOperationException>();
            Assert.Equal(0, testObject.Object.HasMoreOrLessThanCount(3));
        }
    }
}
