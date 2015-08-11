using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class ServiceLevelSpeedComparerTest
    {
        private ServiceLevelSpeedComparer testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new ServiceLevelSpeedComparer();
        }

        [Fact]
        public void Compare_AnyTimeToOneDay_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.OneDay);

            Assert.IsTrue(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToTwoDay_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.TwoDays);

            Assert.IsTrue(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToThreeDay_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.ThreeDays);

            Assert.IsTrue(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToFourDay_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.FourToSevenDays);

            Assert.IsTrue(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToAnytime_IsZero_Test()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.Anytime);

            Assert.IsTrue(result == 0);
        }




        [Fact]
        public void Compare_OneDayToAnyTime_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.OneDay, ServiceLevelType.Anytime);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_TwoDayToAnyTime_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.TwoDays, ServiceLevelType.Anytime);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_ThreeDayToAnyTime_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.ThreeDays, ServiceLevelType.Anytime);

            Assert.IsTrue(result < 0);
        }

        [Fact]
        public void Compare_FourDayToAnyTime_IsNegative_Test()
        {
            int result = testObject.Compare(ServiceLevelType.FourToSevenDays, ServiceLevelType.Anytime);

            Assert.IsTrue(result < 0);
        }

    }
}
