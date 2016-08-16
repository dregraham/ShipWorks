using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Quartz.Spi;
using System.Linq;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// An implementation of the ISchedulingEngine interface that will interact with the
    /// Quartz scheduling API.
    /// </summary>
    public class QuartzSchedulingEngine : ISchedulingEngine
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly IActionScheduleAdapter scheduleAdapter;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine"/> class.
        /// </summary>
        public QuartzSchedulingEngine()
            : this(new SqlSchedulerFactory(), new ReflectingActionScheduleAdapter(), LogManager.GetLogger(typeof(QuartzSchedulingEngine)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzSchedulingEngine" /> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <param name="scheduleAdapter">The action schedule adapter.</param>
        /// <param name="log">The log.</param>
        /// <exception cref="System.ArgumentNullException">schedulerFactory or scheduleAdapter or log</exception>
        public QuartzSchedulingEngine(ISchedulerFactory schedulerFactory, IActionScheduleAdapter scheduleAdapter, ILog log)
        {
            if (null == schedulerFactory)
            {
                throw new ArgumentNullException("schedulerFactory");
            }
            if (null == scheduleAdapter)
            {
                throw new ArgumentNullException("scheduleAdapter");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            this.schedulerFactory = schedulerFactory;
            this.scheduleAdapter = scheduleAdapter;
            this.log = log;
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
                    log.InfoFormat("The {0} action (ID {1}) is an existing scheduled job. Preparing to update the action by removing it and re-adding it with the updated settings.", action.Name, action.ActionID);
                    RemoveJob(action, quartzScheduler);
                }

                var job = JobBuilder.Create<ActionJob>()
                    .WithIdentity(GetQuartzJobName(action))
                    .UsingJobData("ActionID", action.ActionID.ToString(CultureInfo.InvariantCulture))
                    .Build();

                var adaptedSchedule = scheduleAdapter.Adapt(schedule);

                // Create a trigger builder based on common properties
                TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                    .WithIdentity(job.Key.Name)
                    .StartAt(schedule.StartDateTimeInUtc)
                    .WithSchedule(adaptedSchedule.ScheduleBuilder);

                // If the scheduler should end at a specific time, add the end date and time
                if (schedule.EndsOnType == ActionEndsOnType.SpecificDateTime)
                {
                    triggerBuilder.EndAt(schedule.EndDateTimeInUtc);
                }

                // Build the trigger
                IOperableTrigger trigger = (IOperableTrigger) triggerBuilder.Build();

                if (!TriggerUtils.ComputeFireTimes(trigger, adaptedSchedule.Calendar, 1).Any())
                    throw new SchedulingException("Based on the configured schedule, the action will never execute.");

                if (null != adaptedSchedule.Calendar)
                {
                    trigger.CalendarName = job.Key.Name;
                    quartzScheduler.AddCalendar(trigger.CalendarName, adaptedSchedule.Calendar, true, true);
                }

                try
                {
                    quartzScheduler.ScheduleJob(job, trigger);
                }
                catch
                {
                    if (null != trigger.CalendarName)
                        quartzScheduler.DeleteCalendar(trigger.CalendarName);
                    throw;
                }

                log.InfoFormat("The {0} action (ID {1}) has been scheduled.", action.Name, action.ActionID);
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

                log.InfoFormat("The {0} scheduled action (ID {1}) has been removed. The job and its triggers and/or calenders for the action have been deleted.", action.Name, action.ActionID);
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
