using Interapptive.Shared.Utility;
using Quartz;
using Quartz.Impl.Calendar;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using System;
using System.Linq;
using ShipWorks.Common;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class WeeklyActionScheduleAdapter : ActionScheduleAdapter<WeeklyActionSchedule>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public WeeklyActionScheduleAdapter()
        {
            this.dateTimeProvider = new DateTimeProvider();
        }

        public WeeklyActionScheduleAdapter(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }
        
        public override QuartzActionSchedule Adapt(WeeklyActionSchedule schedule)
        {
            return new QuartzActionSchedule
            {
                ScheduleBuilder =
                    CalendarIntervalScheduleBuilder.Create()
                        .PreserveHourOfDayAcrossDaylightSavings(true)
                        .WithIntervalInDays(1)
                        .InTimeZone(dateTimeProvider.TimeZoneInfo),

                Calendar =
                    new IntervalCalendar
                    {
                        StartTimeUtc = schedule.StartDateTimeInUtc,
                        RepeatInterval = schedule.FrequencyInWeeks,
                        RepeatIntervalUnit = IntervalUnit.Week,

                        CalendarBase =
                            new WeeklyCalendar
                            {
                                DaysExcluded =
                                    Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                                        .GroupJoin(schedule.ExecuteOnDays, x => x, x => x, (x, g) => !g.Any())
                                        .ToArray()
                            }
                    }
            };
        }
    }
}
