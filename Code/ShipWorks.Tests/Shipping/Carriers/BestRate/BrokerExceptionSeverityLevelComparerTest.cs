using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class BrokerExceptionSeverityLevelComparerTest
    {
        private BrokerExceptionSeverityLevelComparer testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new BrokerExceptionSeverityLevelComparer();
        }

        [TestMethod]
        public void Compare_HighToLow_IsNegative_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.High, BrokerExceptionSeverityLevel.Low);

            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void Compare_LowToHigh_IsPositive_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Low, BrokerExceptionSeverityLevel.High);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Compare_HighToHigh_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.High, BrokerExceptionSeverityLevel.High);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_LowToLow_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Low, BrokerExceptionSeverityLevel.Low);

            Assert.AreEqual(0, result);
        }

    }
}
