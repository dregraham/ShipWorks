using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class DailyActionScheduleTest
    {
        private readonly DailyActionSchedule testObject;

        public DailyActionScheduleTest()
        {
            testObject = new DailyActionSchedule();
        }

        [TestMethod]
        public void ScheduleType_ReturnsDaily_Test()
        {
            Assert.AreEqual(ActionScheduleType.Daily, testObject.ScheduleType);
        }

        [TestMethod]
        [ExpectedException(typeof(ActionScheduleException))]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToZero_Test()
        {
            testObject.FrequencyInDays = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ActionScheduleException))]
        public void FrequencyInDays_ThrowsActionScheduleException_WhenSettingValueToNegativeValue_Test()
        {
            testObject.FrequencyInDays = -1;
        }

        [TestMethod]
        public void FrequencyInDays_AllowsPositiveValue_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInDays = 1;

            Assert.AreEqual(1, testObject.FrequencyInDays);
        }

        [TestMethod]
        public void CreateEditor_ReturnsDailyActionScheduleEditor_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateEditor(), typeof(DailyActionScheduleEditor));
        }
    }
}
