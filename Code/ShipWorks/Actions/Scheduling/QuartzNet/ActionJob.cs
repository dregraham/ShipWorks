using System;
using Quartz;
using log4net;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// IJob implementation for dispatching a scheduled action
    /// </summary>
    public class ActionJob : Quartz.IJob
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionJob"/> class.
        /// </summary>
        public ActionJob()
            : this(LogManager.GetLogger(typeof(ActionJob)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionJob"/> class. This
        /// version of the constructor is primarily for testin purposes.
        /// </summary>
        /// <param name="log">The log.</param>
        public ActionJob(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Dispatches a scheduled action
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                log.InfoFormat("ActionJob: {0} Starting at {1}", context.JobDetail.Key, DateTime.Now.ToString("r"));

                long actionId = 0;

                if (context.JobDetail.JobDataMap["ActionID"] == null || !long.TryParse(context.JobDetail.JobDataMap["ActionID"].ToString(), out actionId))
                {
                    log.ErrorFormat("ActionID not provided for job with Key: {0}.", context.JobDetail.Key);

                    // Delete Quartz job so it does not continue to run.
                    context.Scheduler.DeleteJob(context.JobDetail.Key);

                    return;
                }

                ActionDispatcher.DispatchScheduledAction(actionId);

                log.InfoFormat("ActionJob: {0} Ending at {1}", context.JobDetail.Key, DateTime.Now.ToString("r"));
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ActionJob: {0} threw the following error. {1}", context.JobDetail.Key, ex.Message);
                throw;
            }
        }
    }
}
