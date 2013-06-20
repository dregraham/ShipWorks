using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class WeeklyActionScheduleAdapterTests
    {
        WeeklyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new WeeklyActionScheduleAdapter();
        }


        [TestMethod]
        public void FiresAtStartTimeOfDay()
        {
            var schedule = new WeeklyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                FrequencyInWeeks = 2,
                ExecuteOnDays = { DayOfWeek.Monday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            Assert.IsTrue(fireTimes.All(
                x => x.ToUniversalTime().TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
            ));
        }

        [TestMethod]
        public void FiresOnSpecifiedLocalDays()
        {
            var schedule = new WeeklyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013Z").ToUniversalTime(),     //Sunday local, Monday UTC
                FrequencyInWeeks = 1,
                ExecuteOnDays = { DayOfWeek.Monday, DayOfWeek.Thursday }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.IsTrue(fireTimes.All(
                x => schedule.ExecuteOnDays.Contains(x.ToLocalTime().DayOfWeek)
            ));
        }

        //[TestMethod]
        //public void FiresAtSpecifiedFrequency()
        //{

        //}
    }
}
