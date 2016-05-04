using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateRatingServiceTest
    {
        private readonly AutoMock mock;

        public BestRateRatingServiceTest()
        {
            //Mock<IAmazonShipmentRequest> amazonShipmentRequest = mock.Mock<IAmazonShipmentRequest>();

            //Mock<IIndex<AmazonMwsApiCall, IAmazonShipmentRequest>> repo = mock.MockRepository.Create<IIndex<AmazonMwsApiCall, IAmazonShipmentRequest>>();

            //repo.Setup(x => x[AmazonMwsApiCall.CancelShipment])
            //    .Returns(amazonShipmentRequest.Object);

            //mock.Provide<IIndex<AmazonMwsApiCall, IAmazonShipmentRequest>>(repo.Object);

            //AmazonLabelService testObject = mock.Create<AmazonLabelService>();

            //ShipmentEntity shipment = new ShipmentEntity();

            //testObject.Void(shipment);

            //amazonShipmentRequest.Verify(x => x.Submit(shipment));

            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
        }

        [Fact]
        public void GetRates_AddsRatesComparedEventToShipment()
        {
            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            ShipmentEntity shipment = new ShipmentEntity {BestRateEvents = 0};

            testObject.GetRates(shipment);

            Assert.Equal((int)BestRateEventTypes.RatesCompared, shipment.BestRateEvents);
        }

        [Fact]
        public void GetRates_DoesNotRemoveOtherBestRateEvents()
        {
            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            ShipmentEntity shipment = new ShipmentEntity { BestRateEvents = (int)BestRateEventTypes.RateSelected };
            
            testObject.GetRates(shipment);

            Assert.Equal(BestRateEventTypes.RateSelected, (BestRateEventTypes)shipment.BestRateEvents & BestRateEventTypes.RateSelected);
        }

        [Fact]
        public void GetRates_ReturnsInvalidRateGroup_WhenBestRateIsHidden()
        {
            Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
            licenseService.Setup(s => s.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.BestRate)).Returns(EditionRestrictionLevel.Hidden);
            
            ShipmentEntity shipment = new ShipmentEntity { BestRateEvents = (int)BestRateEventTypes.RateSelected };

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.IsAssignableFrom<InvalidRateGroup>(rateGroup);
        }

        [Fact]
        public void GetRates_ReturnsInvalidRateGroup_WhenBestRateIsForbidden()
        {
            Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
            licenseService.Setup(s => s.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.BestRate)).Returns(EditionRestrictionLevel.Forbidden);

            ShipmentEntity shipment = new ShipmentEntity { BestRateEvents = (int)BestRateEventTypes.RateSelected };

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.IsAssignableFrom<InvalidRateGroup>(rateGroup);
        }

        [Fact]
        public void GetRates_ReturnsRateGroup_WhenFactoryCreatesZeroBrokers()
        {
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker>());
            ShipmentEntity shipment = new ShipmentEntity();

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.NotNull(rateGroup);
        }

        [Fact]
        public void GetRates_RateGroupHasShippingAccountRequiredForRatingFootnoteFactory_WhenFactoryCreatesZeroBrokers()
        {
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker>());

            ShipmentEntity shipment = new ShipmentEntity();

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.Equal(1, rateGroup.FootnoteFactories.Count());
            Assert.IsAssignableFrom<ShippingAccountRequiredForRatingFootnoteFactory>(rateGroup.FootnoteFactories.First());
        }

        [Fact]
        public void GetRates_DelegatesToBrokerFactory()
        {
            Mock<IBestRateShippingBrokerFactory> brokerFactory = mock.Mock<IBestRateShippingBrokerFactory>();
            ShipmentEntity shipment = new ShipmentEntity();

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            testObject.GetRates(shipment);

            brokerFactory.Verify(f => f.CreateBrokers(shipment), Times.Once());
        }

        [Fact]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound()
        {
            // Setup the broker to return an empty list of rate results
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(new List<RateResult>()));
            ShipmentEntity shipment = new ShipmentEntity();

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.Equal(0, rateGroup.Rates.Count);
        }

        [Fact]
        public void GetRates_AddsFootnote_WhenMultipleBrokerExceptionsAreEncountered()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            Mock<ShipmentType> shipmentType = new Mock<ShipmentType>();

            BrokerException brokerException = new BrokerException(new ShippingException("a shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);
            BrokerException anotherBrokerException = new BrokerException(new ShippingException("another shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { brokerException, anotherBrokerException });

            // We want the factory to return our fake broker for this test
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            // Create a mock of the repository so that the IIndex in the best rate rating service works
            Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
            repo.Setup(x => x[ShipmentTypeCode.BestRate])
                .Returns(shipmentType.Object);
            mock.Provide(repo.Object);

            ShipmentEntity shipment = new ShipmentEntity();

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote(null) as BrokerExceptionsRateFootnoteControl;

            Assert.NotNull(footnote);
        }

        [Fact]
        public void GetRates_AddsFootnote_WithExceptionsOrderedFromHighestToLowestSeverityLevel_WhenMultipleBrokerExceptionsAreEncountered()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            Mock<ShipmentType> shipmentType = mock.Mock<ShipmentType>();

            BrokerException informationLevelBrokerException = new BrokerException(new ShippingException("information severity level"), BrokerExceptionSeverityLevel.Information, shipmentType.Object);
            BrokerException errorLevelBrokerException = new BrokerException(new ShippingException("error severity level"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);
            BrokerException warningLevelBrokerException = new BrokerException(new ShippingException("warning severity level"), BrokerExceptionSeverityLevel.Warning, shipmentType.Object);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { informationLevelBrokerException, errorLevelBrokerException, warningLevelBrokerException });

            // We want the factory to return our fake broker for this test
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> {fakeBroker});
            
            ShipmentEntity shipment = new ShipmentEntity();
            
            // Create a mock of the repository so that the IIndex in the best rate rating service works
            Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
            repo.Setup(x => x[ShipmentTypeCode.BestRate])
                .Returns(shipmentType.Object);
            mock.Provide(repo.Object);

            BestRateRatingService testObject = mock.Create<BestRateRatingService>();
            RateGroup rateGroup = testObject.GetRates(shipment);

            // Create the footnote control and extract the exceptions
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote(null);
            List<BrokerException> exceptionsInFootnoteControl = ((BrokerExceptionsRateFootnoteControl)footnote).BrokerExceptions.ToList();

            Assert.Equal(BrokerExceptionSeverityLevel.Error, exceptionsInFootnoteControl[0].SeverityLevel);
            Assert.Equal(BrokerExceptionSeverityLevel.Warning, exceptionsInFootnoteControl[1].SeverityLevel);
            Assert.Equal(BrokerExceptionSeverityLevel.Information, exceptionsInFootnoteControl[2].SeverityLevel);
        }
    }
}