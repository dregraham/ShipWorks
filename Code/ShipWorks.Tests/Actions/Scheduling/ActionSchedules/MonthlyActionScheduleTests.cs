using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using System;
using System.Collections.Generic;


namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class MonthlyActionScheduleTests
    {
        [TestMethod, ExpectedException(typeof(SchedulingException))]
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

        [TestMethod]
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


        [TestMethod, ExpectedException(typeof(SchedulingException))]
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

        [TestMethod, ExpectedException(typeof(SchedulingException))]
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

        [TestMethod]
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
