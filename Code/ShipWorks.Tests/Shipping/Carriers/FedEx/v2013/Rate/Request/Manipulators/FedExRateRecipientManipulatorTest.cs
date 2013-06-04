﻿using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRateRecipientManipulatorTest
    {
        private FedExRateRecipientManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateRecipientManipulator();
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
    }
}
