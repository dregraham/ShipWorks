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
    public class ActionJob : Quartz.IJob
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionJob));

        public void Execute(IJobExecutionContext context)
        {
            log.InfoFormat("ActionJob: {0} Running at {1}", context.JobDetail.Key, DateTime.Now.ToString("r"));

            long actionID = 0;

            if (!long.TryParse(context.JobDetail.JobDataMap["ActionID"].ToString(), out actionID))
            {
                log.ErrorFormat("ActionID not provided for job with Key: {0}.", context.JobDetail.Key);
                // TODO: Disable ActionEntity so that it does not continue to run.
                // TODO: Add message to SW UI boxes (at the top) to inform the user the action has been disabled due to invalid scheduling data.
                // TODO: Disable Quartz job so it does not continue to run.  Would need to enable it when the SW Action is re-enabled.
                
                return;
            }

            // TODO: Do we need to catch exceptions?  Need to make sure the service does not exit.
            ActionDispatcher.DispatchScheduledAction(actionID);   
        }
    }
}
