using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    [TestClass]
    public class BestRateUpsRestrictionShippingPolicyTest
    {
        private IShippingPolicy testObject;
        private List<IBestRateShippingBroker> brokers;
        private int initialBrokerCount;
        private List<ShipmentTypeCode> shipmentTypeCodes;
        
        [TestInitialize]
        public void Initialize()
        {
            testObject = new BestRateUpsRestrictionShippingPolicy();

            brokers = new List<IBestRateShippingBroker>()
            {
                new EndiciaBestRateBroker(),
                new Express1EndiciaBestRateBroker(),
                new WorldShipBestRateBroker(),
                new UpsBestRateBroker(),
                new UpsCounterRatesBroker()
            };
            
            initialBrokerCount = brokers.Count;

            shipmentTypeCodes = new List<ShipmentTypeCode>();
        }

        [TestMethod]
        public void IsApplicable_ReturnsTrue_RestrictedAndTargetIsListOfBrokers()
        {
            testObject.Configure("true");

            Assert.IsTrue(testObject.IsApplicable(brokers), "Expected IsApplicable to be true.");
        }

        [TestMethod]
        public void IsApplicable_ReturnsTrue_RestrictedAndTargetIsListOfShipmentTypeCodes()
        {
            testObject.Configure("true");

            Assert.IsTrue(testObject.IsApplicable(shipmentTypeCodes), "Expected IsApplicable to be true.");
        }

        [TestMethod]
        public void IsApplicable_ReturnsFalse_NotRestrictedAndTargetIsListOfBrokers()
        {
            testObject.Configure("false");

            Assert.IsFalse(testObject.IsApplicable(brokers), "Expected IsApplicable to be false.");
        }

        [TestMethod]
        public void IsApplicable_ReturnsFalse_RestrictedAndTargetIsNotListOfBrokers()
        {
            testObject.Configure("true");

            List<string> strings = new List<string>();

            Assert.IsFalse(testObject.IsApplicable(strings), "Expected IsApplicable to be false.");
        }

        [TestMethod]
        public void IsApplicable_ReturnsFalse_ConfiguredNotCalledAndTargetIsListOfBrokers()
        {
            testObject.Configure("true");

            List<string> strings = new List<string>();

            Assert.IsFalse(testObject.IsApplicable(strings), "Expected IsApplicable to be false.");
        }

        [TestMethod]
        public void Apply_DoesNotFilterBrokers_NotRestricted()
        {
            testObject.Configure("false");

            testObject.Apply(brokers);

            Assert.AreEqual(initialBrokerCount, brokers.Count);
        }

        [TestMethod]
        public void Apply_FiltersBrokers_Restricted()
        {
            testObject.Configure("true");

            testObject.Apply(brokers);

            Assert.AreEqual(initialBrokerCount - 3, brokers.Count);
        }

        [TestMethod]
        public void Apply_AddsUpsOnlineToolsAndUpsWorldShipToTarget_Restricted()
        {
            testObject.Configure("true");

            testObject.Apply(shipmentTypeCodes);

            Assert.IsTrue(shipmentTypeCodes.Contains(ShipmentTypeCode.UpsOnLineTools));
            Assert.IsTrue(shipmentTypeCodes.Contains(ShipmentTypeCode.UpsWorldShip));
        }

        [TestMethod]
        public void Apply_TargetRemainsEmpty_NotRestricted()
        {
            testObject.Configure("false");

            testObject.Apply(shipmentTypeCodes);

            Assert.IsFalse(shipmentTypeCodes.Any());
        }
    }
}
