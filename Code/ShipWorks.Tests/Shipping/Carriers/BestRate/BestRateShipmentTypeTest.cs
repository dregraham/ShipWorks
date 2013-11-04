using System;
using System.Collections.Generic;
using System.Linq;
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
                new RateResult("Best rate 1", "5", 4.23M, null),
                new RateResult("Best rate 1", "4", 6.23M, null)
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
    }
}
