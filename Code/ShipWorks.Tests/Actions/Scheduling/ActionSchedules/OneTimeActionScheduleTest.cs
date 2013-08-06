using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class OneTimeActionScheduleTest
    {

        OneTimeActionSchedule testObject;

        public OneTimeActionScheduleTest()
        {
            testObject = new OneTimeActionSchedule();
        }

        [TestMethod]
        public void StartDateTimeInUtc_IsTopOfTheHour_Test()
        {
            DateTime utcNow = DateTime.UtcNow;

            DateTime topOfHour = utcNow.AddMinutes(60 - utcNow.Minute);

            // Make sure the time is between before the call and after the call.
            Assert.AreEqual(topOfHour.Hour, testObject.StartDateTimeInUtc.Hour);
        }

        [TestMethod]
        public void StartDateTimeInUtc_HourIsIncremented_Test()
        {
            DateTime utcNow = DateTime.UtcNow;

            if (utcNow.Hour < 23)
            {
                Assert.AreEqual(testObject.StartDateTimeInUtc.Hour, utcNow.Hour + 1);
            }
            else
            {
                Assert.AreEqual(0, testObject.StartDateTimeInUtc.Hour);
            }
        }

        [TestMethod]
        public void StartDateTimeInUtc_MinuteIsZero_Test()
        {
            Assert.AreEqual(0, testObject.StartDateTimeInUtc.Minute);
        }

        [TestMethod]
        public void StartDateTimeInUtc_SecondIsZero_Test()
        {
            Assert.AreEqual(0, testObject.StartDateTimeInUtc.Second);
        }
    }
}
