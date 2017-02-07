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
                BillFirstName = "asdfasdf",
                BillMiddleName = "asdf",
                BillLastName = "asdf"
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
                ShipFirstName = "asdfasdf",
                ShipMiddleName = "asdf",
                ShipLastName = "asdf",
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
    }
}
