using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class FedExShipmentEntityTest
    {
        [Fact]
        public void CodPersonAdapter_HasCorrecPersonNameValues()
        {
            FedExShipmentEntity shipment = new FedExShipmentEntity()
            {
                CodFirstName = "John",
                CodLastName = "Doe",
                BrokerFirstName = "a",
                BrokerLastName = "b",
                ImporterFirstName = "c",
                ImporterLastName = "d",
            };

            PersonAdapter personAdapter = new PersonAdapter(shipment, "Cod");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void BrokerPersonAdapter_HasCorrecPersonNameValues()
        {
            FedExShipmentEntity shipment = new FedExShipmentEntity()
            {
                CodFirstName = "a",
                CodLastName = "b",
                BrokerFirstName = "John",
                BrokerLastName = "Doe",
                ImporterFirstName = "c",
                ImporterLastName = "d",
            };

            PersonAdapter personAdapter = new PersonAdapter(shipment, "Broker");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void ImporterPersonAdapter_HasCorrecPersonNameValues()
        {
            FedExShipmentEntity shipment = new FedExShipmentEntity()
            {
                CodFirstName = "a",
                CodLastName = "b",
                BrokerFirstName = "c",
                BrokerLastName = "d",
                ImporterFirstName = "John",
                ImporterLastName = "Doe",
            };

            PersonAdapter personAdapter = new PersonAdapter(shipment, "Importer");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }
    }
}
