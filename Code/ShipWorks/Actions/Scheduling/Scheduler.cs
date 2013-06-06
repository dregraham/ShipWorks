using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Triggers;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Threading;

namespace ShipWorks.Actions.Scheduling
{
    /// <summary>
    /// The Scheduler class is a facade that will create, edit, and delete scheduled actions/jobs in ShipWorks. 
    /// </summary>
    public class Scheduler : IScheduler
    {
        private readonly ISchedulingEngine schedulingEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class. This version of the 
        /// constructor will cause the Scheduler to use the Quartz scheduling engine by default.
        /// </summary>
        public Scheduler()
            : this (new QuartzSchedulingEngine())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class. This version of the
        /// constructor is primarily used for testing purposes.
        /// </summary>
        /// <param name="schedulingEngine">The scheduling engine.</param>
        public Scheduler(ISchedulingEngine schedulingEngine)
        {
            this.schedulingEngine = schedulingEngine;
        }

        /// <summary>
        /// Schedules the specified action to run at the time specified by the cron trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        public void ScheduleAction(ActionEntity action, CronTrigger cronTrigger)
        {
            if (!schedulingEngine.IsExistingJob(action, cronTrigger))
            {
                // New jobs/actions cannot be scheduled to occur in the past
                if (cronTrigger.StartDateTimeInUtc <= DateTime.UtcNow)
                {
                    throw new SchedulingException("The start date must be in the future when scheduling a new action.");
                }
            }

            try
            {
                // Delegate to the scheduling engine to take care of the details of scheduling the action
                schedulingEngine.Schedule(action, cronTrigger);
            }
            catch (Exception exception)
            {
                string message = string.Format("An error occurred while scheduling the action: {0}", exception.Message);
                throw new SchedulingException(message, exception);
            }
            
        }
        
        /// <summary>
        /// Runs the scheduler engine, which queues actions based on the scheduled cron triggers.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        public Task RunAsync(CancellationToken cancellationToken)
        {
            return schedulingEngine.RunAsync(cancellationToken);
        }
    }
}
