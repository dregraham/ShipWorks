using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class ActionScheduleFactoryTest
    {
        [TestMethod]
        public void CreateActionSchedule_ReturnsOneTimeActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.OneTime), typeof(OneTimeActionSchedule));
        }

        [TestMethod]
        public void CreateActionSchedule_ReturnsHourlyTimeActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Hourly), typeof(HourlyActionSchedule));
        }

        [TestMethod]
        public void CreateActionSchedule_ReturnsDailyActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Daily), typeof(DailyActionSchedule));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateActionSchedule_ThrowsArgumentOutOfRangeException_Test()
        {
            // This test will pass until the next schedule type is added (i.e. the failure will be a 
            // "reminder" to add/edit these tests as factory is changed)
            ActionScheduleFactory.CreateActionSchedule((ActionScheduleType) 3);
        }
    }
}
