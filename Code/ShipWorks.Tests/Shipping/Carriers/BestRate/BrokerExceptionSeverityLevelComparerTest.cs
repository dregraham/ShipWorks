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
        public void Compare_ErrorToWarning_IsNegative_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Warning);

            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void Compare_WarningToError_IsPositive_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Error);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Compare_ErrorToError_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Error);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_WarningToWarning_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Warning);

            Assert.AreEqual(0, result);
        }

    }
}
