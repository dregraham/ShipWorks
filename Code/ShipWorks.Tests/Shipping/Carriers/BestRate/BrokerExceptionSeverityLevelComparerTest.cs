﻿using Xunit;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BrokerExceptionSeverityLevelComparerTest
    {
        private BrokerExceptionSeverityLevelComparer testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new BrokerExceptionSeverityLevelComparer();
        }

        [Fact]
        public void Compare_ErrorToWarning_IsNegative_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Warning);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_ErrorToInformation_IsNegative_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Information);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_ErrorToError_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Error);

            Assert.AreEqual(0, result);
        }

        [Fact]
        public void Compare_WarningToError_IsPositive_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Error);

            Assert.IsTrue(result > 0);
        }

        [Fact]
        public void Compare_WarningToInformation_IsNegative_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Information);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_WarningToWarning_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Warning);

            Assert.AreEqual(0, result);
        }

        [Fact]
        public void Compare_InformationToInformatkion_IsZero_Test()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Information, BrokerExceptionSeverityLevel.Information);

            Assert.IsTrue(result == 0);
        }

    }
}
