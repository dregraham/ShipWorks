using ShipWorks.Stores.Platforms.Magento;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoTwoRestOrderNumberUtilityTest
    {
        [Theory]
        [InlineData("2", 2)]
        [InlineData("1222222-1", 1222222)]
        [InlineData("123-445", 123)]
        [InlineData("123445-5234", 123445)]
        [InlineData("12341234", 12341234)]
        [InlineData("234234-1", 234234)]
        [InlineData("2", 2)]
        [InlineData("22", 22)]
        [InlineData("2222", 2222)]
        [InlineData("222", 222)]
        [InlineData("2134-", 2134)]
        public void GetOrderNumber_ReturnsOrderNumber_WhenIncrementIdIsInAnExpectedFormat(string incrementId, long orderNumber)
        {
            Assert.Equal(orderNumber, MagentoTwoRestOrderNumberUtility.GetOrderNumber(incrementId));
        }

        [Theory]
        [InlineData("123--1")]
        [InlineData("abc-1222222-1")]
        [InlineData("1abc1")]
        [InlineData("abc123")]
        [InlineData("123abc")]
        [InlineData("!@#$")]
        [InlineData("--345")]
        [InlineData("-")]
        
        public void GetOrderNumber_ThrowsMagentoExcpetion_WhenIncrementIdIsInUnexpectedFormat(string incrementId)
        {
            MagentoException ex = Assert.Throws<MagentoException>(() =>  MagentoTwoRestOrderNumberUtility.GetOrderNumber(incrementId));
            Assert.Equal($"Order number {incrementId} is in an unknown format.", ex.Message);
        }

        [Theory]
        [InlineData("2", "")]
        [InlineData("1222222-1", "-1")]
        [InlineData("123-445", "-445")]
        [InlineData("123445-5234", "-5234")]
        [InlineData("12341234", "")]
        [InlineData("234234-1", "-1")]
        [InlineData("2", "")]
        [InlineData("22", "")]
        [InlineData("2222", "")]
        [InlineData("222", "")]
        [InlineData("2134-", "-")]
        public void GetOrderNumberPostfix_ReturnsOrderNumber_WhenIncrementIdIsInAnExpectedFormat(string incrementId, string postfix)
        {
            Assert.Equal(postfix, MagentoTwoRestOrderNumberUtility.GetOrderNumberPostfix(incrementId));
        }


        [Theory]
        [InlineData("123--1")]
        [InlineData("abc-1222222-1")]
        [InlineData("--345")]

        public void GetOrderNumberPostfix_ThrowsMagentoExcpetion_WhenIncrementIdIsInUnexpectedFormat(string incrementId)
        {
            MagentoException ex = Assert.Throws<MagentoException>(() => MagentoTwoRestOrderNumberUtility.GetOrderNumberPostfix(incrementId));
            Assert.Equal($"Order number {incrementId} is in an unknown format.", ex.Message);
        }
    }
}