using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using VersionId = ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress.VersionId;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.GlobalShipAddress.Request.Manipulators
{
    [TestClass]
    public class FedExGlobalShipAddressVersionManipulatorTest
    {
        private FedExGlobalShipAddressVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private SearchLocationsRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new SearchLocationsRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExGlobalShipAddressVersionManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToGlobalShipAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("gsai", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo1_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(1, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
