using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.PackageMovement.Request.Manipulators
{
    [TestClass]
    public class FedExPackageMovementVersionManipulatorTest
    {

        private FedExPackageMovementVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private PostalCodeInquiryRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new PostalCodeInquiryRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExPackageMovementVersionManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ServiceAvailabilityRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToPmis_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("pmis", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo5_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(5, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
