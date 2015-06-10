using System;
using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class DailyActionScheduleAdapter : ActionScheduleAdapter<DailyActionSchedule>
    {
        public override QuartzActionSchedule Adapt(DailyActionSchedule schedule)
        {
            return new QuartzActionSchedule {
                ScheduleBuilder =
                    CalendarIntervalScheduleBuilder.Create()
                        .PreserveHourOfDayAcrossDaylightSavings(true)
                        .InTimeZone(TimeZoneInfo.Local)
                        .WithIntervalInDays(schedule.FrequencyInDays)
            };
        }
    }
}
