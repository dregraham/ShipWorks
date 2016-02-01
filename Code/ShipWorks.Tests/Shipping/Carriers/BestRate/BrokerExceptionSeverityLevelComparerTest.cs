using Xunit;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BrokerExceptionSeverityLevelComparerTest
    {
        private BrokerExceptionSeverityLevelComparer testObject;

        public BrokerExceptionSeverityLevelComparerTest()
        {
            testObject = new BrokerExceptionSeverityLevelComparer();
        }

        [Fact]
        public void Compare_ErrorToWarning_IsNegative()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Warning);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_ErrorToInformation_IsNegative()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Information);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_ErrorToError_IsZero()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Error, BrokerExceptionSeverityLevel.Error);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_WarningToError_IsPositive()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Error);

            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_WarningToInformation_IsNegative()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Information);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_WarningToWarning_IsZero()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Warning, BrokerExceptionSeverityLevel.Warning);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_InformationToInformatkion_IsZero()
        {
            int result = testObject.Compare(BrokerExceptionSeverityLevel.Information, BrokerExceptionSeverityLevel.Information);

            Assert.True(result == 0);
        }

    }
}
