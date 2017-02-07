using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class ShipmentEntityTest
    {
        [Fact]
        public void ShipPersonAdapter_HasCorrecPersonNameValues()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipFirstName = "John",
                ShipMiddleName = "Mid",
                ShipLastName = "Doe",
                OriginFirstName = "asdfasdf",
                OriginMiddleName = "asdf",
                OriginLastName = "asdf"
            };

            PersonAdapter personAdapter = new PersonAdapter(shipment, "Ship");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void OriginPersonAdapter_HasCorrecPersonNameValues()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipFirstName = "asdfasdf",
                ShipMiddleName = "asdf",
                ShipLastName = "asdf",
                OriginFirstName = "John",
                OriginMiddleName = "Mid",
                OriginLastName = "Doe"
            };

            PersonAdapter personAdapter = new PersonAdapter(shipment, "Origin");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }
    }
}
