using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShipperManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExShipperManipulator testObject;

        public FedExShipperManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();

            processShipmentRequest = new ProcessShipmentRequest();

            testObject = mock.Create<FedExShipperManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.IsAssignableFrom<RequestedShipment>(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidRequestedShipmentShipper()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure we got a Shipper back
            Assert.IsAssignableFrom<Party>(processShipmentRequest.RequestedShipment.Shipper);

            // Make sure Address fields match
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Address.City, shipment.OriginCity);
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Address.CountryCode, shipment.OriginCountryCode);
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Address.PostalCode, shipment.OriginPostalCode);
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode, shipment.OriginStateProvCode);

            // Make sure Contact fields match
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Contact.CompanyName, shipment.OriginCompany);
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Contact.EMailAddress, shipment.OriginEmail);
            Assert.Equal(processShipmentRequest.RequestedShipment.Shipper.Contact.PhoneNumber, shipment.OriginPhone);
        }

        [Fact]
        public void Manipulate_ResidentialSpecifiedIsTrue()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.Shipper.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenResidentialTypeIsSpecified()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.Residential;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsTrue_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyIsEmpty()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipment.OriginCompany = string.Empty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ShipperAddressResidentialFlagIsFalse_WhenCommercialIfCompanyTypeIsSpecified_AndCompanyHasValue()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipment.OriginCompany = "Penetrode";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.False(processShipmentRequest.RequestedShipment.Shipper.Address.Residential);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExAddressLookupTypeIsSpecified()
        {
            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.FedExAddressLookup;

            var result = testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_AddressAddedToRecipeint_RequestedShipmentIsReturn()
        {
            shipment.ReturnShipment = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.Shipper);

            Assert.Equal(shipment.OriginCity, processShipmentRequest.RequestedShipment.Recipient.Address.City);
        }
    }
}
