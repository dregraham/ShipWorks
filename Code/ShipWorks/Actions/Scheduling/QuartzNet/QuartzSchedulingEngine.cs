using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
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
        static Quartz.ISchedulerFactory CreateDefaultSchedulerFactory()
        {
            var factory = new Quartz.Impl.StdSchedulerFactory();

            return factory;
        }


        readonly Quartz.ISchedulerFactory schedulerFactory;

        public QuartzSchedulingEngine()
            : this(CreateDefaultSchedulerFactory()) { }

        public QuartzSchedulingEngine(Quartz.ISchedulerFactory schedulerFactory)
        {
            if (null == schedulerFactory)
                throw new ArgumentNullException("schedulerFactory");
            this.schedulerFactory = schedulerFactory;
        }


        /// <summary>
        /// Schedules the specified action according to the details of the cron trigger.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cronTrigger">The cron trigger.</param>
        public void Schedule(ActionEntity action, CronTrigger cronTrigger)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the schedule/trigger from the scheduling engine that the action
        /// is configured with.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A CronTrigger object.</returns>
        public CronTrigger GetTrigger(ActionEntity action)
        {
            throw new NotImplementedException();
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
