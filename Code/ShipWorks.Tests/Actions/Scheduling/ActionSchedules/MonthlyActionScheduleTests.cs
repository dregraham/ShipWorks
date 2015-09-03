using Xunit;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using System;
using System.Collections.Generic;


namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class MonthlyActionScheduleTests
    {
        [Fact]
        public void DayMode_Validate_AtLeastOneMonthIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDay = DayOfWeek.Monday
            };

            Assert.Throws<SchedulingException>(() => target.Validate());
        }

        [Fact]
        public void DayMode_Validate_SucceedsWhenValid()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDay = DayOfWeek.Monday,
                ExecuteOnDayMonths = new List<MonthType>() { MonthType.February }
            };

            target.Validate();
        }


        [Fact]
        public void DateMode_Validate_AtLeastOneDayIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDateMonths = new List<MonthType>() { MonthType.April }
            };

            Assert.Throws<SchedulingException>(() => target.Validate());
        }

        [Fact]
        public void DateMode_Validate_AtLeastOneMonthIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 5 }
            };

            Assert.Throws<SchedulingException>(() => target.Validate());
        }

        [Fact]
        public void DateMode_Validate_SucceedsWhenValid()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int>() { 25 },
                ExecuteOnDateMonths = new List<MonthType>() { MonthType.February }
            };

            target.Validate();
        }
    }
}
