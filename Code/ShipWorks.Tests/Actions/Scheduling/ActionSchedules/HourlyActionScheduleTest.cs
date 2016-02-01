using System;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class HourlyActionScheduleTest
    {
        private readonly HourlyActionSchedule testObject;

        public HourlyActionScheduleTest()
        {
            testObject = new HourlyActionSchedule();
        }
        
        [Fact]
        public void ScheduleType_ReturnsHourly()
        {
            Assert.Equal(ActionScheduleType.Hourly, testObject.ScheduleType);
        }

        [Fact]
        public void FrequencyInHours_ChangesTo1_WhenSettingValueToZero()
        {
            testObject.FrequencyInHours = 0;

            Assert.Equal(1, testObject.FrequencyInHours);
        }

        [Fact]
        public void FrequencyInHours_ChangesTo1_WhenSettingValueToNegativeValue()
        {
            testObject.FrequencyInHours = -1;

            Assert.Equal(1, testObject.FrequencyInHours);
        }

        [Fact]
        public void FrequencyInHours_ChangesTo23_WhenSettingValueGreaterThan23()
        {
            testObject.FrequencyInHours = 24;

            Assert.Equal(23, testObject.FrequencyInHours);
        }

        [Fact]
        public void FrequencyInHours_AllowsPositiveValue()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInHours = 1;

            Assert.Equal(1, testObject.FrequencyInHours);
        }

        [Fact]
        public void CreateEditor_ReturnsHourlyActionScheduleEditor()
        {
            Assert.IsAssignableFrom<HourlyActionScheduleEditor>(testObject.CreateEditor());
        }
    }
}
