using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExRecipientManipulatorTest
    {
        private Mock<CarrierRequest> carrierRequest;

        private ProcessShipmentRequest nativeRequest;

        private ShipmentEntity shipmentEntity;

        private FedExRecipientManipulator testObject;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRecipientManipulator();
        }

        [TestMethod]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsInstanceOfType(nativeRequest.RequestedShipment, typeof(RequestedShipment));
        }

        [TestMethod]
        public void Manipulate_FedExRecipientManipulator_ReturnsValidRequestedShipmentRecipient_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Recipient back
            Assert.IsInstanceOfType(nativeRequest.RequestedShipment.Recipient, typeof(Party));

            // Make sure the Address matches what we input
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.City, shipmentEntity.ShipCity);
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.CountryCode, shipmentEntity.ShipCountryCode);
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.PostalCode, shipmentEntity.ShipPostalCode);
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode, shipmentEntity.ShipStateProvCode);

            // Make sure Contact fields match
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Contact.CompanyName, shipmentEntity.ShipCompany);
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Contact.EMailAddress, shipmentEntity.ShipEmail);
            Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Contact.PhoneNumber, shipmentEntity.ShipPhone);

            // Make sure residential info matches
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipmentEntity))
            {
                Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.Residential, shipmentEntity.ResidentialResult);
                Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, true);
            }
            else
            {
                Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.Residential, false);
                Assert.AreEqual(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, false);
            }
        }

        [TestMethod]
        public void Manipulate_AddressAddedToSender_RequestedShipmentIsReturn_Test()
        {
            shipmentEntity.ReturnShipment = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.Recipient);

            Assert.AreEqual(shipmentEntity.ShipCity, nativeRequest.RequestedShipment.Shipper.Address.City);
        }
    }
}
