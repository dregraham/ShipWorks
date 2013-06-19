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
        public void StartDateTimeInUtc_IsNow_WhenNullConstructorIsCalled_Test()
        {
            DateTime before = DateTime.UtcNow;

            testObject = new OneTimeActionSchedule();

            DateTime after = DateTime.UtcNow;

            // Make sure the time is between before the call and after the call.
            Assert.IsTrue(testObject.StartDateTimeInUtc >= before 
                && testObject.StartDateTimeInUtc <= after);
        }
    }
}
