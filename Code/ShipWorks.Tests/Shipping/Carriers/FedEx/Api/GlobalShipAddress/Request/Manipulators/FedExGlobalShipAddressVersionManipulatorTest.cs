using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using VersionId = ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress.VersionId;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressVersionManipulatorTest
    {
        private FedExGlobalShipAddressVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private SearchLocationsRequest nativeRequest;

        public FedExGlobalShipAddressVersionManipulatorTest()
        {
            nativeRequest = new SearchLocationsRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExGlobalShipAddressVersionManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToGlobalShipAddress()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("gsai", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo1()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(2, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((SearchLocationsRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
