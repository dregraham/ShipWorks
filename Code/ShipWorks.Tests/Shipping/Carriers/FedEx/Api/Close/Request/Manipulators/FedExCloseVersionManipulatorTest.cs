using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    public class FedExCloseVersionManipulatorTest
    {
        private FedExCloseVersionManipulator testObject;

        private Mock<CarrierRequest> groundCarrierRequest;
        private GroundCloseRequest nativeGroundCloseRequest;

        private Mock<CarrierRequest> smartPostCarrierRequest;
        private SmartPostCloseRequest nativesmartPostCloseRequest;

        public FedExCloseVersionManipulatorTest()
        {
            nativeGroundCloseRequest = new GroundCloseRequest { Version = new VersionId() };
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeGroundCloseRequest);

            nativesmartPostCloseRequest = new SmartPostCloseRequest { Version = new VersionId() };
            smartPostCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativesmartPostCloseRequest);

            testObject = new FedExCloseVersionManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(groundCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSmartPostCloseRequestOrGroundCloseRequest()
        {
            // Setup the native request to be an unexpected type
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(groundCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToClos_ForGroundClose()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("clos", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo5_ForGroundClose()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(5, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForGroundClose()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForGroundClose()
        {
            testObject.Manipulate(groundCarrierRequest.Object);

            VersionId version = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }



        [Fact]
        public void Manipulate_SetsServiceIdToClos_ForSmartPostClose()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("clos", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo5_ForSmartPostClose()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(5, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForSmartPostClose()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForSmartPostClose()
        {
            testObject.Manipulate(smartPostCarrierRequest.Object);

            VersionId version = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
