﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class NullShippingBrokerTest
    {
        private NullShippingBroker testObject;

        public NullShippingBrokerTest()
        {
            testObject = new NullShippingBroker();
        }
        
        [Fact]
        public void GetBestRates_ReturnsEmptyList()
        {
            IEnumerable<RateResult> rates = testObject.GetBestRates(new ShipmentEntity(), new List<BrokerException>()).Rates;

            Assert.True(!rates.Any());
        }

        [Fact]
        public void HasAccounts_ReturnsFalse()
        {
            Assert.False(testObject.HasAccounts);
        }
    }
}
