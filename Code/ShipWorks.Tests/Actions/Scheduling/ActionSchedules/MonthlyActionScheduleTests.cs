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
        [Fact, ExpectedException(typeof(SchedulingException))]
        public void DayMode_Validate_AtLeastOneMonthIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDay = DayOfWeek.Monday
            };

            try
            {
                target.Validate();
            }
            catch (SchedulingException ex)
            {
                Assert.AreEqual("At least one month must be scheduled.", ex.Message);
                throw;
            }
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


        [Fact, ExpectedException(typeof(SchedulingException))]
        public void DateMode_Validate_AtLeastOneDayIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDateMonths = new List<MonthType>() { MonthType.April }
            };

            try
            {
                target.Validate();
            }
            catch (SchedulingException ex)
            {
                Assert.AreEqual("At least one day must be scheduled.", ex.Message);
                throw;
            }
        }

        [Fact, ExpectedException(typeof(SchedulingException))]
        public void DateMode_Validate_AtLeastOneMonthIsRequired()
        {
            var target = new MonthlyActionSchedule
            {
                CalendarType = MonthlyCalendarType.Date,
                ExecuteOnDates = new List<int> { 5 }
            };

            try
            {
                target.Validate();
            }
            catch (SchedulingException ex)
            {
                Assert.AreEqual("At least one month must be scheduled.", ex.Message);
                throw;
            }
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
