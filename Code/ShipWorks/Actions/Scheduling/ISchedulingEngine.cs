using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using System.Threading.Tasks;
using System.Threading;

namespace ShipWorks.Actions.Scheduling
{
    public interface ISchedulingEngine
    {
        /// <summary>
        /// Schedules the specified action according to the details of the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="schedule">The schedule.</param>
        void Schedule(ActionEntity action, ActionSchedule schedule);

        /// <summary>
        /// Determines whether a job for the given action exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action; otherwise, <c>false</c>.
        /// </returns>
        bool HasExistingSchedule(ActionEntity action);

        /// <summary>
        /// Unschedules the specified action by removing the job/action from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        void Unschedule(ActionEntity action);

        /// <summary>
        /// Runs the scheduler engine, which queues ShipWorks actions.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
