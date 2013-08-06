using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class OneTimeActionScheduleAdapterTests
    {
        OneTimeActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new OneTimeActionScheduleAdapter();
        }


        [TestMethod]
        public void FiresOnce()
        {
            var fireTimes = new OneTimeActionSchedule().ComputeFireTimes(target, 5);

            Assert.AreEqual(1, fireTimes.Count);
        }

        [TestMethod]
        public void FiresAtStartTime()
        {
            var schedule = new OneTimeActionSchedule { StartDateTimeInUtc = DateTime.UtcNow };

            var fireTimes = schedule.ComputeFireTimes(target, 1);

            Assert.AreEqual(schedule.StartDateTimeInUtc, fireTimes[0].DateTime);
        }
    }
}
