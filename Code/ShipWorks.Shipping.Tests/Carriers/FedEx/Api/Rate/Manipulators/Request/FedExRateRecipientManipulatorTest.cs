using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateRecipientManipulatorTest
    {
        private FedExRateRecipientManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateRecipientManipulatorTest()
        {
            shipmentEntity = Create.Shipment().AsFedEx().Build();

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateRecipientManipulator();
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_WithOneStreetLine_ReturnsValidStreetLines()
        {
            shipmentEntity.ShipStreet1 = "Foo";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo" }, nativeRequest.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithTwoStreetLines_ReturnsValidStreetLines()
        {
            shipmentEntity.ShipStreet1 = "Foo";
            shipmentEntity.ShipStreet2 = "Bar";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo", "Bar" }, nativeRequest.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithThreeStreetLines_ReturnsValidStreetLines()
        {
            shipmentEntity.ShipStreet1 = "Foo";
            shipmentEntity.ShipStreet2 = "Bar";
            shipmentEntity.ShipStreet3 = "Baz";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, nativeRequest.RequestedShipment.Recipient.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsValidRequestedShipmentRecipient()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Recipient back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Recipient);

            // Make sure the Address matches what we input
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.City, shipmentEntity.ShipCity);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.CountryCode, shipmentEntity.ShipCountryCode);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.PostalCode, shipmentEntity.ShipPostalCode);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode, shipmentEntity.ShipStateProvCode);

            // Make sure residential info matches
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipmentEntity))
            {
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.Residential, shipmentEntity.ResidentialResult);
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, true);
            }
            else
            {
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.Residential, false);
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, false);
            }
        }

        [Theory]
        [InlineData("gu", "US")]
        [InlineData("Guam", "US")]
        [InlineData("GGG", "GU")]
        [InlineData("GGG", "guam")]
        public void Manipulate_SendingToGuamSetsStateToBlankAndCountryToGU(string state, string country)
        {
            shipmentEntity.ShipStateProvCode = "GU";
            shipmentEntity.ShipCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(string.Empty, nativeRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode);
            Assert.Equal("GU", nativeRequest.RequestedShipment.Recipient.Address.CountryCode);
        }
    }
}
