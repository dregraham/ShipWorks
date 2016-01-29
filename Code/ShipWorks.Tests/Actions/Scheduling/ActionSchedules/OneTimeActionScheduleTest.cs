using System;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class OneTimeActionScheduleTest
    {

        OneTimeActionSchedule testObject;

        public OneTimeActionScheduleTest()
        {
            testObject = new OneTimeActionSchedule();
        }

        [Fact]
        public void StartDateTimeInUtc_IsTopOfTheHour()
        {
            DateTime utcNow = DateTime.UtcNow;

            DateTime topOfHour = utcNow.AddMinutes(60 - utcNow.Minute);

            // Make sure the time is between before the call and after the call.
            Assert.Equal(topOfHour.Hour, testObject.StartDateTimeInUtc.Hour);
        }

        [Fact]
        public void StartDateTimeInUtc_HourIsIncremented()
        {
            DateTime utcNow = DateTime.UtcNow;

            if (utcNow.Hour < 23)
            {
                Assert.Equal(testObject.StartDateTimeInUtc.Hour, utcNow.Hour + 1);
            }
            else
            {
                Assert.Equal(0, testObject.StartDateTimeInUtc.Hour);
            }
        }

        [Fact]
        public void StartDateTimeInUtc_MinuteIsZero()
        {
            Assert.Equal(0, testObject.StartDateTimeInUtc.Minute);
        }

        [Fact]
        public void StartDateTimeInUtc_SecondIsZero()
        {
            Assert.Equal(0, testObject.StartDateTimeInUtc.Second);
        }
    }
}
