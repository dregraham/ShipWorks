using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Moq;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class ActionScheduleTest
    {
        private readonly ActionSchedule testObject;

        public ActionScheduleTest()
        {
            testObject = new Mock<ActionSchedule> { CallBase = true }.Object;
        }
        
        [TestMethod]
        public void StartDateTimeInUtc_HourIsIncremented_Test()
        {
            DateTime utcNow = DateTime.UtcNow;

            Assert.AreEqual((utcNow.Hour + 1) % 24, testObject.StartDateTimeInUtc.Hour);
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
