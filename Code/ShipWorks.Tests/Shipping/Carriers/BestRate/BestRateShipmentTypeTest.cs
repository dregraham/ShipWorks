using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ShipWorks.Shipping;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

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
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            testObject.GetRates(shipment);

            broker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<ShippingException>>()), Times.Once());
            secondBroker.Verify(b => b.GetBestRates(shipment, It.IsAny<Action<ShippingException>>()), Times.Once());
        }

        [TestMethod]
        public void GetRates_AddsRates_FromAllBrokers_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Both brokers are setup to return two rate results, so we should have a total of four
            Assert.AreEqual(4, rateGroup.Rates.Count());
        }

        [TestMethod]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound_Test()
        {
            // Setup the broker to return an empty list of rate results
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(new List<RateResult>());

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), It.IsAny<Action<ShippingException>>())).Returns(rates);

            testObject.GetRates(shipment);

            testRate.Verify(x => x.MaskDescription(rates));
        }

        [TestMethod]
        public void GetRates_PassesExceptionHandlerToBroker()
        {
            Action<ShippingException> handler = exception => { };

            testObject.GetRates(shipment, handler);

            broker.Verify(b => b.GetBestRates(It.IsAny<ShipmentEntity>(), handler));
        }

        [TestMethod]
        public void SupportsGetRates_ReturnsTrue_Test()
        {
            Assert.IsTrue(testObject.SupportsGetRates);
        }
    }
}
