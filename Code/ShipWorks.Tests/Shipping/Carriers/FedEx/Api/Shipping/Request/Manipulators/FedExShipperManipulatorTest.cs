using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExShipperManipulatorTest
    {
        private FedExShipperManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExShipperManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExShipperManipulator();
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Shipper);

            // Make sure Address fields match
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.City, shipmentEntity.OriginCity);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.CountryCode, shipmentEntity.OriginCountryCode);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.PostalCode, shipmentEntity.OriginPostalCode);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode, shipmentEntity.OriginStateProvCode);

            // Make sure Contact fields match
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Contact.CompanyName, shipmentEntity.OriginCompany);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Contact.EMailAddress, shipmentEntity.OriginEmail);
            Assert.Equal(nativeRequest.RequestedShipment.Shipper.Contact.PhoneNumber, shipmentEntity.OriginPhone);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsNull()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = "Penetrode";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.FedExAddressLookup;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AddressAddedToRecipeint_RequestedShipmentIsReturn()
        {
            shipmentEntity.ReturnShipment = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.Shipper);

            Assert.Equal(shipmentEntity.OriginCity, nativeRequest.RequestedShipment.Recipient.Address.City);
        }
    }
}
