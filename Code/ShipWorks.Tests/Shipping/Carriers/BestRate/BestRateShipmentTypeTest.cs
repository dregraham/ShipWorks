using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRateShipmentTypeTest
    {
        private BestRateShipmentType testObject;

        private Mock<IBestRateShippingBrokerFactory> brokerFactory;
        private Mock<IBestRateShippingBroker> broker;
        private Mock<ILog> log;

        private List<RateResult> rates;
        private ShipmentEntity shipment;

        private RateGroup rateGroupWithNoFootnote;
        private RateGroup rateGroupWithFooterWithAssociatedAmount;
        private RateGroup rateGroupWithFooterNotAssociatedWithAmount;

        private Mock<IRateFootnoteFactory> associatedWithAmountFooterFootnoteFactory;
        private Mock<IRateFootnoteFactory> notAssociatedWithAmountFooterFootnoteFactory;

        private Mock<IRateGroupFilterFactory> filterFactory;

        public BestRateShipmentTypeTest()
        {
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "5", 4.23M),
                CreateRateResult("Rate 123", "4", 6.23M)
            };

            shipment = new ShipmentEntity
            {
                BestRate = new BestRateShipmentEntity()
            };

            broker = new Mock<IBestRateShippingBroker>();
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            brokerFactory = new Mock<IBestRateShippingBrokerFactory>();
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            filterFactory = new Mock<IRateGroupFilterFactory>();
            filterFactory.Setup(f => f.CreateFilters(It.IsAny<ShipmentEntity>())).Returns(new List<IRateGroupFilter>());

            log = new Mock<ILog>();

            testObject = new BestRateShipmentType(brokerFactory.Object, filterFactory.Object, log.Object);

            InitializeFootnoteTests();
        }

        [Fact]
        public void GetRates_DelegatesToBrokerFactory_Test()
        {
            testObject.GetRates(shipment);

            brokerFactory.Verify(f => f.CreateBrokers(shipment, It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public void GetRates_DelegatesToEachBroker_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            testObject.GetRates(shipment);

            broker.Verify(b => b.GetBestRates(shipment, It.IsAny<List<BrokerException>>()), Times.Once());
            secondBroker.Verify(b => b.GetBestRates(shipment, It.IsAny<List<BrokerException>>()), Times.Once());
        }

        [Fact]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound_Test()
        {
            // Setup the broker to return an empty list of rate results
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(new List<RateResult>()));

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.Equal(0, rateGroup.Rates.Count());
        }

        [Fact]
        public void GetRates_RateAmountFromBrokerIsUnmodified_Test()
        {
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "5", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "4", 9.87M, "SomeRateResult2")
            };
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.Equal(4.23M, bestRates[0].Amount);
        }

        [Fact]
        public void GetRates_RateDaysFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2")
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.Equal("12", bestRates[0].Days);
        }

        [Fact]
        public void GetRates_RateTagFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2")
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            BestRateResultTag bestRateResultTag = (BestRateResultTag)bestRates.First(rr => ((BestRateResultTag)rr.Tag).ResultKey == "SomeRateResult").Tag;

            Assert.Equal(rates[0].Tag, bestRateResultTag);
        }

        [Fact]
        public void GetRates_RateSelectableFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2"),
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.Equal(rates[0].Selectable, bestRates[0].Selectable);
        }

        [Fact]
        public void GetRates_CallsMaskDescription_OnReturnedRates_Test()
        {
            var testRate = new Mock<RateResult>();
            testRate.Object.Tag = new BestRateResultTag() { ResultKey = "SomeKey" };

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                testRate.Object
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            testObject.GetRates(shipment);

            testRate.Verify(x => x.MaskDescription(rates));
        }

        [Fact]
        public void GetRates_DelegatesToFilterFactory_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2"),
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            testObject.GetRates(shipment);

            filterFactory.Verify(f => f.CreateFilters(shipment), Times.Once());
        }

        [Fact]
        public void GetRates_UsesEachFilter_Test()
        {
            Mock<IRateGroupFilter> firstFilter = new Mock<IRateGroupFilter>();
            firstFilter.Setup(f => f.Filter(It.IsAny<RateGroup>())).Returns((RateGroup group) => group);

            Mock<IRateGroupFilter> secondFilter = new Mock<IRateGroupFilter>();
            secondFilter.Setup(f => f.Filter(It.IsAny<RateGroup>())).Returns((RateGroup group) => group);

            filterFactory.Setup(f => f.CreateFilters(It.IsAny<ShipmentEntity>())).Returns(new List<IRateGroupFilter> { firstFilter.Object, secondFilter.Object });

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2"),
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            testObject.GetRates(shipment);

            firstFilter.Verify(f => f.Filter(It.IsAny<RateGroup>()), Times.Once());
            secondFilter.Verify(f => f.Filter(It.IsAny<RateGroup>()), Times.Once());

        }

        [Fact]
        public void GetRates_AddsFootnote_WhenMultipleBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            Mock<ShipmentType> shipmentType = new Mock<ShipmentType>();

            BrokerException brokerException = new BrokerException(new ShippingException("a shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);
            BrokerException anotherBrokerException = new BrokerException(new ShippingException("another shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { brokerException, anotherBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote(null) as BrokerExceptionsRateFootnoteControl;

            Assert.NotNull(footnote);
        }

        [Fact]
        public void GetRates_AddsFootnote_WithExceptionsOrderedFromHighestToLowestSeverityLevel_WhenMultipleBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            Mock<ShipmentType> shipmentType = new Mock<ShipmentType>();

            BrokerException informationLevelBrokerException = new BrokerException(new ShippingException("information severity level"), BrokerExceptionSeverityLevel.Information, shipmentType.Object);
            BrokerException errorLevelBrokerException = new BrokerException(new ShippingException("error severity level"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);
            BrokerException warningLevelBrokerException = new BrokerException(new ShippingException("warning severity level"), BrokerExceptionSeverityLevel.Warning, shipmentType.Object);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { informationLevelBrokerException, errorLevelBrokerException, warningLevelBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Create the footnote control and extract the exceptions
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote(null);
            List<BrokerException> exceptionsInFootnoteControl = ((BrokerExceptionsRateFootnoteControl)footnote).BrokerExceptions.ToList();

            Assert.Equal(BrokerExceptionSeverityLevel.Error, exceptionsInFootnoteControl[0].SeverityLevel);
            Assert.Equal(BrokerExceptionSeverityLevel.Warning, exceptionsInFootnoteControl[1].SeverityLevel);
            Assert.Equal(BrokerExceptionSeverityLevel.Information, exceptionsInFootnoteControl[2].SeverityLevel);
        }

        [Fact]
        public void GetRates_AddsRatesComparedEventToShipment_Test()
        {
            shipment.BestRateEvents = 0;
            testObject.GetRates(shipment);

            Assert.Equal((int)BestRateEventTypes.RatesCompared, shipment.BestRateEvents);
        }

        [Fact]
        public void GetRates_DoesNotRemoveOtherBestRateEvents_Test()
        {
            shipment.BestRateEvents = (int)BestRateEventTypes.RateSelected;
            testObject.GetRates(shipment);

            Assert.Equal(BestRateEventTypes.RateSelected, (BestRateEventTypes)shipment.BestRateEvents & BestRateEventTypes.RateSelected);
        }

        [Fact]
        public void GetRates_ReturnsRateGroup_WhenFactoryCreatesZeroBrokers_Test()
        {
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker>());

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.NotNull(rateGroup);
        }

        [Fact]
        public void GetRates_RateGroupHasExceptionFootnoteFactory_WhenFactoryCreatesZeroBrokers_Test()
        {
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker>());

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.Equal(1, rateGroup.FootnoteFactories.Count());
            Assert.IsAssignableFrom<ExceptionsRateFootnoteFactory>(rateGroup.FootnoteFactories.First());
        }

        [Fact]
        public void SupportsGetRates_ReturnsTrue_Test()
        {
            Assert.True(testObject.SupportsGetRates);
        }

        [Fact]
        public void ProcessShipment_ThrowsInvalidOperationException_Test()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.ProcessShipment(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_OneBrokersWithNoAccounts_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(false);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);

            Assert.Equal(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsShipWorks_TwoBrokersWithAccountsAndShipWorksInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });

            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });

            Assert.Equal(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsCarrier_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker.Object });

            Assert.Equal(InsuranceProvider.Carrier, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void ApplySelectedShipmentRate_AddsRateSelectedEventToShipment_Test()
        {
            shipment.BestRateEvents = 0;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            testObject.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal((int)BestRateEventTypes.RateSelected, shipment.BestRateEvents);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotRemoveOtherBestRateEvents_Test()
        {
            shipment.BestRateEvents = (int)BestRateEventTypes.RatesCompared;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            testObject.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(BestRateEventTypes.RatesCompared, (BestRateEventTypes)shipment.BestRateEvents & BestRateEventTypes.RatesCompared);
        }

        [Fact]
        public void ApplySelectedShipmentRate_CallsSelectActionSetOnTag_Test()
        {
            ShipmentEntity calledShipment = null;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            testObject.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(shipment, calledShipment);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotCallSignUpActionOnTag_WhenSignUpActionIsNull_Test()
        {
            ShipmentEntity calledShipment = null;
            bool? signUpActionResult = null;

            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            testObject.ApplySelectedShipmentRate(shipment, rate);

            Assert.False(signUpActionResult.HasValue);
        }

        private void InitializeFootnoteTests()
        {
            associatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            associatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote(null)).Returns(new FakeRateFootnoteControl(true));

            rateGroupWithNoFootnote = new RateGroup(new List<RateResult> { new RateResult("result1", "2"), new RateResult("result2", "2") });

            rateGroupWithFooterWithAssociatedAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { AmountFootnote = new Bitmap(1, 1), Tag = new BestRateResultTag() { ResultKey = "result1"}},
                new RateResult("result2", "2") {Tag = new BestRateResultTag() { ResultKey = "result2"}}
            });
            rateGroupWithFooterWithAssociatedAmount.AddFootnoteFactory(associatedWithAmountFooterFootnoteFactory.Object);


            notAssociatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            notAssociatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote(null)).Returns(new FakeRateFootnoteControl(false));

            rateGroupWithFooterNotAssociatedWithAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { Tag = new BestRateResultTag() { ResultKey = "result1"} },
                new RateResult("result2", "2") { Tag = new BestRateResultTag() { ResultKey = "result2"} }
            });
            rateGroupWithFooterNotAssociatedWithAmount.AddFootnoteFactory(notAssociatedWithAmountFooterFootnoteFactory.Object);
        }

        // IsCustomsRequierd has a hard dependency to the database, so these are no longer testable 
        // until that dependency is abstracted away
        //[Fact]
        //public void IsCustomsRequired_ReturnsFalse_WhenSingleBrokerDoesNotRequireCustoms_Test()
        //{
        //    broker = new Mock<IBestRateShippingBroker>();
        //    broker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

        //    brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker.Object });

        //    bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
        //    Assert.False(isRequired);
        //}

        //[Fact]
        //public void IsCustomsRequired_ReturnsFalse_WhenSingleBrokerRequiresCustoms_Test()
        //{
        //    broker = new Mock<IBestRateShippingBroker>();
        //    broker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

        //    brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker.Object });

        //    bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
        //    Assert.True(isRequired);
        //}

        //[Fact]
        //public void IsCustomsRequired_ReturnsFalse_WithMultipleBrokers_WhenNoBrokersRequireCustoms_Test()
        //{
        //    Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
        //    firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

        //    Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
        //    secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

        //    brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

        //    bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
        //    Assert.False(isRequired);
        //}

        //[Fact]
        //public void IsCustomsRequired_ReturnsTrue_WithMultipleBrokers_WhenOneBrokerRequireCustoms_Test()
        //{
        //    Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
        //    firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

        //    Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
        //    secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

        //    brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

        //    bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
        //    Assert.True(isRequired);
        //}

        //[Fact]
        //public void IsCustomsRequired_ReturnsTrue_WithMultipleBrokers_WhenAllBrokerRequireCustoms_Test()
        //{
        //    Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
        //    firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

        //    Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
        //    secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

        //    brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

        //    bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
        //    Assert.True(isRequired);
        //}

        // Helper methods for creating rate results
        private RateResult CreateRateResult(string description, string days, decimal amount)
        {
            return CreateRateResult(description, days, amount, description);
        }

        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey)
        {
            return new RateResult(description, days, amount, new BestRateResultTag() { ResultKey = tagResultKey });
        }
    }
}
