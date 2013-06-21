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
        public void DayMode_FiresOnSpecifiedLocalDay()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("3/4/2013Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Third,
                ExecuteOnDay = DayOfWeek.Thursday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.IsTrue(fireTimes.All(
                x => schedule.ExecuteOnDay == x.ToLocalTime().DayOfWeek
            ));
        }

        [TestMethod]
        public void DayMode_FiresOnSpecifiedLocalWeek()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("3/4/2013Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Fourth,
                ExecuteOnDay = DayOfWeek.Wednesday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June, MonthType.December }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.IsTrue(fireTimes.All(
                x => schedule.ExecuteOnWeek == (WeekOfMonthType)(x.ToLocalTime().Day / 7)
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

        [TestMethod]
        public void DateMode_FiresOnSpecifiedLocalDates()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 1, 5, 15 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.June }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.IsTrue(fireTimes.All(
                x => schedule.ExecuteOnDates.Contains(x.ToLocalTime().Day)
            ));
        }

        [TestMethod]
        public void DateMode_FiresOnSpecifiedMonths()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("4/9/2013Z").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 7 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.January, MonthType.August, MonthType.September }
            };

            var fireTimes = schedule.ComputeFireTimes(target, 6);

            Assert.IsTrue(fireTimes.All(
                x => schedule.ExecuteOnDateMonths.Contains((MonthType)(x.ToLocalTime().Month - 1))
            ));
        }
    }
}
