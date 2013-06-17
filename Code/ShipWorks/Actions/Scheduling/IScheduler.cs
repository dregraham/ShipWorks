using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;


namespace ShipWorks.Actions.Scheduling
{
    /// <summary>
    /// The scheduler is a facade that will create, edit, and delete scheduled actions/jobs in ShipWorks. 
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Schedules the specified action to run at the time specified by the scheduled trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="scheduledTrigger">The scheduled trigger.</param>
        void ScheduleAction(ActionEntity action, ScheduledTrigger scheduledTrigger);

        /// <summary>
        /// Removes the specified action/trigger from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="scheduledTrigger">The scheduled trigger.</param>
        void UnscheduleAction(ActionEntity action, ScheduledTrigger scheduledTrigger);

        /// <summary>
        /// Runs the scheduler engine, which queues actions based on the scheduled triggers.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
