using System;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class DailyActionScheduleTest
    {
        private readonly DailyActionSchedule testObject;

        public DailyActionScheduleTest()
        {
            testObject = new DailyActionSchedule();
        }

        [Fact]
        public void ScheduleType_ReturnsDaily_Test()
        {
            Assert.Equal(ActionScheduleType.Daily, testObject.ScheduleType);
        }

        [Fact]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToZero_Test()
        {
            Assert.Throws<ActionScheduleException>(() => testObject.FrequencyInDays = 0);
        }

        [Fact]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToNegativeValue_Test()
        {
            Assert.Throws<ActionScheduleException>(() => testObject.FrequencyInDays = -1);
        }

        [Fact]
        public void FrequencyInDays_AllowsPositiveValue_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInDays = 1;

            Assert.Equal(1, testObject.FrequencyInDays);
        }

        [Fact]
        public void CreateEditor_ReturnsDailyActionScheduleEditor_Test()
        {
            Assert.IsAssignableFrom<DailyActionScheduleEditor>(testObject.CreateEditor());
        }
    }
}
