using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class WeeklyActionScheduleAdapter : ActionScheduleAdapter<WeeklyActionSchedule>
    {
        public override IScheduleBuilder Adapt(WeeklyActionSchedule schedule)
        {
            var time = schedule.StartDateTimeInUtc.ToLocalTime().TimeOfDay;

            return DailyTimeIntervalScheduleBuilder.Create()
                .OnDaysOfTheWeek(schedule.ExecuteOnDays.ToArray())
                .StartingDailyAt(new TimeOfDay(time.Hours, time.Minutes, time.Seconds))
                .WithRepeatCount(0);
        }
    }
}
