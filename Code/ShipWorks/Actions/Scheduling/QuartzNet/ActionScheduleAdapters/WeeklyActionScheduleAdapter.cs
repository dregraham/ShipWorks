using Quartz;
using Quartz.Impl.Calendar;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using System;
using System.Linq;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class WeeklyActionScheduleAdapter : ActionScheduleAdapter<WeeklyActionSchedule>
    {
        public override QuartzActionSchedule Adapt(WeeklyActionSchedule schedule)
        {
            return new QuartzActionSchedule
            {
                ScheduleBuilder =
                    CalendarIntervalScheduleBuilder.Create()
                        .PreserveHourOfDayAcrossDaylightSavings(true)
                        .WithIntervalInDays(1),

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
