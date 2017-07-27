using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class OrderEntityTest
    {
        [Fact]
        public void ShipPersonAdapter_HasCorrecPersonNameValues()
        {
            OrderEntity order = new OrderEntity()
            {
                ShipFirstName = "John",
                ShipMiddleName = "Mid",
                ShipLastName = "Doe",
            };

            PersonAdapter personAdapter = new PersonAdapter(order, "Ship");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void BillPersonAdapter_HasCorrecPersonNameValues()
        {
            OrderEntity order = new OrderEntity()
            {
                BillFirstName = "John",
                BillMiddleName = "Mid",
                BillLastName = "Doe"
            };

            PersonAdapter personAdapter = new PersonAdapter(order, "Bill");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Theory]
        [InlineData("1A2B3C")]
        [InlineData("-1")]
        [InlineData("1A-2B3C")]
        [InlineData("Has some spaces!")]
        public void ChangeOrderNumber_SetsOrderNumberComplete(string orderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber);
            Assert.Equal(orderNumber, order.OrderNumberComplete);
        }

        [Theory]
        [InlineData("12345", 12345)]
        [InlineData("-1", -1)]
        public void ChangeOrderNumber_SetsOrderNumber_WhenGivenNumericValue(string orderNumberToSet, long expectedOrderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumberToSet);

            Assert.Equal(expectedOrderNumber, order.OrderNumber);
        }

        [Theory]
        [InlineData("1A2B3C")]
        [InlineData("1.5")]
        public void ChangeOrderNumber_SetsOrderNumberToMinLong_WhenGivenNonNumericValue(string orderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber);

            Assert.Equal(long.MinValue, order.OrderNumber);
        }

        [Theory]
        [InlineData("", "1", "", "1", 1)]
        [InlineData("abc", "1", "", "abc1", 1)]
        [InlineData("", "1", "def", "1def", 1)]
        [InlineData("", "abc", "", "abc", -9223372036854775808)]
        [InlineData("abc", "def", "ghi", "abcdefghi", -9223372036854775808)]
        [InlineData("abc", "", "", "abc", -9223372036854775808)]
        [InlineData("", "", "def", "def", -9223372036854775808)]
        [InlineData("", "", "", "", -9223372036854775808)]
        [InlineData("", "a12d34", "", "a12d34", -9223372036854775808)]
        public void ChangeOrderNumber_SetsOrderNumberCompleteWithPrefixPostfix(string prefix, string orderNumber,
            string postfix, string expectedOrderNumberComplete, long expectedOrderNumber)
        {
            
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber, prefix, postfix);

            Assert.Equal(expectedOrderNumber, order.OrderNumber);
            Assert.Equal(expectedOrderNumberComplete, order.OrderNumberComplete);
        }
    }
}
