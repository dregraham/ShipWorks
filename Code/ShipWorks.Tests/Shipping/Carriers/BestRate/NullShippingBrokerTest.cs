using System;
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

        [TestInitialize]
        public void Initialize()
        {
            testObject = new NullShippingBroker();
        }
        
        [Fact]
        public void GetBestRates_ReturnsEmptyList_Test()
        {
            IEnumerable<RateResult> rates = testObject.GetBestRates(new ShipmentEntity(), new List<BrokerException>()).Rates;

            Assert.IsTrue(!rates.Any());
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_Test()
        {
            Assert.IsFalse(testObject.HasAccounts);
        }
    }
}
