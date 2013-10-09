using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators.International
{
    [TestClass]
    public class FedExTrafficInArmsManipulatorTest
    {
        private FedExTrafficInArmsManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() { SpecialServiceTypes = new ShipmentSpecialServiceType[0] }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExTrafficInArmsManipulator();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new CancelPendingShipmentRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenLicencseNumberIsProvided_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenLicencseNumberIsProvided_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [TestMethod]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenLicencseNumberIsProvided_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [TestMethod]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsEmptyString_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = string.Empty;

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [TestMethod]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsNull_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = null;

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [TestMethod]
        public void Manipulate_ArmsDetailIsNotNull_WhenLicencseNumberIsProvided_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail);
        }

        [TestMethod]
        public void Manipulate_LicenseNumberIsAssigned_WhenLicencseNumberIsProvided_Test()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("123456", nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail.LicenseOrExemptionNumber);
        }
    }
}
