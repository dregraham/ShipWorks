using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Actions.Scheduling.ActionSchedules.Fakes;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class ActionScheduleTest
    {
        private readonly FakeActionScheduler testObject;

        public ActionScheduleTest()
        {
            testObject = new FakeActionScheduler();
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
