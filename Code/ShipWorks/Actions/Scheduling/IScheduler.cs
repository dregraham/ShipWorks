using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Scheduling.ActionSchedules;


namespace ShipWorks.Actions.Scheduling
{
    /// <summary>
    /// The scheduler is a facade that will create, edit, and delete scheduled actions/jobs in ShipWorks. 
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Schedules the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="schedule">The schedule.</param>
        void ScheduleAction(ActionEntity action, ActionSchedule schedule);

        /// <summary>
        /// Removes the specified action from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="schedule">The schedule.</param>
        void UnscheduleAction(ActionEntity action, ActionSchedule schedule);

        /// <summary>
        /// Runs the scheduler engine, which queues ShipWorks actions.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
