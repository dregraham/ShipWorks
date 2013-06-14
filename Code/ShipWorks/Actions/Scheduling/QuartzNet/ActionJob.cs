using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Job;
using ShipWorks.Data.Model.EntityClasses;
using log4net;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// IJob implementation for dispatching a scheduled action
    /// </summary>
    public class ActionJob : Quartz.IJob
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionJob));

        /// <summary>
        /// Dispatches a scheduled action
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            log.InfoFormat("ActionJob: {0} Running at {1}", context.JobDetail.Key, DateTime.Now.ToString("r"));

            long actionID = 0;

            if (!long.TryParse(context.JobDetail.JobDataMap["ActionID"].ToString(), out actionID))
            {
                log.ErrorFormat("ActionID not provided for job with Key: {0}.", context.JobDetail.Key);
                
                // Delete Quartz job so it does not continue to run.
                context.Scheduler.DeleteJob(context.JobDetail.Key);
                
                return;
            }

            ActionDispatcher.DispatchScheduledAction(actionID);   
        }
    }
}
