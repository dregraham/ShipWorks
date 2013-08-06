using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using System;
using System.Linq;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class MonthlyActionScheduleAdapter : ActionScheduleAdapter<MonthlyActionSchedule>
    {
        public override QuartzActionSchedule Adapt(MonthlyActionSchedule schedule)
        {
            var time = schedule.StartDateTimeInUtc.ToLocalTime().TimeOfDay;

            if (schedule.CalendarType == MonthlyCalendarType.Date)
            {
                var cron = string.Format(
                    "{0} {1} {2} {3} {4} ? *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    string.Join(",", schedule.ExecuteOnDates),
                    string.Join(",", schedule.ExecuteOnDateMonths.Select(m => (int)m + 1))
                );

                return new QuartzActionSchedule
                {
                    ScheduleBuilder = CronScheduleBuilder.CronSchedule(cron)
                };
            }

            if (schedule.CalendarType == MonthlyCalendarType.Day)
            {
                var cron = string.Format(
                    "{0} {1} {2} ? {3} {4}#{5} *",
                    time.Seconds,
                    time.Minutes,
                    time.Hours,
                    string.Join(",", schedule.ExecuteOnDayMonths.Select(m => (int)m + 1)),
                    (int)schedule.ExecuteOnDay + 1,
                    (int)schedule.ExecuteOnWeek + 1
                );

                return new QuartzActionSchedule
                {
                    ScheduleBuilder = CronScheduleBuilder.CronSchedule(cron)
                };
            }

            throw new ArgumentException("The schedule has an unexpected monthly calendar type.", "schedule");
        }
    }
}
