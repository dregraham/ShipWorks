using Quartz;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Defines the components of an ActionSchedule adapted to the Quartz framework.
    /// </summary>
    public class QuartzActionSchedule
    {
        public IScheduleBuilder ScheduleBuilder { get; set; }
        public ICalendar Calendar { get; set; }
    }
}
