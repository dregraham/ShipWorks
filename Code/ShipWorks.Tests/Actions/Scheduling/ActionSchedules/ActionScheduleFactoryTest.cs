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
            Assert.IsAssignableFrom<OneTimeActionSchedule>(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.OneTime));
        }

        [Fact]
        public void CreateActionSchedule_ReturnsHourlyTimeActionSchedule_Test()
        {
            Assert.IsAssignableFrom<HourlyActionSchedule>(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Hourly));
        }

        [Fact]
        public void CreateActionSchedule_ReturnsDailyActionSchedule_Test()
        {
            Assert.IsAssignableFrom<DailyActionSchedule>(ActionScheduleFactory.CreateActionSchedule(ActionScheduleType.Daily));
        }

        [Fact]
        public void CreateActionSchedule_ThrowsArgumentOutOfRangeException_Test()
        {

            // This test will pass until the next schedule type is added (i.e. the failure will be a 
            // "reminder" to add/edit these tests as factory is changed)
            Assert.Throws<ArgumentOutOfRangeException>(() => ActionScheduleFactory.CreateActionSchedule((ActionScheduleType) 5));
        }
    }
}
