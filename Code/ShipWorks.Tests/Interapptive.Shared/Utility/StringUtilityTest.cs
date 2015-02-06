using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    [TestClass]
    public class StringUtilityTest
    {
        [TestMethod]
        public void Truncate_ReturnsNull_WhenInputIsNull()
        {
            string result = ((string) null).Truncate(20);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Truncate_ReturnsEmptyString_WhenInputIsEmpty()
        {
            string result = string.Empty.Truncate(20);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Truncate_ReturnsOriginalString_WhenInputEqualsMaxLength()
        {
            string input = new string('x', 20);
            string result = input.Truncate(20);
            Assert.AreSame(input, result);
        }

        [TestMethod]
        public void Truncate_ReturnsTruncatedString_WhenInputIsLongerThanMaxLength()
        {
            string result = "Foobar".Truncate(3);
            Assert.AreEqual("Foo", result);
        }
    }
}
