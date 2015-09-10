using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using ShipWorks.Tests.Integration.MSTest.Utilities;

namespace ShipWorks.Tests.Integration.MSTest.Actions.Scheduling
{
    /// <summary>
    /// A QuartzJob listener that implements IJobListener
    /// </summary>
    public class QuartzJobListener :IJobListener
    {
        public event JobExecuted JobExecutedHandler;
        public delegate void JobExecuted(string actionID, DateTime fireTime, DateTime nextScheduledFireTime);

        /// <summary>
        /// Job executed event fired by the Quartz scheduler.  
        /// If JobExecutedHandler is not null, it will raise the JobExecutedHandler event.
        /// </summary>
        public virtual void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            string actionID = context.JobDetail.JobDataMap["ActionID"].ToString();

            DateTime nextFireTime = context.NextFireTimeUtc.Value.LocalDateTime;

            if (JobExecutedHandler != null)
            {
                JobExecutedHandler(actionID, DateTime.Now, nextFireTime);
            }
        }

        /// <summary>
        /// Job about to be executed event fired by the Quartz scheduler.  
        /// </summary>
        public virtual void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        /// <summary>
        /// Job execution vetoed event fired by the Quartz scheduler.  
        /// </summary>
        public virtual void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        /// <summary>
        /// Job listener name.
        /// </summary>
        public virtual string Name
        {
            get { return "Quartz Job Listener"; }
        }
    }
}
