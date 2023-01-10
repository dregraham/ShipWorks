using System;
using Interapptive.Shared.Utility;
using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class DailyActionScheduleAdapter : ActionScheduleAdapter<DailyActionSchedule>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public DailyActionScheduleAdapter(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public override QuartzActionSchedule Adapt(DailyActionSchedule schedule)
        {
            return new QuartzActionSchedule {
                ScheduleBuilder =
                    CalendarIntervalScheduleBuilder.Create()
                        .PreserveHourOfDayAcrossDaylightSavings(true)
                        .InTimeZone(dateTimeProvider.TimeZoneInfo)
                        .WithIntervalInDays(schedule.FrequencyInDays)
            };
        }
    }
}
