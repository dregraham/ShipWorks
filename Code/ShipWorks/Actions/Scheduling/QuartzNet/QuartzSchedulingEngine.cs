using System.Collections.Generic;
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
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();

            if (IsExistingJob(action, cronTrigger))
            {
                // Simplest way to update a job in Quartz is to delete it along with all of its triggers
                // (http://stackoverflow.com/questions/6728012/quartz-net-update-delete-jobs-triggers)
                RemoveJob(action, quartzScheduler);
            }

            IJobDetail jobDetail = new JobDetailImpl(action.ActionID.ToString(CultureInfo.InvariantCulture), null, typeof(ActionJob));
            SimpleTriggerImpl trigger = new SimpleTriggerImpl(jobDetail.Key.Name, null, cronTrigger.StartDateTimeInUtc);
            
            quartzScheduler.ScheduleJob(jobDetail, trigger);
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
        /// Unschedules the specified action by removing the job/action from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Unschedule(ActionEntity action)
        {
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();
            RemoveJob(action, quartzScheduler);
        }

        /// <summary>
        /// Removes the job from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="quartzScheduler">The quartz scheduler.</param>
        private void RemoveJob(ActionEntity action, Quartz.IScheduler quartzScheduler)
        {
            if (action != null)
            {
                JobKey jobKey = new JobKey(action.ActionID.ToString(CultureInfo.InvariantCulture));

                // We need to detach the job from any triggers before deleting the job
                IList<ITrigger> existingTriggers = schedulerFactory.GetScheduler().GetTriggersOfJob(jobKey);
                foreach (ITrigger existingTrigger in existingTriggers)
                {
                    quartzScheduler.UnscheduleJob(existingTrigger.Key);
                }

                quartzScheduler.DeleteJob(jobKey);
            }
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
