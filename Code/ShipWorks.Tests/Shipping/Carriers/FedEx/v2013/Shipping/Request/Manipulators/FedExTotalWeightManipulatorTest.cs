using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Api;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Interapptive.Shared.Enums;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExTotalWeightManipulatorTest
    {
        FedExTotalWeightManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                TotalWeight = 3.245f,
                FedEx = new FedExShipmentEntity { WeightUnitType = (int) WeightUnitOfMeasure.Pounds }
            };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExTotalWeightManipulator();
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
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_SetsUnitsToPounds_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.AreEqual(WeightUnits.LB, weight.Units);
        }

        [TestMethod]
        public void Manipulate_SetsUnitsToKilograms_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.AreEqual(WeightUnits.KG, weight.Units);
        }

        [TestMethod]
        public void Manipulate_SetsWeightValue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.AreEqual((decimal)shipmentEntity.TotalWeight, weight.Value);
        }
    }
}
