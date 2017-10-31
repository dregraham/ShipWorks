using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateShipperManipulatorTest
    {
        private FedExRateShipperManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateShipperManipulatorTest()
        {
            shipmentEntity = Create.Shipment().AsFedEx().Build();

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateShipperManipulator();
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_WithOneStreetLine_ReturnsValidStreetLines()
        {
            shipmentEntity.OriginStreet1 = "Foo";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo" }, nativeRequest.RequestedShipment.Shipper.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithTwoStreetLines_ReturnsValidStreetLines()
        {
            shipmentEntity.OriginStreet1 = "Foo";
            shipmentEntity.OriginStreet2 = "Bar";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo", "Bar" }, nativeRequest.RequestedShipment.Shipper.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_WithThreeStreetLines_ReturnsValidStreetLines()
        {
            shipmentEntity.OriginStreet1 = "Foo";
            shipmentEntity.OriginStreet2 = "Bar";
            shipmentEntity.OriginStreet3 = "Baz";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, nativeRequest.RequestedShipment.Shipper.Address.StreetLines);
        }



        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper()
        {
            shipmentEntity.OriginPerson = new PersonAdapter
            {
                City = "Foo",
                CountryCode = "US",
                PostalCode = "63102",
                StateProvCode = "MO"
            };

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Shipper);

            // Make sure Address fields match
            Assert.Equal("Foo", nativeRequest.RequestedShipment.Shipper.Address.City);
            Assert.Equal("US", nativeRequest.RequestedShipment.Shipper.Address.CountryCode);
            Assert.Equal("63102", nativeRequest.RequestedShipment.Shipper.Address.PostalCode);
            Assert.Equal("MO", nativeRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty(string value)
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = value;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = "Penetrode";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.FedExAddressLookup;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }
    }
}
