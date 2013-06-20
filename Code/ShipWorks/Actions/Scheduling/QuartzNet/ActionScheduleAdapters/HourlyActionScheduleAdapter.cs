using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    /// <summary>
    /// Hourly schedule adatper to create the Quartz schedule based on the ShipWorks HourlyActionSchedule
    /// </summary>
    public class HourlyActionScheduleAdapter : ActionScheduleAdapter<HourlyActionSchedule>
    {
        /// <summary>
        /// Adapts an HourlyActionSchedule to a Quartz schedule builder.
        /// </summary>
        public override IScheduleBuilder Adapt(HourlyActionSchedule schedule)
        {
            return CalendarIntervalScheduleBuilder.Create()
                .WithIntervalInHours(schedule.FrequencyInHours);
        }
    }
}
