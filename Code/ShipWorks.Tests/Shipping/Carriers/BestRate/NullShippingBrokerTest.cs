using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class NullShippingBrokerTest
    {
        private NullShippingBroker testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new NullShippingBroker();
        }
        
        [TestMethod]
        public void GetBestRates_ReturnsEmptyList_Test()
        {
            IEnumerable<RateResult> rates = testObject.GetBestRates(new ShipmentEntity(), ex => { }).Rates;

            Assert.IsTrue(!rates.Any());
        }

        [TestMethod]
        public void HasAccounts_ReturnsFalse_Test()
        {
            Assert.IsFalse(testObject.HasAccounts);
        }
    }
}
