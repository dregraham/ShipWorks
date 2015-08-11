using System;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class ActionScheduleFactoryTest
    {
        [Fact]
        public void CreateActionSchedule_ReturnsOneTimeActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.OneTime), typeof(OneTimeActionSchedule));
        }

        [Fact]
        public void CreateActionSchedule_ReturnsHourlyTimeActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Hourly), typeof(HourlyActionSchedule));
        }

        [Fact]
        public void CreateActionSchedule_ReturnsDailyActionSchedule_Test()
        {
            Assert.IsInstanceOfType(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Daily), typeof(DailyActionSchedule));
        }

        [Fact]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateActionSchedule_ThrowsArgumentOutOfRangeException_Test()
        {
            // This test will pass until the next schedule type is added (i.e. the failure will be a 
            // "reminder" to add/edit these tests as factory is changed)
            ActionScheduleFactory.CreateActionSchedule((ActionScheduleType) 5);
        }
    }
}
