using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class WeeklyActionScheduleAdapterTests
    {

        private readonly TimeSpan ThreeWeeks = new TimeSpan(21, 0, 0, 0);
        private readonly TimeSpan OneHour = new TimeSpan(0, 1, 0, 0);

        WeeklyActionScheduleAdapter target;

        public WeeklyActionScheduleAdapterTests()
        {
            target = new WeeklyActionScheduleAdapter();
        }


        [Fact]
        public void FiresAtStartTimeOfDay()
        {
            var schedule = new WeeklyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                FrequencyInWeeks = 2,
                ExecuteOnDays = { DayOfWeek.Monday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            Assert.True(fireTimes.All(
                x => x.ToUniversalTime().TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
            ));
        }

        [Fact]
        public void FiresOnSpecifiedLocalDays()
        {
            var schedule = new WeeklyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013Z").ToUniversalTime(),     //Sunday local, Monday UTC
                FrequencyInWeeks = 1,
                ExecuteOnDays = { DayOfWeek.Monday, DayOfWeek.Wednesday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.True(fireTimes.All(
                x => schedule.ExecuteOnDays.Contains(x.ToLocalTime().DayOfWeek)
            ));
        }

        [Fact]
        public void FiresAtSpecifiedFrequency()
        {
            var schedule = new WeeklyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                FrequencyInWeeks = 3,
                ExecuteOnDays = { DayOfWeek.Wednesday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.True(intervals.All(x => x.TotalDays == 7 * schedule.FrequencyInWeeks));
        }

        [Fact]
        public void FiresAtSpecifiedFrequencyWhenDstEnds()
        {
            var schedule = new WeeklyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("10/1/2015 03:31Z").ToUniversalTime(),
                FrequencyInWeeks = 3,
                ExecuteOnDays = { DayOfWeek.Wednesday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.True(intervals[0] == ThreeWeeks);
            Assert.True(intervals[1] == ThreeWeeks.Add(OneHour));
            Assert.True(intervals[2] == ThreeWeeks);
            Assert.True(intervals[3] == ThreeWeeks);
        }

        [Fact]
        public void FiresAtSpecifiedFrequencyWhenDstStarts()
        {
            var schedule = new WeeklyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("2/1/2015 03:31Z").ToUniversalTime(),
                FrequencyInWeeks = 3,
                ExecuteOnDays = { DayOfWeek.Wednesday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            var intervals = fireTimes.Skip(1)
                .Zip(fireTimes, (x, x0) => x - x0)
                .ToArray();

            Assert.True(intervals[0] == ThreeWeeks);
            Assert.True(intervals[1] == ThreeWeeks.Subtract(OneHour));
            Assert.True(intervals[2] == ThreeWeeks);
            Assert.True(intervals[3] == ThreeWeeks);
        }
    }
}
