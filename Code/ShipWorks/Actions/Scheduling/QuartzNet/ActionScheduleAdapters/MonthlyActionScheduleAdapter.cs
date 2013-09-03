using System;
using System.Linq;
using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class MonthlyActionScheduleAdapter : ActionScheduleAdapter<MonthlyActionSchedule>
    {
        public override QuartzActionSchedule Adapt(MonthlyActionSchedule schedule)
        {
            TimeSpan time = schedule.StartDateTimeInUtc.ToLocalTime().TimeOfDay;

            if (schedule.CalendarType != MonthlyCalendarType.Date && schedule.CalendarType != MonthlyCalendarType.Day)
            {
                throw new ArgumentException("The schedule has an unexpected monthly calendar type.", "schedule");
            }

            string cron;

            if (schedule.CalendarType == MonthlyCalendarType.Date)
            {
                // eg. Run on the 1st and 15th of January and February
                cron = string.Format(
                    "{0} {1} {2} {3} {4} ? *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    string.Join(",", schedule.ExecuteOnDates),
                    string.Join(",", schedule.ExecuteOnDateMonths.Select(m => (int)m + 1))
                    );
            }
            else if (schedule.ExecuteOnAnyDay && schedule.ExecuteOnWeek == WeekOfMonthType.Last)
            {
                // eg. Last day of January and February
                cron = string.Format("{0} {1} {2} L {3} ? *",
                                     time.Seconds,
                                     time.Minutes,
                                     time.Hours,
                                     string.Join(",", schedule.ExecuteOnDayMonths.Select(m => (int)m + 1)));
            }
            else if (schedule.ExecuteOnAnyDay && schedule.ExecuteOnWeek == WeekOfMonthType.First)
            {
                // eg. First day in January and February
                cron = string.Format(
                    "{0} {1} {2} {3} {4} ? *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    1,
                    string.Join(",", schedule.ExecuteOnDayMonths.Select(m => (int)m + 1))
                    );
            }
            else if (schedule.ExecuteOnAnyDay)
            {
                throw new SchedulingException("Cannot schedule a Day and Week ");
            }
            else if (schedule.ExecuteOnWeek == WeekOfMonthType.Last)
            {
                // eg. Last Friday of January and February
                cron = string.Format(
                    "{0} {1} {2} ? {3} {4}L *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    string.Join(",", schedule.ExecuteOnDayMonths.Select(m => (int)m + 1)),
                    (int)schedule.ExecuteOnDay + 1);
            }
            else
            {
                // eg. Second Tuesday of January and February
                cron = string.Format(
                    "{0} {1} {2} ? {3} {4}#{5} *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    string.Join(",", schedule.ExecuteOnDayMonths.Select(m => (int)m + 1)),
                    (int)schedule.ExecuteOnDay + 1,
                    (int)schedule.ExecuteOnWeek + 1);
            }


            return new QuartzActionSchedule
            {
                ScheduleBuilder = CronScheduleBuilder.CronSchedule(cron)
            };
        }
    }
}