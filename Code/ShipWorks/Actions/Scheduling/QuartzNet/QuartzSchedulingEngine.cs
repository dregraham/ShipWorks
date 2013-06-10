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

            try
            {
                if (IsExistingJob(action, cronTrigger, quartzScheduler))
                {
                    // Simplest way to update a job in Quartz is to delete it along with all of its triggers
                    // (http://stackoverflow.com/questions/6728012/quartz-net-update-delete-jobs-triggers)
                    RemoveJob(action, quartzScheduler);
                }

                IJobDetail jobDetail = new JobDetailImpl(GetQuartzJobName(action), null, typeof (ActionJob));
                jobDetail.JobDataMap.Add("ActionID", action.ActionID.ToString(CultureInfo.InvariantCulture));

                SimpleTriggerImpl trigger = new SimpleTriggerImpl(jobDetail.Key.Name, null, cronTrigger.StartDateTimeInUtc);

                quartzScheduler.ScheduleJob(jobDetail, trigger);
            }
            finally
            {
                if (quartzScheduler != null)
                {
                    // The scheduler needs to be explicitly shutdown(even though it was never actually started) 
                    // otherwise the process will continue to run after exiting the ShipWorks UI
                    quartzScheduler.Shutdown(true);
                }
            }
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
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();

            try
            {
                JobKey jobKey = new JobKey(GetQuartzJobName(action));
                return quartzScheduler.GetJobDetail(jobKey) != null;
            }
            finally
            {
                // The scheduler needs to be explicitly shutdown(even though it was never actually started) 
                // otherwise the process will continue to run after exiting the ShipWorks UI
                quartzScheduler.Shutdown(true);
            }
        }

        /// <summary>
        /// Determines whether a job for the given action/trigger exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        /// <param name="quartzScheduler">The quartz scheduler.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action/trigger; otherwise, <c>false</c>.
        /// </returns>
        private bool IsExistingJob(ActionEntity action, CronTrigger cronTrigger, Quartz.IScheduler quartzScheduler)
        {
            JobKey jobKey = new JobKey(GetQuartzJobName(action));
            return quartzScheduler.GetJobDetail(jobKey) != null;
        }

        /// <summary>
        /// Unschedules the specified action by removing the job/action from the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Unschedule(ActionEntity action)
        {
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();

            try
            {
                RemoveJob(action, quartzScheduler);
            }
            finally
            {
                if (quartzScheduler != null)
                {
                    // The scheduler needs to be explicitly shutdown(even though it was never actually started) 
                    // otherwise the process will continue to run after exiting the ShipWorks UI
                    quartzScheduler.Shutdown(true);
                }
            }
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
                JobKey jobKey = new JobKey(GetQuartzJobName(action));

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
        /// There are numerous points in the engine where the job must be retreived by name, so this is just a helper
        /// method to create/get the name of the quartz job based on the name and action ID of the action provided.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The name of the job.</returns>
        private string GetQuartzJobName(ActionEntity action)
        {
            // The job name must be unique, so include the action ID in the name
            return string.Format("{0} (ID {1})", action.Name, action.ActionID.ToString(CultureInfo.InvariantCulture));
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
