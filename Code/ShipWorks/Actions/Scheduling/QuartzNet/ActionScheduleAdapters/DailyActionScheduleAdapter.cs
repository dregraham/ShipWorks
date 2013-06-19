using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class DailyActionScheduleAdapter : ActionScheduleAdapter<DailyActionSchedule>
    {
        public override IScheduleBuilder Adapt(DailyActionSchedule schedule)
        {
            return CalendarIntervalScheduleBuilder.Create()
                .WithIntervalInDays(schedule.FrequencyInDays);
        }
    }
}
