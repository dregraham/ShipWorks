﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class BestRateUpsRestrictionShippingPolicyTest
    {
        private BestRateUpsRestrictionShippingPolicy testObject;
        private List<IBestRateShippingBroker> brokers;
        private int initialBrokerCount;
        private List<ShipmentTypeCode> shipmentTypeCodes;

        public BestRateUpsRestrictionShippingPolicyTest()
        {
            testObject = new BestRateUpsRestrictionShippingPolicy();

            brokers = new List<IBestRateShippingBroker>()
            {
                new EndiciaBestRateBroker(),
                new Express1EndiciaBestRateBroker(),
                new WorldShipBestRateBroker(),
                new UpsBestRateBroker(),
            };

            initialBrokerCount = brokers.Count;

            shipmentTypeCodes = new List<ShipmentTypeCode>();
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_RestrictedAndTargetIsListOfBrokers()
        {
            testObject.Configure("true");

            Assert.True(testObject.IsApplicable(brokers), "Expected IsApplicable to be true.");
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_RestrictedAndTargetIsListOfShipmentTypeCodes()
        {
            testObject.Configure("true");

            Assert.True(testObject.IsApplicable(shipmentTypeCodes), "Expected IsApplicable to be true.");
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_NotRestrictedAndTargetIsListOfBrokers()
        {
            testObject.Configure("false");

            Assert.False(testObject.IsApplicable(brokers), "Expected IsApplicable to be false.");
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_RestrictedAndTargetIsNotListOfBrokers()
        {
            testObject.Configure("true");

            List<string> strings = new List<string>();

            Assert.False(testObject.IsApplicable(strings), "Expected IsApplicable to be false.");
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_ConfiguredNotCalledAndTargetIsListOfBrokers()
        {
            testObject.Configure("true");

            List<string> strings = new List<string>();

            Assert.False(testObject.IsApplicable(strings), "Expected IsApplicable to be false.");
        }

        [Fact]
        public void Apply_DoesNotFilterBrokers_NotRestricted()
        {
            testObject.Configure("false");

            testObject.Apply(brokers);

            Assert.Equal(initialBrokerCount, brokers.Count);
        }

        [Fact]
        public void Apply_FiltersBrokers_Restricted()
        {
            testObject.Configure("true");

            testObject.Apply(brokers);

            Assert.Equal(initialBrokerCount - 2, brokers.Count);
        }

        [Fact]
        public void Apply_AddsUpsOnlineToolsAndUpsWorldShipToTarget_Restricted()
        {
            testObject.Configure("true");

            testObject.Apply(shipmentTypeCodes);

            Assert.True(shipmentTypeCodes.Contains(ShipmentTypeCode.UpsOnLineTools));
            Assert.True(shipmentTypeCodes.Contains(ShipmentTypeCode.UpsWorldShip));
        }

        [Fact]
        public void Apply_TargetRemainsEmpty_NotRestricted()
        {
            testObject.Configure("false");

            testObject.Apply(shipmentTypeCodes);

            Assert.False(shipmentTypeCodes.Any());
        }
    }
}
