using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class DailyActionScheduleAdapterTests
    {
        DailyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new DailyActionScheduleAdapter();
        }


        [TestMethod]
        public void FiresAtStartTime()
        {
            DailyActionSchedule schedule = new DailyActionSchedule { StartDateTimeInUtc = DateTime.UtcNow };

            IList<DateTimeOffset> fireTimes = schedule.ComputeFireTimes(target, 1);

            Assert.AreEqual(schedule.StartDateTimeInUtc, fireTimes[0].DateTime);
        }

        [TestMethod]
        public void FiresAtSpecifiedFrequency()
        {
            DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 3, StartDateTimeInUtc = DateTime.UtcNow };

            DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            TimeSpan[] intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals.All(x => x.TotalDays == schedule.FrequencyInDays));
        }
    }
}
