using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
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

        [TestInitialize]
        public void Initialize()
        {
            rates = new List<RateResult>
            {
                new RateResult("Rate xyz", "5", 4.23M, null),
                new RateResult("Rate 123", "4", 6.23M, null)
            };

            shipment = new ShipmentEntity()
            {
                BestRate = new BestRateShipmentEntity()
            };

            broker = new Mock<IBestRateShippingBroker>();
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            brokerFactory = new Mock<IBestRateShippingBrokerFactory>();
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object });

            log = new Mock<ILog>();


            testObject = new BestRateShipmentType(brokerFactory.Object, log.Object);
        }

        [TestMethod]
        public void GetRates_DelegatesToBrokerFactory_Test()
        {
            testObject.GetRates(shipment);

            brokerFactory.Verify(f => f.CreateBrokers(), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToEachBroker_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            testObject.GetRates(shipment);

            broker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<BrokerException>>()), Times.Once());
            secondBroker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<BrokerException>>()), Times.Once());
        }

        [TestMethod]
        public void GetRates_AddsRates_FromAllBrokers_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Both brokers are setup to return two rate results, so we should have a total of four
            Assert.AreEqual(4, rateGroup.Rates.Count());
        }

        [TestMethod]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound_Test()
        {
            // Setup the broker to return an empty list of rate results
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(new List<RateResult>());

            RateGroup rateGroup = testObject.GetRates(shipment);
            
            Assert.AreEqual(0, rateGroup.Rates.Count());
        }
        
        [TestMethod]
        public void GetRates_RateAmountFromBrokerIsUnmodified_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                new RateResult("Rate xyz", "12", 4.23M, null),
                new RateResult("Rate 123", "7", 9.87M, null)
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate xyz", "12", 4.23M, null),
                new RateResult("Rate 123", "probably 7", 9.87M, null)
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual("12", bestRates[0].Days);
            Assert.AreEqual("probably 7", bestRates[1].Days);
        }

        [TestMethod]
        public void GetRates_RateTagFromBrokerIsUnmodified_Test()
        {
            Cookie cookie = new Cookie("someCookie", "chocolate chip");

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                new RateResult("Rate xyz", "12", 4.23M, cookie),
                new RateResult("Rate 123", "probably 7", 9.87M, null)
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(cookie, bestRates[0].Tag);
            Assert.IsNull(bestRates[1].Tag);
        }

        [TestMethod]
        public void GetRates_RateSelectableFromBrokerIsUnmodified_Test()
        {
            Cookie cookie = new Cookie("someCookie", "chocolate chip");

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                new RateResult("Rate xyz", "12", 4.23M, cookie),
                new RateResult("Rate 123", "probably 7", 9.87M, null)
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate abc", "12", 34.30M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate xyz", "12", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "probably 7", 9.87M, null) { ServiceLevel = ServiceLevelType.Anytime }
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate abc", "3", 4.23M, null) { ServiceLevel = ServiceLevelType.ThreeDays },
                new RateResult("Rate xyz", "It will get there when it gets there", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "1", 4.23M, null) { ServiceLevel = ServiceLevelType.OneDay },
                new RateResult("Rate 456", "Soon", 4.23M, null) { ServiceLevel = ServiceLevelType.FourToSevenDays },
                new RateResult("Rate 789", "2", 4.23M, null) { ServiceLevel = ServiceLevelType.TwoDays },                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays },  

                // These are the rates that should be returned
                new RateResult("Rate abc", "3", 4.23M, null) { ServiceLevel = ServiceLevelType.ThreeDays },
                new RateResult("Rate xyz", "It will get there when it gets there", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "1", 4.23M, null) { ServiceLevel = ServiceLevelType.OneDay },
                new RateResult("Rate 456", "Soon", 4.23M, null) { ServiceLevel = ServiceLevelType.FourToSevenDays },
                new RateResult("Rate 789", "2", 4.23M, null) { ServiceLevel = ServiceLevelType.TwoDays },                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate xyz", "It will get there when it gets there", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "1", 4.23M, null) { ServiceLevel = ServiceLevelType.OneDay },
                new RateResult("Rate 456", "Soon", 4.23M, null) { ServiceLevel = ServiceLevelType.FourToSevenDays },
                new RateResult("Rate 789", "2", 4.23M, null) { ServiceLevel = ServiceLevelType.TwoDays },                
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(shipment);
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates.Count, bestRates.Count);
        }


        [TestMethod]
        public void GetRates_CallsMaskDescription_OnReturnedRates()
        {
            var testRate = new Mock<RateResult>();

            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                testRate.Object
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

            testObject.GetRates(shipment);

            testRate.Verify(x => x.MaskDescription(rates));
        }

        [TestMethod]
        public void GetRates_PassesExceptionHandlerToBroker_Test()
        {
            Action<ShippingException> handler = exception => { };

            testObject.GetRates(shipment, handler);

            broker.Verify(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), handler));
        }

        [TestMethod]
        public void GetRates_ReturnsOneAndTwoDayRates_When2DaysAreSpecifiedAndExpectedDateIsNull_Test()
        {
            // Setup the broker to return specific rates
            rates = new List<RateResult>
            {
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.88M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.89M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.90M, null) { ServiceLevel = ServiceLevelType.TwoDays },  

                new RateResult("Rate abc", "3", 4.23M, null) { ServiceLevel = ServiceLevelType.ThreeDays },
                new RateResult("Rate xyz", "It will get there when it gets there", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "1", 4.23M, null) { ServiceLevel = ServiceLevelType.OneDay },
                new RateResult("Rate 456", "Soon", 4.23M, null) { ServiceLevel = ServiceLevelType.FourToSevenDays },
                new RateResult("Rate 789", "2", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },                  
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
                new RateResult("Rate 789", "2", 6.87M, null) { ServiceLevel = ServiceLevelType.TwoDays, ExpectedDeliveryDate = DateTime.Today.AddDays(3) },  
                new RateResult("Rate 789", "2", 6.88M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.89M, null) { ServiceLevel = ServiceLevelType.TwoDays },  
                new RateResult("Rate 789", "2", 6.90M, null) { ServiceLevel = ServiceLevelType.TwoDays },  

                new RateResult("Rate abc", "3", 4.23M, null) { ServiceLevel = ServiceLevelType.ThreeDays },
                new RateResult("Rate xyz", "It will get there when it gets there", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },
                new RateResult("Rate 123", "1", 4.23M, null) { ServiceLevel = ServiceLevelType.OneDay },
                new RateResult("Rate 456", "Soon", .23M, null) { ServiceLevel = ServiceLevelType.FourToSevenDays, ExpectedDeliveryDate = DateTime.Today.AddDays(3)},
                new RateResult("Rate 789", "2", 4.23M, null) { ServiceLevel = ServiceLevelType.Anytime },                  
            };

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<BrokerException>>())).Returns(rates);

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
        public void GetRates_AddsBrokerExceptionsToRateGroup_WhenOneBrokerExceptionIsEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called; a fake broker is used
            // because we couldn't get this functionality with Moq
            BrokerException brokerException = new BrokerException(new ShippingException("a shipping exception"), BrokerExceptionSeverityLevel.High);
            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(brokerException);

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.IsTrue(rateGroup.Rates.First().Description.Contains("a shipping exception"));
        }

        [TestMethod]
        public void GetRates_AddsBrokerExceptionsToRateGroup_WhenMultipleBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            BrokerException brokerException = new BrokerException(new ShippingException("a shipping exception"), BrokerExceptionSeverityLevel.High);
            BrokerException anotherBrokerException = new BrokerException(new ShippingException("another shipping exception"), BrokerExceptionSeverityLevel.High);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { brokerException, anotherBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.AreEqual(2, rateGroup.Rates.Count(r => r.Description.Contains("shipping exception")));
        }

        [TestMethod]
        public void GetRates_OrdersBrokerExceptionsFromHighestToLowestSeverity_WhenMultipleBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq
            BrokerException lowSeverityBrokerException = new BrokerException(new ShippingException("a low severity shipping exception"), BrokerExceptionSeverityLevel.Low);
            BrokerException highSeverityBrokerException = new BrokerException(new ShippingException("high severity shipping exception"), BrokerExceptionSeverityLevel.High);
            BrokerException anotherhighSeverityBrokerException = new BrokerException(new ShippingException("another high severity shipping exception"), BrokerExceptionSeverityLevel.High);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { lowSeverityBrokerException, highSeverityBrokerException, anotherhighSeverityBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Can't actually test the severity level, so inspect the description
            Assert.IsTrue(rateGroup.Rates.ToList()[0].Description.Contains("high severity"));
            Assert.IsTrue(rateGroup.Rates.ToList()[1].Description.Contains("high severity"));
            Assert.IsTrue(rateGroup.Rates.ToList()[2].Description.Contains("low severity"));
        }

        [TestMethod]
        public void GetRates_RemovesDuplicateMessages_WhenBrokerExceptionsAreEncountered_Test()
        {
            // Use the fake broker for simulating the exception handler being called multiple times; a fake broker is used
            // because we couldn't get this functionality with Moq          
            BrokerException lowSeverityBrokerException = new BrokerException(new ShippingException("a low severity shipping exception"), BrokerExceptionSeverityLevel.Low);
            BrokerException highSeverityBrokerException = new BrokerException(new ShippingException("high severity shipping exception"), BrokerExceptionSeverityLevel.High);
            BrokerException anotherhighSeverityBrokerException = new BrokerException(new ShippingException("high severity shipping exception"), BrokerExceptionSeverityLevel.High);

            FakeExceptionHandlerBroker fakeBroker = new FakeExceptionHandlerBroker(new List<BrokerException> { lowSeverityBrokerException, highSeverityBrokerException, anotherhighSeverityBrokerException });

            // We want the factory to return our fake broker for this test
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { fakeBroker });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Make sure there are two exceptions, but only one with high severity message
            Assert.AreEqual(2, rateGroup.Rates.Count());
            Assert.AreEqual(1, rateGroup.Rates.Count(r => r.Description.Contains("high severity shipping exception")));
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
            broker.Setup(b => b.HasAccounts)
                  .Returns(false);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>()))
                  .Returns(InsuranceProvider.ShipWorks);

            Assert.AreEqual(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider());
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsShipWorks_TwoBrokersWithAccountsAndShipWorksInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts)
                  .Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>()))
                .Returns(InsuranceProvider.ShipWorks);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });
            
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetShipmentInsuranceProvider());
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts)
                  .Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>()))
                .Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, broker.Object });

            Assert.AreEqual(InsuranceProvider.Invalid, testObject.GetShipmentInsuranceProvider());
        }

        [TestMethod]
        public void GetShipmentInsuranceProvider_ReturnsCarrier_TwoBrokersWithAccountsAndCarrierInsurance_Test()
        {
            broker.Setup(b => b.HasAccounts)
                  .Returns(true);
            broker.Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>()))
                .Returns(InsuranceProvider.Carrier);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object });

            Assert.AreEqual(InsuranceProvider.Carrier, testObject.GetShipmentInsuranceProvider());
        }
    }
}
