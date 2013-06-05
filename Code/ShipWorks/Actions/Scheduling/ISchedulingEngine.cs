using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Triggers;

namespace ShipWorks.Actions.Scheduling
{
    public interface ISchedulingEngine
    {
        /// <summary>
        /// Schedules the specified action according to the details of the cron trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        void Schedule(ActionEntity action, CronTrigger cronTrigger);

        /// <summary>
        /// Determines whether a job for the given action/trigger exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action/trigger; otherwise, <c>false</c>.
        /// </returns>
        bool IsExistingJob(ActionEntity action, CronTrigger cronTrigger);

        /// <summary>
        /// Gets the schedule/trigger from the scheduling engine that the action
        /// is configured with.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A CronTrigger object.</returns>
        CronTrigger GetTrigger(ActionEntity action);
    }
}
