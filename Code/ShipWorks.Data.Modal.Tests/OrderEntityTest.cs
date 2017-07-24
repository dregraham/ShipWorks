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

        [Fact]
        public void SetOrderNumber_SetsOrderNumberComplete()
        {
            OrderEntity order = new OrderEntity();
            order.SetOrderNumber("1A2B3C");
            Assert.Equal("1A2B3C", order.OrderNumberComplete);
        }

        [Fact]
        public void SetOrderNumber_SetsOrderNumber_WhenGivenNumericValue()
        {
            OrderEntity order = new OrderEntity();
            order.SetOrderNumber("12345");

            Assert.Equal(12345, order.OrderNumber);
        }

        [Fact]
        public void SetOrderNumber_SetsOrderNumberToMinLong_WhenGivenNonNumericValue()
        {
            OrderEntity order = new OrderEntity();
            order.SetOrderNumber("1A2B3C");

            Assert.Equal(long.MinValue, order.OrderNumber);
        }
    }
}
