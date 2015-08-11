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

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExShipperManipulator();
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsRequestedShipment_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsInstanceOfType(nativeRequest.RequestedShipment, typeof(RequestedShipment));
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Shipper back
            Assert.IsInstanceOfType(nativeRequest.RequestedShipment.Shipper, typeof(Party));

            // Make sure Address fields match
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Address.City, shipmentEntity.OriginCity);
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Address.CountryCode, shipmentEntity.OriginCountryCode);
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Address.PostalCode, shipmentEntity.OriginPostalCode);
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode, shipmentEntity.OriginStateProvCode);

            // Make sure Contact fields match
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Contact.CompanyName, shipmentEntity.OriginCompany);
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Contact.EMailAddress, shipmentEntity.OriginEmail);
            Assert.AreEqual(nativeRequest.RequestedShipment.Shipper.Contact.PhoneNumber, shipmentEntity.OriginPhone);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsNull_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipmentEntity.OriginCompany = "Penetrode";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified_Test()
        {
            shipmentEntity.FedEx.OriginResidentialDetermination = (int)ResidentialDeterminationType.FedExAddressLookup;
            
            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_AddressAddedToRecipeint_RequestedShipmentIsReturn_Test()
        {
            shipmentEntity.ReturnShipment = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.Shipper);

            Assert.AreEqual(shipmentEntity.OriginCity, nativeRequest.RequestedShipment.Recipient.Address.City);
        }
    }
}
