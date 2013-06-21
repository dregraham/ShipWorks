using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Adapts a ShipWorks action schedule to Quartz.
    /// </summary>
    public interface IActionScheduleAdapter
    {
        QuartzActionSchedule Adapt(ActionSchedule schedule);
    }
}
