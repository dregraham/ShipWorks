using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateRecipientManipulatorTest
    {
        private FedExRateRecipientManipulator testObject;
        private ShipmentEntity shipment;

        public FedExRateRecipientManipulatorTest()
        {
            shipment = Create.Shipment().AsFedEx().Build();

            testObject = new FedExRateRecipientManipulator();
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.IsAssignableFrom<RequestedShipment>(result.RequestedShipment);
        }

        [Fact]
        public void Manipulate_WithOneStreetLine_ReturnsValidStreetLines()
        {
            shipment.ShipStreet1 = "Foo";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo" }, result.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithTwoStreetLines_ReturnsValidStreetLines()
        {
            shipment.ShipStreet1 = "Foo";
            shipment.ShipStreet2 = "Bar";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo", "Bar" }, result.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithThreeStreetLines_ReturnsValidStreetLines()
        {
            shipment.ShipStreet1 = "Foo";
            shipment.ShipStreet2 = "Bar";
            shipment.ShipStreet3 = "Baz";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, result.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsValidRequestedShipmentRecipient()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            // Make sure we got a Recipient back
            Assert.IsAssignableFrom<Party>(result.RequestedShipment.Recipient);

            // Make sure the Address matches what we input
            Assert.Equal(result.RequestedShipment.Recipient.Address.City, shipment.ShipCity);
            Assert.Equal(result.RequestedShipment.Recipient.Address.CountryCode, shipment.ShipCountryCode);
            Assert.Equal(result.RequestedShipment.Recipient.Address.PostalCode, shipment.ShipPostalCode);
            Assert.Equal(result.RequestedShipment.Recipient.Address.StateOrProvinceCode, shipment.ShipStateProvCode);

            // Make sure residential info matches
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                Assert.Equal(result.RequestedShipment.Recipient.Address.Residential, shipment.ResidentialResult);
                Assert.Equal(result.RequestedShipment.Recipient.Address.ResidentialSpecified, true);
            }
            else
            {
                Assert.Equal(result.RequestedShipment.Recipient.Address.Residential, false);
                Assert.Equal(result.RequestedShipment.Recipient.Address.ResidentialSpecified, false);
            }
        }

        [Theory]
        [InlineData("gu", "US")]
        [InlineData("Guam", "US")]
        [InlineData("GGG", "GU")]
        [InlineData("GGG", "guam")]
        public void Manipulate_SendingToGuamSetsStateToBlankAndCountryToGU(string state, string country)
        {
            shipment.ShipStateProvCode = "GU";
            shipment.ShipCountryCode = "US";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(string.Empty, result.RequestedShipment.Recipient.Address.StateOrProvinceCode);
            Assert.Equal("GU", result.RequestedShipment.Recipient.Address.CountryCode);
        }
    }
}
