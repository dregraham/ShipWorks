using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// An implementation of the ISchedulingEngine interface that will interact with the
    /// Quartz scheduling API.
    /// </summary>
    public class QuartzSchedulingEngine : ISchedulingEngine
    {
        readonly Quartz.ISchedulerFactory schedulerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine"/> class.
        /// </summary>
        public QuartzSchedulingEngine()
            : this(new QuartzSchedulerFactory()) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine"/> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <exception cref="System.ArgumentNullException">schedulerFactory</exception>
        public QuartzSchedulingEngine(Quartz.ISchedulerFactory schedulerFactory)
        {
            if (null == schedulerFactory)
            {
                throw new ArgumentNullException("schedulerFactory");
            }

            this.schedulerFactory = schedulerFactory;
        }

        /// <summary>
        /// Schedules the specified action according to the details of the cron trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        public void Schedule(ActionEntity action, CronTrigger cronTrigger)
        {
            IJobDetail jobDetail = new JobDetailImpl(action.ActionID.ToString(CultureInfo.InvariantCulture), null, typeof (ActionJob));
            SimpleTriggerImpl trigger = new SimpleTriggerImpl(jobDetail.Key.Name, null, cronTrigger.StartDateTimeInUtc);

            schedulerFactory.GetScheduler().ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// Determines whether a job for the given action/trigger exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action/trigger; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExistingJob(ActionEntity action, CronTrigger cronTrigger)
        {
            JobKey jobKey = new JobKey(action.ActionID.ToString(CultureInfo.InvariantCulture));
            return schedulerFactory.GetScheduler().GetJobDetail(jobKey) != null;
        }
        
        /// <summary>
        /// Runs the scheduler engine, which queues actions based on the scheduled cron triggers.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel (stop) the engine.</param>
        /// <returns>The running engine task.</returns>
        public Task RunAsync(CancellationToken cancellationToken)
        {
            var scheduler = schedulerFactory.GetScheduler();

            scheduler.Start();

            var taskSource = new TaskCompletionSource<object>();

            cancellationToken.Register(() => {
                scheduler.Shutdown(true);
                taskSource.SetCanceled();
            });

            return taskSource.Task;
        }
    }
}
