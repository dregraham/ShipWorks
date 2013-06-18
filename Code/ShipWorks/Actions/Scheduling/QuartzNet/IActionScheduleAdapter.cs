using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Adapts a ShipWorks action schedule to a Quartz schedule builder.
    /// </summary>
    public interface IActionScheduleAdapter
    {
        Quartz.IScheduleBuilder Adapt(ActionSchedule schedule);
    }
}
