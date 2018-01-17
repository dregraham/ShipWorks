using System;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateShipperManipulatorTest
    {
        private FedExRateShipperManipulator testObject;
        private ShipmentEntity shipment;

        public FedExRateShipperManipulatorTest()
        {
            shipment = Create.Shipment().AsFedEx().Build();

            testObject = new FedExRateShipperManipulator();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            var result = testObject.ShouldApply(null, FedExRateRequestOptions.None);

            Assert.True(result);
        }

        [Fact]
        public void Manipulate_WithOneStreetLine_ReturnsValidStreetLines()
        {
            shipment.OriginStreet1 = "Foo";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo" }, result.RequestedShipment.Shipper.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithTwoStreetLines_ReturnsValidStreetLines()
        {
            shipment.OriginStreet1 = "Foo";
            shipment.OriginStreet2 = "Bar";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo", "Bar" }, result.RequestedShipment.Shipper.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithThreeStreetLines_ReturnsValidStreetLines()
        {
            shipment.OriginStreet1 = "Foo";
            shipment.OriginStreet2 = "Bar";
            shipment.OriginStreet3 = "Baz";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, result.RequestedShipment.Shipper.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper()
        {
            shipment.OriginPerson = new PersonAdapter
            {
                City = "Foo",
                CountryCode = "US",
                PostalCode = "63102",
                StateProvCode = "MO"
            };

            var result = testObject.Manipulate(shipment, new RateRequest());

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(result.RequestedShipment.Shipper);

            // Make sure Address fields match
            Assert.Equal("Foo", result.RequestedShipment.Shipper.Address.City);
            Assert.Equal("US", result.RequestedShipment.Shipper.Address.CountryCode);
            Assert.Equal("63102", result.RequestedShipment.Shipper.Address.PostalCode);
            Assert.Equal("MO", result.RequestedShipment.Shipper.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.Shipper.Address.Residential);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty(string value)
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipment.OriginCompany = value;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipment.OriginCompany = "Penetrode";

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.False(result.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.FedExAddressLookup;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }
    }
}
