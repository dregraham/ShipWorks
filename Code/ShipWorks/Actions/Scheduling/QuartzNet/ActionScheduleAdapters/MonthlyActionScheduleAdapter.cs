using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class MonthlyActionScheduleAdapter : ActionScheduleAdapter<MonthlyActionSchedule>
    {
        public override IScheduleBuilder Adapt(MonthlyActionSchedule schedule)
        {
            return null;
        }
    }
}
