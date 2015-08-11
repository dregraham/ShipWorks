using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExRateTypeManipulatorTest
    {
        private FedExRateTypeManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;

        public FedExRateTypeManipulatorTest()
        {
            // Default the repository to indicate we should use list rates
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.UseListRates).Returns(true);

            // Setup the native request to have the appropriate properties instantiated
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Use the native request to create the carrier request based
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExRateTypeManipulator(settingsRepository.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            settingsRepository.Verify(r => r.UseListRates, Times.Once());
        }

        [Fact]
        public void Manipulate_AddsListRate_WhenUseListRatesIsTrue_Test()
        {
            // The mocked repository was setup to use list rates in the Initialize() method, so no additional
            // setup is required for this test
            testObject.Manipulate(carrierRequest.Object);

            // extract the rates from the manipulated request and test for the rate type
            RateRequestType[] rateTypes = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.RateRequestTypes;

            Assert.Equal(1, rateTypes.Length);
            Assert.Equal(RateRequestType.LIST, rateTypes[0]);
        }

        [Fact]
        public void Manipulate_DoesNotAddRateRequestTypes_WhenUseListRatesIsFalse_Test()
        {
            // Setup gthe the repository to return false
            settingsRepository.Setup(r => r.UseListRates).Returns(false);

            testObject.Manipulate(carrierRequest.Object);

            // extract the rates from the manipulated request and test for the rate type
            RateRequestType[] rateTypes = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.RateRequestTypes;

            Assert.Null(rateTypes);
        }
    }
}
