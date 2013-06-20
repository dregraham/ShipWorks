using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class HourlyActionScheduleTest
    {
        private readonly HourlyActionSchedule testObject;

        public HourlyActionScheduleTest()
        {
            testObject = new HourlyActionSchedule();
        }

        [TestMethod]
        public void ScheduleType_ReturnsHourly_Test()
        {
            Assert.AreEqual(ActionScheduleType.Hourly, testObject.ScheduleType);
        }

        [TestMethod]
        public void FrequencyInHours_ChangesTo1_WhenSettingValueToZero_Test()
        {
            testObject.FrequencyInHours = 0;

            Assert.AreEqual(1, testObject.FrequencyInHours);
        }

        [TestMethod]
        public void FrequencyInHours_ChangesTo1_WhenSettingValueToNegativeValue_Test()
        {
            testObject.FrequencyInHours = -1;

            Assert.AreEqual(1, testObject.FrequencyInHours);
        }

        [TestMethod]
        public void FrequencyInHours_ChangesTo23_WhenSettingValueGreaterThan23_Test()
        {
            testObject.FrequencyInHours = 24;

            Assert.AreEqual(23, testObject.FrequencyInHours);
        }

        [TestMethod]
        public void FrequencyInHours_AllowsPositiveValue_Test()
        {
            // Prove that a positive number sets property correctly
            testObject.FrequencyInHours = 1;

            Assert.AreEqual(1, testObject.FrequencyInHours);
        }

        [TestMethod]
        public void CreateEditor_ReturnsHourlyActionScheduleEditor_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateEditor(), typeof(HourlyActionScheduleEditor));
        }
    }
}
