using System;
using Autofac.Extras.Moq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExRateTypeManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExRateTypeManipulator testObject;
        private readonly Mock<IFedExSettingsRepository> fedExSettingsRepo;

        public FedExRateTypeManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            fedExSettingsRepo = mock.Mock<IFedExSettingsRepository>();
            fedExSettingsRepo.Setup(x => x.UseListRates)
                             .Returns(true);

            // Setup the native request to have the appropriate properties instantiated
            processShipmentRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            shipment = Create.Shipment().AsFedEx().Build();

            testObject = mock.Create<FedExRateTypeManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            fedExSettingsRepo.Verify(r => r.UseListRates, Times.Once());
        }

        [Fact]
        public void Manipulate_AddsListRate_WhenUseListRatesIsTrue()
        {
            // The mocked repository was setup to use list rates in the Initialize() method, so no additional
            // setup is required for this test
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // extract the rates from the manipulated request and test for the rate type
            RateRequestType[] rateTypes = processShipmentRequest.RequestedShipment.RateRequestTypes;

            Assert.Equal(1, rateTypes.Length);
            Assert.Equal(RateRequestType.LIST, rateTypes[0]);
        }

        [Fact]
        public void Manipulate_DoesNotAddRateRequestTypes_WhenUseListRatesIsFalse()
        {
            // Setup the the repository to return false
            fedExSettingsRepo.Setup(r => r.UseListRates).Returns(false);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // extract the rates from the manipulated request and test for the rate type
            RateRequestType[] rateTypes = processShipmentRequest.RequestedShipment.RateRequestTypes;

            Assert.Null(rateTypes);
        }
    }
}
