using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    public class FedExCloseVersionManipulatorTest
    {
        private FedExCloseVersionManipulator testObject;
        
        private Mock<CarrierRequest> groundCarrierRequest;
        private GroundCloseRequest nativeGroundCloseRequest;

        private Mock<CarrierRequest> smartPostCarrierRequest;
        private SmartPostCloseRequest nativesmartPostCloseRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeGroundCloseRequest = new GroundCloseRequest { Version = new VersionId() };
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeGroundCloseRequest);

            nativesmartPostCloseRequest = new SmartPostCloseRequest { Version = new VersionId() };
            smartPostCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativesmartPostCloseRequest);

            testObject = new FedExCloseVersionManipulator();
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(groundCarrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSmartPostCloseRequestOrGroundCloseRequest_Test()
        {
            // Setup the native request to be an unexpected type
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseReply());

            testObject.Manipulate(groundCarrierRequest.Object);
        }

        [Fact]
        public void Manipulate_SetsServiceIdToClos_ForGroundClose_Test()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("clos", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo3_ForGroundClose_Test()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(3, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForGroundClose_Test()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForGroundClose_Test()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }



        [Fact]
        public void Manipulate_SetsServiceIdToClos_ForSmartPostClose_Test()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("clos", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo3_ForSmartPostClose_Test()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(3, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForSmartPostClose_Test()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForSmartPostClose_Test()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
