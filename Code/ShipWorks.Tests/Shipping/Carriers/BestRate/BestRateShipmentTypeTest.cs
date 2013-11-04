using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class BestRateShipmentTypeTest
    {
        private BestRateShipmentType testObject;

        private Mock<IBestRateShippingBrokerFactory> brokerFactory;
        private Mock<IBestRateShippingBroker> broker;

        private List<RateResult> rates;

        [TestInitialize]
        public void Initialize()
        {
            rates = new List<RateResult>
            {
                new RateResult("Rate xyz", "5", 4.23M, null),
                new RateResult("Rate 123", "4", 6.23M, null)
            };

            broker = new Mock<IBestRateShippingBroker>();
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            brokerFactory = new Mock<IBestRateShippingBrokerFactory>();
            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object });

            testObject = new BestRateShipmentType(brokerFactory.Object);
        }

        [TestMethod]
        public void GetRates_DelegatesToBrokerFactory_Test()
        {
            testObject.GetRates(new ShipmentEntity());

            brokerFactory.Verify(f => f.CreateBrokers(), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToEachBroker_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });
            ShipmentEntity shipment = new ShipmentEntity();

            testObject.GetRates(shipment);

            broker.Verify(b => b.GetBestRates(shipment), Times.Once());
            secondBroker.Verify(b => b.GetBestRates(shipment), Times.Once());
        }

        [TestMethod]
        public void GetRates_AddsRates_FromAllBrokers_Test()
        {
            // Setup the factory to return two brokers - the one already defined at the class level 
            // and another one for this test
            Mock<IBestRateShippingBroker> secondBroker = new Mock<IBestRateShippingBroker>();
            secondBroker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            brokerFactory.Setup(f => f.CreateBrokers()).Returns(new List<IBestRateShippingBroker> { broker.Object, secondBroker.Object });
            ShipmentEntity shipment = new ShipmentEntity();

            RateGroup rateGroup = testObject.GetRates(shipment);

            // Both brokers are setup to return two rate results, so we should have a total of four
            Assert.AreEqual(4, rateGroup.Rates.Count());
        }

        [TestMethod]
        public void GetRates_ReturnsEmptyRateGroup_WhenNoRatesAreFound_Test()
        {
            // Setup the broker to return an empty list of rate results
            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(new List<RateResult>());

            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());
            
            Assert.AreEqual(0, rateGroup.Rates.Count());
        }

        [TestMethod]
        public void GetRates_ClearsRateDescription_Test()
        {
            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());

            foreach (RateResult rate in rateGroup.Rates)
            {
                Assert.AreEqual(string.Empty, rate.Description);
            }
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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());
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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());
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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());
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

            broker.Setup(b => b.GetBestRates(It.IsAny<ShipmentEntity>())).Returns(rates);

            RateGroup rateGroup = testObject.GetRates(new ShipmentEntity());
            List<RateResult> bestRates = rateGroup.Rates.ToList();

            Assert.AreEqual(rates[0].Selectable, bestRates[0].Selectable);
            Assert.AreEqual(rates[1].Selectable, bestRates[1].Selectable);
        }

        [TestMethod]
        public void SupportsGetRates_ReturnsTrue()
        {
            Assert.IsTrue(testObject.SupportsGetRates);
        }
    }
}
