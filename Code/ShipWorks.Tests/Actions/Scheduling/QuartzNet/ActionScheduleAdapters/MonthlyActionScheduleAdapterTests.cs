﻿using Interapptive.Shared.Collections;
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
        private MonthlyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new MonthlyActionScheduleAdapter();
        }


        [TestMethod]
        public void DayMode_FiresAtStartTimeOfDay()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDay = DayOfWeek.Monday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June }
            };

            var utcFireTimes = schedule.ComputeFireTimes(target, 5)
                                       .Select(x => x.ToUniversalTime()).ToArray();

            Assert.IsTrue(utcFireTimes.All(
                x => x.TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
                              ));
        }

        [TestMethod]
        public void DayMode_FiresOnSpecifiedLocalDay()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("3/4/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Third,
                ExecuteOnDay = DayOfWeek.Thursday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 6)
                                         .Select(x => x.ToLocalTime()).ToArray();

            Assert.IsTrue(localFireTimes.All(
                x => x.DayOfWeek == schedule.ExecuteOnDay
                              ));
        }

        [TestMethod]
        public void DayMode_FiresOnSpecifiedLocalWeek()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("3/4/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Fourth,
                ExecuteOnDay = DayOfWeek.Wednesday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.June, MonthType.December }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 6)
                                         .Select(x => x.ToLocalTime()).ToArray();

            Assert.IsTrue(localFireTimes.All(
                x => (WeekOfMonthType)(x.Day/7) == schedule.ExecuteOnWeek
                              ));
        }

        [TestMethod]
        public void DayMode_FiresOnSpecifiedMonths()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("1/1/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Second,
                ExecuteOnDay = DayOfWeek.Sunday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.January, MonthType.April, MonthType.December }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 3*schedule.ExecuteOnDayMonths.Count)
                                         .Select(x => x.ToLocalTime()).ToArray();

            CollectionAssert.AreEqual(
                schedule.ExecuteOnDayMonths.Repeat(3).ToArray(),
                localFireTimes.Select(x => (MonthType)(x.Month - 1)).ToArray()
                );
        }

        [TestMethod]
        public void DayMode_FiresOnLastDayOfMonth()
        {
            var schedule = new MonthlyActionSchedule()
            {

                StartDateTimeInUtc = DateTime.Parse("1/1/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Last,
                ExecuteOnAnyDay = true,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.January, MonthType.February, MonthType.December }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 3 * schedule.ExecuteOnDayMonths.Count)
                                        .Select(x => x.ToLocalTime()).ToArray();

            Assert.AreEqual((new DateTime(2013, 1, 31)).ToString(), localFireTimes[0].Date.ToString());
            Assert.AreEqual((new DateTime(2013, 2, 28)).ToString(), localFireTimes[1].Date.ToString());
            Assert.AreEqual((new DateTime(2013, 12, 31)).ToString(), localFireTimes[2].Date.ToString());
        }

        [TestMethod]
        public void DayMode_FiresOnLastSpecifiedDayOfMonth()
        {
            var schedule = new MonthlyActionSchedule()
            {
                StartDateTimeInUtc = DateTime.Parse("1/1/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.Last,
                ExecuteOnAnyDay = false,
                ExecuteOnDay = DayOfWeek.Wednesday,
                ExecuteOnDayMonths = new List<MonthType> { MonthType.January, MonthType.February, MonthType.December }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 3 * schedule.ExecuteOnDayMonths.Count)
                                        .Select(x => x.ToLocalTime()).ToArray();

            Assert.AreEqual((new DateTime(2013, 1, 30)).ToString(), localFireTimes[0].Date.ToString());
            Assert.AreEqual((new DateTime(2013, 2, 27)).ToString(), localFireTimes[1].Date.ToString());
            Assert.AreEqual((new DateTime(2013, 12, 25)).ToString(), localFireTimes[2].Date.ToString());

        }


        [TestMethod]
        public void DateMode_FiresAtStartTimeOfDay()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 1, 15 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.June }
            };

            var utcFireTimes = schedule.ComputeFireTimes(target, 5)
                                       .Select(x => x.ToUniversalTime()).ToArray();

            Assert.IsTrue(utcFireTimes.All(
                x => x.TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
                              ));
        }

        [TestMethod]
        public void DateMode_FiresOnSpecifiedLocalDates()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("1/1/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 1, 5, 28 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.June }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 3*schedule.ExecuteOnDates.Count)
                                         .Select(x => x.ToLocalTime()).ToArray();

            CollectionAssert.AreEqual(
                schedule.ExecuteOnDates.Repeat(3).ToArray(),
                localFireTimes.Select(x => x.Day).ToArray()
                );
        }

        [TestMethod]
        public void DateMode_FiresOnSpecifiedMonths()
        {
            var schedule = new MonthlyActionSchedule
            {
                StartDateTimeInUtc = DateTime.Parse("1/1/2013").ToUniversalTime(),
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 7 },
                ExecuteOnDateMonths = new List<MonthType> { MonthType.January, MonthType.August, MonthType.September }
            };

            var localFireTimes = schedule.ComputeFireTimes(target, 3*schedule.ExecuteOnDateMonths.Count)
                                         .Select(x => x.ToLocalTime()).ToArray();

            CollectionAssert.AreEqual(
                schedule.ExecuteOnDateMonths.Repeat(3).ToArray(),
                localFireTimes.Select(x => (MonthType)(x.Month - 1)).ToArray()
                );
        }
    }
}
