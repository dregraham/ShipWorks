using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateShipperManipulatorTest
    {
        private FedExRateShipperManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateShipperManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateShipperManipulator();
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsRequestedShipment_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidStreetLines_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Shipper);

            // Make sure Address fields match
            string[] addressLines = nativeRequest.RequestedShipment.Shipper.Address.StreetLines;

            Assert.Equal(shipmentEntity.OriginStreet1, addressLines[0]);

            if (addressLines.Length > 1)
            {
                // Check address line 2
                Assert.Equal(shipmentEntity.OriginStreet2, addressLines[1]);
            }

            if (addressLines.Length > 2)
            {
                // Check address line 3
                Assert.Equal(shipmentEntity.OriginStreet3, addressLines[2]);
            }
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Shipper);

            // Make sure Address fields match
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.City, shipmentEntity.OriginCity);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.CountryCode, shipmentEntity.OriginCountryCode);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.PostalCode, shipmentEntity.OriginPostalCode);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode, shipmentEntity.OriginStateProvCode);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsNull_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = "Penetrode";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.FedExAddressLookup;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }
    }
}
