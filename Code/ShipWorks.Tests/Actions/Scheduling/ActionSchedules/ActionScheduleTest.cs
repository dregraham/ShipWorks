using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Moq;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class ActionScheduleTest
    {
        private readonly ActionSchedule testObject;

        public ActionScheduleTest()
        {
            testObject = new Mock<ActionSchedule> { CallBase = true }.Object;
        }
        
        [Fact]
        public void StartDateTimeInUtc_HourIsIncremented()
        {
            DateTime utcNow = DateTime.UtcNow;

            Assert.Equal((utcNow.Hour + 1) % 24, testObject.StartDateTimeInUtc.Hour);
        }

        [Fact]
        public void StartDateTimeInUtc_MinuteIsZero()
        {
            Assert.Equal(0, testObject.StartDateTimeInUtc.Minute);
        }

        [Fact]
        public void StartDateTimeInUtc_SecondIsZero()
        {
            Assert.Equal(0, testObject.StartDateTimeInUtc.Second);
        }
    }
}
