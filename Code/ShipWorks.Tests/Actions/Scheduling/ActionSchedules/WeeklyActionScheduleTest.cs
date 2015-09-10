using System;
using System.Collections.Generic;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Scheduling;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class WeeklyActionScheduleTest
    {
        private readonly WeeklyActionSchedule testObject;

        public WeeklyActionScheduleTest()
        {
            testObject = new WeeklyActionSchedule();
        }

        [Fact]
        public void ScheduleType_ReturnsWeekly_Test()
        {
            Assert.Equal(ActionScheduleType.Weekly, testObject.ScheduleType);
        }

        [Fact]
        public void FrequencyInWeeks_ChangesTo1_WhenSettingValueToZero_Test()
        {
            testObject.FrequencyInWeeks = 0;

            Assert.Equal(1, testObject.FrequencyInWeeks);
        }

        [Fact]
        public void FrequencyInWeeks_ChangesTo1_WhenSettingValueToNegativeValue_Test()
        {
            testObject.FrequencyInWeeks = -1;

            Assert.Equal(1, testObject.FrequencyInWeeks);
        }

        [Fact]
        public void FrequencyInWeeks_ChangesTo52_WhenSettingValueGreaterThan52_Test()
        {
            testObject.FrequencyInWeeks = 53;

            Assert.Equal(52, testObject.FrequencyInWeeks);
        }

        [Fact]
        public void FrequencyInWeeks_AllowsPositiveValue_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInWeeks = 1;

            Assert.Equal(1, testObject.FrequencyInWeeks);
        }

        [Fact]
        public void ExecuteOnDays_CountIsZero_WhenSetToNull_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.ExecuteOnDays = null;

            Assert.Equal(0, testObject.ExecuteOnDays.Count);
        }

        [Fact]
        public void ExecuteOnDays_CountIsCorrect_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.ExecuteOnDays.Add(DayOfWeek.Sunday);
            testObject.ExecuteOnDays.Add(DayOfWeek.Monday);

            Assert.Equal(2, testObject.ExecuteOnDays.Count);
        }

        [Fact]
        public void CreateEditor_ReturnsWeeklyActionScheduleEditor_Test()
        {
            Assert.IsAssignableFrom<WeeklyActionScheduleEditor>(testObject.CreateEditor());
        }

        [Fact]
        public void Validate_AtLeastOneDayIsRequired()
        {
            testObject.FrequencyInWeeks = 1;
            Assert.Throws<SchedulingException>(() => testObject.Validate());
        }
    }
}
