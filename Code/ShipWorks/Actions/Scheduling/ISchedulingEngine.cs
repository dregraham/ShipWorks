using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Triggers;
using System.Threading.Tasks;
using System.Threading;

namespace ShipWorks.Actions.Scheduling
{
    public interface ISchedulingEngine
    {
        /// <summary>
        /// Schedules the specified action according to the details of the scheduled trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="scheduledTrigger">The scheduled trigger.</param>
        void Schedule(ActionEntity action, ScheduledTrigger scheduledTrigger);

        /// <summary>
        /// Determines whether a job for the given action/trigger exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="scheduledTrigger">The scheduled trigger.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action/trigger; otherwise, <c>false</c>.
        /// </returns>
        bool IsExistingJob(ActionEntity action, ScheduledTrigger scheduledTrigger);

        /// <summary>
        /// Unschedules the specified action by removing the job/action from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        void Unschedule(ActionEntity action);

        /// <summary>
        /// Runs the scheduler engine, which queues actions based on the scheduled triggers.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
