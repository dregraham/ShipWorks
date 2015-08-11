using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Linq;

namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class HourlyActionScheduleAdapterTest
    {
        HourlyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new HourlyActionScheduleAdapter();
        }

        [Fact]
        public void FiresAtStartTime()
        {
            var schedule = new HourlyActionSchedule { StartDateTimeInUtc = DateTime.UtcNow };

            var fireTimes = schedule.ComputeFireTimes(target, 1);

            Assert.AreEqual(schedule.StartDateTimeInUtc, fireTimes[0].DateTime);
        }

        [Fact]
        public void FiresAtSpecifiedFrequency()
        {
            var schedule = new HourlyActionSchedule
                {
                    StartDateTimeInUtc = DateTime.UtcNow, 
                    FrequencyInHours = 3
                };

            var fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals.All(x => (int)x.TotalHours == schedule.FrequencyInHours));
        }

        [Fact]
        public void FiresAtSpecifiedFrequencyWhenDaylightSavingTimeEnds()
        {
            var schedule = new HourlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("10/31/2015 6:00:00 PM").ToUniversalTime(),
                FrequencyInHours = 3
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals.All(x => (int)x.TotalHours == schedule.FrequencyInHours));
        }

        [Fact]
        public void FiresAtSpecifiedFrequencyWhenDaylightSavingTimeStarts()
        {
            var schedule = new HourlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("3/7/2015 6:00:00 PM").ToUniversalTime(),
                FrequencyInHours = 3
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.IsTrue(intervals.All(x => (int)x.TotalHours == schedule.FrequencyInHours));
        }
    }
}
