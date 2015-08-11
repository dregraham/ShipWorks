using System.Linq;
using Interapptive.Shared.Collections;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Collections
{
    public class ArrayUtilityTest
    {
        [Fact]
        public void ParseCommaSeparatedList_WithEmptyList_ReturnsEmptyList()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("");
            Assert.IsFalse(result.Any());
        }

        [Fact]
        public void ParseCommaSeparatedList_WithOneItemInList_ReturnsSingleItemList()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("6");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(6, result[0]);
        }

        [Fact]
        public void ParseCommaSeparatedList_WithTwoItemsInList_ReturnsTwoItemList()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("6,9");
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(6, result[0]);
            Assert.AreEqual(9, result[1]);
        }

        [Fact]
        public void ParseCommaSeparatedList_WithTwoItemsInListAndSpaceBetweenItems_ReturnsTwoItemList()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("6 , 9");
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(6, result[0]);
            Assert.AreEqual(9, result[1]);
        }

        [Fact]
        public void ParseCommaSeparatedList_WithEmptyItemInList_SkipsEmptyItem()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("6,,9");
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(6, result[0]);
            Assert.AreEqual(9, result[1]);
        }

        [Fact]
        public void ParseCommaSeparatedList_WithBadItemInList_SkipsEmptyItem()
        {
            var result = ArrayUtility.ParseCommaSeparatedList<int>("6,foo,9");
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(6, result[0]);
            Assert.AreEqual(9, result[1]);
        }
    }
}
