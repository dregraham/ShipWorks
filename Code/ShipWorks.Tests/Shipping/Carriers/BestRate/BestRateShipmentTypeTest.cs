using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
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

        [TestInitialize]
        public void Initialize()
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
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            brokerFactory = new Mock<IBestRateShippingBrokerFactory>();
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            log = new Mock<ILog>();

            testObject = new BestRateShipmentType(brokerFactory.Object, log.Object);

            InitializeFootnoteTests();
        }

        [TestMethod]
        public void GetRates_DelegatesToBrokerFactory_Test()
        {
            testObject.GetRates(shipment);

            brokerFactory.Verify(f => f.CreateBrokers(shipment), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToEachBroker_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            testObject.GetRates(shipment);

            broker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<BrokerException>>()), Times.Once());
            secondBroker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<BrokerException>>()), Times.Once());
        }

        [TestMethod]
        public void GetRates_AddsRates_FromAllBrokers_Test()
        {
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "5", 4.23M, "SomeRateResult3"),
                CreateRateResult("Rate 123", "4", 6.23M, "SomeRateResult4")
            };

            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup( rates));

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Both brokers are setup to return two rate results, so we should have a total of four
            Assert.AreEqual(4, rateGroup.Rates.Count());
        }

        [TestMethod]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound_Test()
        {
            // Setup the broker to return an empty list of rate results
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(new List<RateResult>()));

            RateGroup rateGroup = testObject.GetRates(shipment);
            
            Assert.AreEqual(0, rateGroup.Rates.Count());
        }
        
        [TestMethod]
        public void GetRates_RateAmountFromBrokerIsUnmodified_Test()
        {
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "5", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "4", 9.87M, "SomeRateResult2")
            };
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(4.23M, bestRates[0].Amount);
            Assert.AreEqual(9.87M, bestRates[1].Amount);
        }

        [TestMethod]
        public void GetRates_RateDaysFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2")
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual("12", bestRates[0].Days);
            Assert.AreEqual("probably 7", bestRates[1].Days);
        }

        [TestMethod]
        public void GetRates_RateTagFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2")
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            BestRateResultTag bestRateResultTag = (BestRateResultTag)bestRates.First(rr => ((BestRateResultTag)rr.Tag).ResultKey == "SomeRateResult").Tag;

            Assert.AreEqual(rates[0].Tag, bestRateResultTag);
            Assert.IsNotNull(bestRates[1].Tag as BestRateResultTag);
        }

        [TestMethod]
        public void GetRates_RateSelectableFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult"),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult2"),
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates[0].Selectable, bestRates[0].Selectable);
            Assert.AreEqual(rates[1].Selectable, bestRates[1].Selectable);
        }

        [TestMethod]
        public void GetRates_RatesAreOrderedFromCheapestToMostExpensive_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "12", 34.30M, "SomeRateResult", ServiceLevelType.Anytime),
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult2", ServiceLevelType.Anytime),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult3", ServiceLevelType.Anytime)
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();
            
            Assert.AreEqual(rates[1], bestRates[0]);
            Assert.AreEqual(rates[2], bestRates[1]);
            Assert.AreEqual(rates[0], bestRates[2]);
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_AreOrderedByServiceLevel_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult3", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult4", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult5", ServiceLevelType.TwoDays ),                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates[2], bestRates[0]);
            Assert.AreEqual(rates[4], bestRates[1]);
            Assert.AreEqual(rates[0], bestRates[2]);
            Assert.AreEqual(rates[3], bestRates[3]);
            Assert.AreEqual(rates[1], bestRates[4]);
        }



        [TestMethod]
        public void GetRates_ReturnsFirstFiveRates_WhenMoreThanFiveRatesAreAvailable_Test()
        {

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult2", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult3", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult4", ServiceLevelType.TwoDays ),  

                // These are the rates that should be returned
                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.TwoDays ),                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates[6], bestRates[0]);
            Assert.AreEqual(rates[8], bestRates[1]);
            Assert.AreEqual(rates[4], bestRates[2]);
            Assert.AreEqual(rates[7], bestRates[3]);
            Assert.AreEqual(rates[5], bestRates[4]);
        }


        [TestMethod]
        public void GetRates_ReturnsAllRates_WhenLessThanFiveRatesAreAvailable_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult3", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult4", ServiceLevelType.TwoDays ),                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates.Count, bestRates.Count);
        }


        [TestMethod]
        public void GetRates_CallsMaskDescription_OnReturnedRates()
        {
            var testRate = new Mock<RateResult>();
            testRate.Object.Tag = new BestRateResultTag() { ResultKey = "SomeKey"};

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                testRate.Object
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            testObject.GetRates(shipment);

            testRate.Verify(x => x.MaskDescription(rates));
        }

        [TestMethod]
        public void GetRates_ReturnsOneAndTwoDayRates_When2DaysAreSpecifiedAndExpectedDateIsNull_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.88M, "SomeRateResult2", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.89M, "SomeRateResult3", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.90M, "SomeRateResult4", ServiceLevelType.TwoDays ),  

                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.Anytime ),                  
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            shipment.BestRate.ServiceLevel = (int)ServiceLevelType.TwoDays;

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();
             
            Assert.AreEqual(rates[6], bestRates[0]);
            Assert.AreEqual(rates[0], bestRates[1]);
            Assert.AreEqual(rates[1], bestRates[2]);
            Assert.AreEqual(rates[2], bestRates[3]);
            Assert.AreEqual(rates[3], bestRates[4]);
        }

        [TestMethod]
        public void GetRates_ReturnsTwoDayAnd4DayRates_When2DaysAreSpecifiedAndA2DayServiceArivesAfter4DayService_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays, DateTime.Today.AddDays(3) ),  
                CreateRateResult("Rate 789", "2", 6.88M, "SomeRateResult2", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.89M, "SomeRateResult3", ServiceLevelType.TwoDays ),  
                CreateRateResult("Rate 789", "2", 6.90M, "SomeRateResult4", ServiceLevelType.TwoDays ),  

                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", .23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays, DateTime.Today.AddDays(3)),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.Anytime ),                  
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

            shipment.BestRate.ServiceLevel = (int)ServiceLevelType.TwoDays;

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates[7], bestRates[0]);
            Assert.AreEqual(rates[6], bestRates[1]);
            Assert.AreEqual(rates[0], bestRates[2]);
            Assert.AreEqual(rates[1], bestRates[3]);
            Assert.AreEqual(rates[2], bestRates[4]);
        }
        
        [TestMethod]
        public void GetRates_AddsFootnote_WhenMultipleBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            Mock<ShipmentType> shipmentType = new Mock<ShipmentType>();

            BrokerException brokerException = new BrokerException(new ShippingException("a shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);
            BrokerException anotherBrokerException = new BrokerException(new ShippingException("another shipping exception"), BrokerExceptionSeverityLevel.Error, shipmentType.Object);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { brokerException, anotherBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote() as BrokerExceptionsRateFootnoteControl;

            Assert.IsNotNull(footnote);
        }

        [TestMethod]
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
            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Create the footnote control and extract the exceptions
            RateFootnoteControl footnote = rateGroup.FootnoteFactories.First().CreateFootnote();
            List<BrokerException> exceptionsInFootnoteControl = ((BrokerExceptionsRateFootnoteControl)footnote).BrokerExceptions.ToList();
            
            Assert.AreEqual(BrokerExceptionSeverityLevel.Error, exceptionsInFootnoteControl[0].SeverityLevel);
            Assert.AreEqual(BrokerExceptionSeverityLevel.Warning, exceptionsInFootnoteControl[1].SeverityLevel);
            Assert.AreEqual(BrokerExceptionSeverityLevel.Information, exceptionsInFootnoteControl[2].SeverityLevel);
        }

        [TestMethod]
        public void GetRates_AddsRatesComparedEventToShipment_Test()
        {
            shipment.BestRateEvents = 0;
            testObject.GetRates(shipment);

            Assert.AreEqual((int)BestRateEventTypes.RatesCompared, shipment.BestRateEvents);
        }

        [TestMethod]
        public void GetRates_DoesNotRemoveOtherBestRateEvents_Test()
        {
            shipment.BestRateEvents = (int)BestRateEventTypes.RateSelected;
            testObject.GetRates(shipment);

            Assert.AreEqual(BestRateEventTypes.RateSelected, (BestRateEventTypes)shipment.BestRateEvents & BestRateEventTypes.RateSelected);
        }

        [TestMethod]
        public void SupportsGetRates_ReturnsTrue_Test()
        {
            Assert.IsTrue(testObject.SupportsGetRates);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProcessShipment_ThrowsInvalidOperationException_Test()
        {
            testObject.ProcessShipment(new ShipmentEntity());
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_OneBrokersWithNoAccounts_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(false);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);

            Assert.AreEqual(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsShipWorks_TwoBrokersWithAccountsAndShipWorksInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });

            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });

            Assert.AreEqual(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsCarrier_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts).Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            Assert.AreEqual(InsuranceProvider.Carrier, testObject.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }
		
        [TestMethod]
        public void ApplySelectedShipmentRate_AddsRateSelectedEventToShipment_Test()
        {
            shipment.BestRateEvents = 0;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.AreEqual((int)BestRateEventTypes.RateSelected, shipment.BestRateEvents); 
        }

        [TestMethod]
        public void ApplySelectedShipmentRate_DoesNotRemoveOtherBestRateEvents_Test()
        {
            shipment.BestRateEvents = (int)BestRateEventTypes.RatesCompared;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.AreEqual(BestRateEventTypes.RatesCompared, (BestRateEventTypes)shipment.BestRateEvents & BestRateEventTypes.RatesCompared);
        }

        [TestMethod]
        public void ApplySelectedShipmentRate_CallsActionSetOnTag_Test()
        {
            ShipmentEntity calledShipment = null;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.AreEqual(shipment, calledShipment);
        }

        private void InitializeFootnoteTests()
        {
            associatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            associatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote()).Returns(new FakeRateFootnoteControl(true));

            rateGroupWithNoFootnote = new RateGroup(new List<RateResult> { new RateResult("result1", "2"), new RateResult("result2", "2") });

            rateGroupWithFooterWithAssociatedAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { AmountFootnote = new Bitmap(1, 1), Tag = new BestRateResultTag() { ResultKey = "result1"}},
                new RateResult("result2", "2") {Tag = new BestRateResultTag() { ResultKey = "result2"}}
            });
            rateGroupWithFooterWithAssociatedAmount.AddFootnoteFactory(associatedWithAmountFooterFootnoteFactory.Object);


            notAssociatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            notAssociatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote()).Returns(new FakeRateFootnoteControl(false));

            rateGroupWithFooterNotAssociatedWithAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { Tag = new BestRateResultTag() { ResultKey = "result1"} }, 
                new RateResult("result2", "2") { Tag = new BestRateResultTag() { ResultKey = "result2"} }
            });
            rateGroupWithFooterNotAssociatedWithAmount.AddFootnoteFactory(notAssociatedWithAmountFooterFootnoteFactory.Object);
        }

        [TestMethod]
        public void IsCustomsRequired_ReturnsFalse_WhenSingleBrokerDoesNotRequireCustoms_Test()
        {
            broker = new Mock<IBestRateShippingBroker>();
            broker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
            Assert.IsFalse(isRequired);
        }

        [TestMethod]
        public void IsCustomsRequired_ReturnsFalse_WhenSingleBrokerRequiresCustoms_Test()
        {
            broker = new Mock<IBestRateShippingBroker>();
            broker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker.Object });

            bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
            Assert.IsTrue(isRequired);
        }

        [TestMethod]
        public void IsCustomsRequired_ReturnsFalse_WithMultipleBrokers_WhenNoBrokersRequireCustoms_Test()
        {
            Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
            firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

            bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
            Assert.IsFalse(isRequired);
        }

        [TestMethod]
        public void IsCustomsRequired_ReturnsTrue_WithMultipleBrokers_WhenOneBrokerRequireCustoms_Test()
        {
            Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
            firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(false);

            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

            bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
            Assert.IsTrue(isRequired);
        }

        [TestMethod]
        public void IsCustomsRequired_ReturnsTrue_WithMultipleBrokers_WhenAllBrokerRequireCustoms_Test()
        {
            Mock<IBestRateShippingBroker> firstBroker = new Mock<IBestRateShippingBroker>();
            firstBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.IsCustomsRequired(It.IsAny<ShipmentEntity>())).Returns(true);

            brokerFactory.Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { firstBroker.Object, secondBroker.Object });

            bool isRequired = testObject.IsCustomsRequired(new ShipmentEntity());
            Assert.IsTrue(isRequired);
        }

        // Helper methods for creating rate results
        private RateResult CreateRateResult(string description, string days, decimal amount)
        {
            return CreateRateResult(description, days, amount, description);
        }

        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey)
        {
            return new RateResult(description, days, amount, new BestRateResultTag() { ResultKey = tagResultKey });
        }

        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey, ServiceLevelType serviceLevel)
        {
            RateResult rateResult = CreateRateResult(description, days, amount, tagResultKey);
            rateResult.ServiceLevel = serviceLevel;
            return rateResult;
        }
        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey, ServiceLevelType serviceLevel, DateTime expectedDeliveryDate)
        {
            RateResult rateResult = CreateRateResult(description, days, amount, tagResultKey, serviceLevel);
            rateResult.ExpectedDeliveryDate = expectedDeliveryDate;
            return rateResult;
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsExpress1EndiciaForResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),    
                CreateRateResult("Rate 000", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),  
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),            
            };
            rates[0].ShipmentType = ShipmentTypeCode.Express1Stamps;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Express1Endicia;
            rates[3].ShipmentType = ShipmentTypeCode.Endicia;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Stamps;
            rates[5].ShipmentType = ShipmentTypeCode.Express1Endicia;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() { "Rate 123"});
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsExpress1StampsForResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),                
            };
            rates[0].ShipmentType = ShipmentTypeCode.Endicia;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Express1Stamps;
            rates[3].ShipmentType = ShipmentTypeCode.Stamps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() {"Rate 123"});
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsEndiciaForResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),                
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Stamps;
            rates[3].ShipmentType = ShipmentTypeCode.Stamps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() {"Rate 789"});
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsExpress1EndiciaForEachResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.Express1Endicia;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Stamps;
            rates[3].ShipmentType = ShipmentTypeCode.Endicia;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Stamps;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.Express1Endicia;
            rates[7].ShipmentType = ShipmentTypeCode.Express1Stamps;
            rates[8].ShipmentType = ShipmentTypeCode.Stamps;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() { "Rate abc", "Rate 2xyz" });
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsExpress1StampsForEachResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Stamps;
            rates[3].ShipmentType = ShipmentTypeCode.Stamps;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Stamps;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[7].ShipmentType = ShipmentTypeCode.Express1Stamps;
            rates[8].ShipmentType = ShipmentTypeCode.Stamps;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() { "Rate 789", "Rate 2123" });
        }

        [TestMethod]
        public void GetRates_RatesWithSameCost_ReturnsEndiciaForEachResultKey_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Stamps;
            rates[3].ShipmentType = ShipmentTypeCode.Stamps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[7].ShipmentType = ShipmentTypeCode.Endicia;
            rates[8].ShipmentType = ShipmentTypeCode.Stamps;

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() { "Rate 789", "Rate 2123" });
        }

        [TestMethod]
        public void GetRates_RatesWithDifferentCosts_ReturnsOneRatePerServiceTypePerTag_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 0.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 1.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 2.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 3.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 5.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 6.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 7.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 8.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            
            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rates, new List<string>() { "Rate abc", "Rate 2abc" });
        }

        private void RunQueueTest(List<RateResult> rates, List<string> correctRateResultDescriptions)
        {
            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            Queue<RateResult> testQueue = new Queue<RateResult>();
            rates.ForEach(testQueue.Enqueue);

            for (int i = 0; i < rates.Count; i++)
            {
                Debug.WriteLine("====================================Iteration: " + i + ", " + rates.First().ShipmentType + rates.First().Description);

                broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new RateGroup(rates));

                RateGroup rateGroup = testObject.GetRates(shipment);
                List<RateResult> bestRates = rateGroup.Rates.ToList();

                // Find the list of correct results based on the rate result description passed in.
                List<RateResult> correctRateResults = rates.Join(correctRateResultDescriptions, 
                                                                 rr => rr.Description, 
                                                                 correct => correct,
                                                                 (rr, correct) => rr).ToList();

                // Make sure the counts of correct results matches the rates returned.
                Assert.AreEqual(correctRateResultDescriptions.Count(), bestRates.Count());

                // Check each of the correct results with the returned results to make sure they are correct.
                correctRateResults.ForEach(rr => Assert.AreEqual(rr, 
                                                                 bestRates.First(br => br.Description == rr.Description)));

                // Shift the last entry to the first.
                testQueue.Enqueue(testQueue.Dequeue());
                rates = testQueue.ToList();
            }
        }
    }
}
