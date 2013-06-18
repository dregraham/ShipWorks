using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class OneTimeActionScheduleAdapter : ActionScheduleAdapter<OneTimeActionSchedule>
    {
        public override IScheduleBuilder Adapt(OneTimeActionSchedule schedule)
        {
            return SimpleScheduleBuilder.Create();
        }
    }
}
