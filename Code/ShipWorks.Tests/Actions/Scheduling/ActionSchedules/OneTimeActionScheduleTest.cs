using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class OneTimeActionScheduleTest
    {

        OneTimeActionSchedule testObject;

        [TestMethod]
        public void StartDateTimeInUtc_IsTopOfTheHour_WhenNullConstructorIsCalled_Test()
        {
            DateTime now = DateTime.UtcNow;

            DateTime topOfHour = now.AddMinutes(60 - now.Minute);

            testObject = new OneTimeActionSchedule();

            // Make sure the time is between before the call and after the call.
            Assert.AreEqual(topOfHour, testObject.StartDateTimeInUtc);
        }
    }
}
