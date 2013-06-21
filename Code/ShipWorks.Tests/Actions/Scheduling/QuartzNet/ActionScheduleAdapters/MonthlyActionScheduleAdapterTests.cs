using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class MonthlyActionScheduleAdapterTests
    {
        MonthlyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new MonthlyActionScheduleAdapter();
        }


        [TestMethod]
        public void DayMode_FiresAtStartTimeOfDay()
        {
            var schedule = new MonthlyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDay = DayOfWeek.Monday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            Assert.IsTrue(fireTimes.All(
                x => x.ToUniversalTime().TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
            ));
        }

        [TestMethod]
        public void DateMode_FiresAtStartTimeOfDay()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 1, 15 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.June }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            Assert.IsTrue(fireTimes.All(
                x => x.ToUniversalTime().TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
            ));
        }
    }
}
