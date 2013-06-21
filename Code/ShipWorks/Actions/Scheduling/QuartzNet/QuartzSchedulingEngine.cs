using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using ShipWorks.Actions.Scheduling.ActionSchedules;
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
        readonly IActionScheduleAdapter scheduleAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine"/> class.
        /// </summary>
        public QuartzSchedulingEngine()
            : this(new SqlSchedulerFactory(), new ReflectingActionScheduleAdapter()) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine"/> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <param name="scheduleAdapter">The action schedule adapter.</param>
        /// <exception cref="System.ArgumentNullException">schedulerFactory</exception>
        public QuartzSchedulingEngine(Quartz.ISchedulerFactory schedulerFactory, IActionScheduleAdapter scheduleAdapter)
        {
            if (null == schedulerFactory)
                throw new ArgumentNullException("schedulerFactory");
            if (null == scheduleAdapter)
                throw new ArgumentNullException("scheduleAdapter");

            this.schedulerFactory = schedulerFactory;
            this.scheduleAdapter = scheduleAdapter;
        }

        /// <summary>
        /// Schedules the specified action according to the details of the schedule.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="schedule">The schedule.</param>
        public void Schedule(ActionEntity action, ActionSchedule schedule)
        {
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();

            try
            {
                if (IsExistingJob(action, quartzScheduler))
                {
                    // Simplest way to update a job in Quartz is to delete it along with all of its triggers
                    // (http://stackoverflow.com/questions/6728012/quartz-net-update-delete-jobs-triggers)
                    RemoveJob(action, quartzScheduler);
                }

                var job = JobBuilder.Create<ActionJob>()
                    .WithIdentity(GetQuartzJobName(action))
                    .UsingJobData("ActionID", action.ActionID.ToString(CultureInfo.InvariantCulture))
                    .Build();

                var adaptedSchedule = scheduleAdapter.Adapt(schedule);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(job.Key.Name)
                    .StartAt(schedule.StartDateTimeInUtc)
                    .WithSchedule(adaptedSchedule.ScheduleBuilder);

                if (null != adaptedSchedule.Calendar)
                {
                    quartzScheduler.AddCalendar(job.Key.Name, adaptedSchedule.Calendar, true, true);
                    trigger = trigger.ModifiedByCalendar(job.Key.Name);
                }

                quartzScheduler.ScheduleJob(job, trigger.Build());
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
        /// Determines whether a job for the given action exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action/trigger; otherwise, <c>false</c>.
        /// </returns>
        public bool HasExistingSchedule(ActionEntity action)
        {
            Quartz.IScheduler quartzScheduler = schedulerFactory.GetScheduler();

            try
            {
                return IsExistingJob(action, quartzScheduler);
            }
            finally
            {
                // The scheduler needs to be explicitly shutdown(even though it was never actually started) 
                // otherwise the process will continue to run after exiting the ShipWorks UI
                quartzScheduler.Shutdown(true);
            }
        }

        /// <summary>
        /// Determines whether a job for the given action exists.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="quartzScheduler">The quartz scheduler.</param>
        /// <returns>
        ///   <c>true</c> if [a job exists] for the given action; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsExistingJob(ActionEntity action, Quartz.IScheduler quartzScheduler)
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
                quartzScheduler.DeleteCalendar(jobKey.Name);
            }
        }

        /// <summary>
        /// There are numerous points in the engine where the job must be retreived by name, so this is just a helper
        /// method to create/get the name of the quartz job based on the action ID of the action provided.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The name of the job.</returns>
        private static string GetQuartzJobName(ActionEntity action)
        {
            // The job name must be unique, so just use action ID for the job name
            return action.ActionID.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Runs the scheduler engine, which queues actions based on the scheduled triggers.
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
