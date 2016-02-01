using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExRecipientManipulatorTest
    {
        private Mock<CarrierRequest> carrierRequest;

        private ProcessShipmentRequest nativeRequest;

        private ShipmentEntity shipmentEntity;

        private FedExRecipientManipulator testObject;

        public FedExRecipientManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRecipientManipulator();
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
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

            // Make sure Contact fields match
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Contact.CompanyName, shipmentEntity.ShipCompany);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Contact.EMailAddress, shipmentEntity.ShipEmail);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Contact.PhoneNumber, shipmentEntity.ShipPhone);

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

        [Fact]
        public void Manipulate_AddressAddedToSender_RequestedShipmentIsReturn()
        {
            shipmentEntity.ReturnShipment = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.Recipient);

            Assert.Equal(shipmentEntity.ShipCity, nativeRequest.RequestedShipment.Shipper.Address.City);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedToSecondLine_WhenSingleLineAddressIsTooLong()
        {
            shipmentEntity.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipmentEntity.ShipStreet2 = string.Empty;
            shipmentEntity.ShipStreet3 = string.Empty;

            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("1234 1234 1234 1234 1234 1234 1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedToSecondLineAtWord_WhenSingleLineAddressIsTooLong()
        {
            shipmentEntity.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234567890";
            shipmentEntity.ShipStreet2 = string.Empty;
            shipmentEntity.ShipStreet3 = string.Empty;

            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("1234 1234 1234 1234 1234 1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234567890", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsTruncated_WhenMultiLineAddressIsTooLong()
        {
            shipmentEntity.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipmentEntity.ShipStreet2 = "y";
            shipmentEntity.ShipStreet3 = string.Empty;

            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("1234 1234 1234 1234 1234 1234 1234 1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("y", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddressIsWrappedAtThirtyChars_WhenSingleLineAddressIsTooLongAndSmartPost()
        {
            shipmentEntity.ShipStreet1 = "1234 1234 1234 1234 1234 1234 1234 1234";
            shipmentEntity.ShipStreet2 = string.Empty;
            shipmentEntity.ShipStreet3 = string.Empty;

            shipmentEntity.FedEx.Service = (int)FedExServiceType.SmartPost;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("1234 1234 1234 1234 1234 1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[0]);
            Assert.Equal("1234 1234", nativeRequest.RequestedShipment.Recipient.Address.StreetLines[1]);
        }
    }
}
