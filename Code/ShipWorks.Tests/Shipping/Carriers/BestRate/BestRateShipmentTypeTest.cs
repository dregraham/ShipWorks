using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRateShipmentTypeTest
    {
        private BestRateLabelService labelService;
        
        private ShipmentEntity shipment;
        
        private RateGroup rateGroupWithFooterWithAssociatedAmount;
        private RateGroup rateGroupWithFooterNotAssociatedWithAmount;

        private Mock<IRateFootnoteFactory> associatedWithAmountFooterFootnoteFactory;
        private Mock<IRateFootnoteFactory> notAssociatedWithAmountFooterFootnoteFactory;

        private Mock<IRateGroupFilterFactory> filterFactory;
        private AutoMock mock;


        public BestRateShipmentTypeTest()
        {
            shipment = new ShipmentEntity
            {
                BestRate = new BestRateShipmentEntity()
            };

            //broker = new Mock<IBestRateShippingBroker>();
            //broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<List<BrokerException>>())).Returns(new RateGroup(rates));

            //brokerFactory = new Mock<IBestRateShippingBrokerFactory>();
           // brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), It.IsAny<bool>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            filterFactory = new Mock<IRateGroupFilterFactory>();
            filterFactory.Setup(f => f.CreateFilters(It.IsAny<ShipmentEntity>())).Returns(new List<IRateGroupFilter>());
            
            //testObject = new BestRateShipmentType(brokerFactory.Object, log.Object,);

            labelService = new BestRateLabelService();


            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            InitializeFootnoteTests();
        }

        [Fact]
            // Setup the factory to return two brokers - the one already defined at the class level 
        public void GetShipmentInsuranceProvider_ReturnsInvalid_OneBrokersWithNoAccounts_Test()
        {
            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Invalid, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void SupportsGetRates_ReturnsTrue_Test()
        {
            BestRateShipmentType bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.True(bestRateShipmentType.SupportsGetRates);
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsShipWorks_TwoBrokersWithAccountsAndShipWorksInsurance_Test()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);
            IBestRateShippingBroker broker = mock.Create<IBestRateShippingBroker>();
            
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker, broker });
            
            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.ShipWorks, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);
            IBestRateShippingBroker broker = mock.Create<IBestRateShippingBroker>();
            
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker, broker });

            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Invalid, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsCarrier_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);
            IBestRateShippingBroker broker = mock.Create<IBestRateShippingBroker>();

            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>(), false)).Returns(new List<IBestRateShippingBroker> { broker });

            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Carrier, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void ProcessShipment_ThrowsInvalidOperationException_Test()
        {
            Assert.Throws<InvalidOperationException>(() => labelService.Create(new ShipmentEntity()));
        }

        [Fact]
        public void ApplySelectedShipmentRate_AddsRateSelectedEventToShipment_Test()
        {
            shipment.BestRateEvents = 0;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal((int) BestRateEventTypes.RateSelected, shipment.BestRateEvents);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotRemoveOtherBestRateEvents_Test()
        {
            shipment.BestRateEvents = (int) BestRateEventTypes.RatesCompared;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(BestRateEventTypes.RatesCompared, (BestRateEventTypes) shipment.BestRateEvents & BestRateEventTypes.RatesCompared);
        }

        [Fact]
        public void ApplySelectedShipmentRate_CallsSelectActionSetOnTag_Test()
        {
            ShipmentEntity calledShipment = null;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(shipment, calledShipment);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotCallSignUpActionOnTag_WhenSignUpActionIsNull_Test()
        {
            ShipmentEntity calledShipment = null;
            bool? signUpActionResult = null;

            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.False(signUpActionResult.HasValue);
        }

        private void InitializeFootnoteTests()
        {
            associatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            associatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote(null)).Returns(new FakeRateFootnoteControl(true));
            
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
        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey)
        {
            return new RateResult(description, days, amount, new BestRateResultTag() { ResultKey = tagResultKey });
        }
    }
}
