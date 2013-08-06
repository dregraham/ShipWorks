using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public class OneTimeActionScheduleAdapter : ActionScheduleAdapter<OneTimeActionSchedule>
    {
        public override QuartzActionSchedule Adapt(OneTimeActionSchedule schedule)
        {
            return new QuartzActionSchedule {
                ScheduleBuilder = SimpleScheduleBuilder.Create()
            };
        }
    }
}
