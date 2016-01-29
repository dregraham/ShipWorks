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

        public ServiceLevelSpeedComparerTest()
        {
            testObject = new ServiceLevelSpeedComparer();
        }

        [Fact]
        public void Compare_AnyTimeToOneDay_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.OneDay);

            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToTwoDay_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.TwoDays);

            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToThreeDay_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.ThreeDays);

            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToFourDay_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.FourToSevenDays);

            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_AnyTimeToAnytime_IsZero()
        {
            int result = testObject.Compare(ServiceLevelType.Anytime, ServiceLevelType.Anytime);

            Assert.True(result == 0);
        }




        [Fact]
        public void Compare_OneDayToAnyTime_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.OneDay, ServiceLevelType.Anytime);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_TwoDayToAnyTime_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.TwoDays, ServiceLevelType.Anytime);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_ThreeDayToAnyTime_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.ThreeDays, ServiceLevelType.Anytime);

            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_FourDayToAnyTime_IsNegative()
        {
            int result = testObject.Compare(ServiceLevelType.FourToSevenDays, ServiceLevelType.Anytime);

            Assert.True(result < 0);
        }

    }
}
