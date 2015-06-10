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
        private TimeSpan ThreeDays = new TimeSpan(3, 0, 0, 0);
        private TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
        private TimeSpan OneHour = new TimeSpan(0, 1, 0, 0);

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

        [TestMethod]
        public void FiresAtSpecifiedFrequency_WhenDstEnds()
        {
            DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 3, StartDateTimeInUtc = DateTime.Parse("10/27/2015").AddHours(4).ToUniversalTime() };

            DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            TimeSpan[] intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals[0] == ThreeDays);
            Assert.IsTrue(intervals[1] == ThreeDays.Add(OneHour));
            Assert.IsTrue(intervals[2] == ThreeDays);
            Assert.IsTrue(intervals[3] == ThreeDays);

        }

        [TestMethod]
        public void FiresAtSpecifiedFrequency_WhenDstStarts()
        {
            DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 3, StartDateTimeInUtc = DateTime.Parse("3/4/2015").AddHours(4).ToUniversalTime() };

            DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            TimeSpan[] intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals[0] == ThreeDays);
            Assert.IsTrue(intervals[1] == ThreeDays.Subtract(OneHour));
            Assert.IsTrue(intervals[2] == ThreeDays);
            Assert.IsTrue(intervals[3] == ThreeDays);
        }

        [TestMethod]
        public void FiresOnceWhenHourRepeats_WhenDstEnds()
        {
            DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 1, StartDateTimeInUtc = DateTime.Parse("10/30/2015 02:30:00 AM").ToUniversalTime() };

            DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            TimeSpan[] intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals[0] == OneDay);
            Assert.IsTrue(intervals[1] == OneDay.Add(OneHour));
            Assert.IsTrue(intervals[2] == OneDay);
            Assert.IsTrue(intervals[3] == OneDay);
        }

    }
}
