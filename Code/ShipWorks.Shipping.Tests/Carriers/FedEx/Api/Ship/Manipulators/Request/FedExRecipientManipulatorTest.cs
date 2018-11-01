using System;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    [Collection(TestCollections.IoC)]
    public class FedExRecipientManipulatorTest : IDisposable
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExRecipientManipulator testObject;
        private readonly AutoMock mock;

        public FedExRecipientManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            IoC.InitializeForUnitTests(mock.Container);

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            processShipmentRequest = new ProcessShipmentRequest();

            testObject = mock.Create<FedExRecipientManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.IsAssignableFrom<RequestedShipment>(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsValidRequestedShipmentRecipient()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure we got a Recipient back
            Assert.IsAssignableFrom<Party>(processShipmentRequest.RequestedShipment.Recipient);

            // Make sure the Address matches what we input
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Address.City, shipment.ShipCity);
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Address.CountryCode, shipment.ShipCountryCode);
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Address.PostalCode, shipment.ShipPostalCode);
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode, shipment.ShipStateProvCode);

            // Make sure Contact fields match
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName, shipment.ShipCompany);
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Contact.EMailAddress, shipment.ShipEmail);
            Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Contact.PhoneNumber, shipment.ShipPhone);

            // Make sure residential info matches
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                Assert.Equal(processShipmentRequest.RequestedShipment.Recipient.Address.Residential, shipment.ResidentialResult);
                Assert.True(processShipmentRequest.RequestedShipment.Recipient.Address.ResidentialSpecified);
            }
            else
            {
                Assert.False(processShipmentRequest.RequestedShipment.Recipient.Address.Residential);
                Assert.False(processShipmentRequest.RequestedShipment.Recipient.Address.ResidentialSpecified);
            }
        }

        [Fact]
        public void Manipulate_AddressAddedToSender_RequestedShipmentIsReturn()
        {
            shipment.ReturnShipment = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.Recipient);

            Assert.Equal(shipment.ShipCity, processShipmentRequest.RequestedShipment.Shipper.Address.City);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedToSecondLine_WhenSingleLineAddressIsTooLong()
        {
            shipment.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipment.ShipStreet2 = string.Empty;
            shipment.ShipStreet3 = string.Empty;

            shipment.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("1234 1234 1234 1234 1234 1234 1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedToSecondLineAtWord_WhenSingleLineAddressIsTooLong()
        {
            shipment.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234567890";
            shipment.ShipStreet2 = string.Empty;
            shipment.ShipStreet3 = string.Empty;

            shipment.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("1234 1234 1234 1234 1234 1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234567890", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsTruncated_WhenMultiLineAddressIsTooLong()
        {
            shipment.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipment.ShipStreet2 = "y";
            shipment.ShipStreet3 = string.Empty;

            shipment.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("1234 1234 1234 1234 1234 1234 1234 1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("y", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedAtThirtyChars_WhenSingleLineAddressIsTooLongAndSmartPost()
        {
            shipment.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipment.ShipStreet2 = string.Empty;
            shipment.ShipStreet3 = string.Empty;

            shipment.FedEx.Service = (int) FedExServiceType.SmartPost;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("1234 1234 1234 1234 1234 1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234 1234", processShipmentRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Theory]
        [InlineData("gu", "US")]
        [InlineData("Guam", "US")]
        [InlineData("GGG", "GU")]
        [InlineData("GGG", "guam")]
        public void Manipulate_SendingToGuamSetsStateToBlankAndCountryToGU(string state, string country)
        {
            shipment.ShipStateProvCode = state;
            shipment.ShipCountryCode = country;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(string.Empty, processShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode);
            Assert.Equal("GU", processShipmentRequest.RequestedShipment.Recipient.Address.CountryCode);
        }

        public void Dispose() => mock.Dispose();
    }
}
