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
            Assert.AreEqual(ActionScheduleType.Daily, testObject.ScheduleType);
        }

        [Fact]
        [ExpectedException(typeof(ActionScheduleException))]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToZero_Test()
        {
            testObject.FrequencyInDays = 0;
        }

        [Fact]
        [ExpectedException(typeof(ActionScheduleException))]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToNegativeValue_Test()
        {
            testObject.FrequencyInDays = -1;
        }

        [Fact]
        public void FrequencyInDays_AllowsPositiveValue_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInDays = 1;

            Assert.AreEqual(1, testObject.FrequencyInDays);
        }

        [Fact]
        public void CreateEditor_ReturnsDailyActionScheduleEditor_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateEditor(), typeof(DailyActionScheduleEditor));
        }
    }
}
