using ShipWorks.Actions.Scheduling.ActionSchedules;
using System;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Adapts a specific ShipWorks action schedule type to a Quartz schedule builder.
    /// Derived types in this assembly will be automatically discovered by the default <see cref="ReflectingActionScheduleAdapter"/>.
    /// </summary>
    /// <typeparam name="TSchedule">The action schedule type.</typeparam>
    public abstract class ActionScheduleAdapter<TSchedule> : IActionScheduleAdapter
        where TSchedule : ActionSchedule
    {
        Quartz.IScheduleBuilder IActionScheduleAdapter.Adapt(ActionSchedule schedule)
        {
            if (null == schedule)
                throw new ArgumentNullException("schedule");

            return Adapt((TSchedule)schedule);
        }

        public abstract Quartz.IScheduleBuilder Adapt(TSchedule schedule);
    }
}
