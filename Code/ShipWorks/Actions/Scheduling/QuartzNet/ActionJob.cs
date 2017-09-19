using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using log4net;
using Quartz;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// IJob implementation for dispatching a scheduled action
    /// </summary>
    [Obfuscation(Exclude = true)]
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
                log.InfoFormat("ActionJob: {0} Starting.", context.JobDetail.Key);

                long actionId = 0;
                long actionIdByJobKeyName = 0;

                // When SW adds default scheduled actions, we can't set the Quartz JobData field in SQL
                // Therefore, we must check the job name to get the action ID if the JobDataMap is empty or has the wrong ActionID
                // We get the ActionID defined by the job name in case we need it.
                long.TryParse(context.JobDetail.Key.Name, out actionIdByJobKeyName);

                // If we don't have an action id in the job data, use the action id from the job name
                if (context.JobDetail.JobDataMap["ActionID"] == null || !long.TryParse(context.JobDetail.JobDataMap["ActionID"].ToString(), out actionId))
                {
                    // We couldn't parse an action id from the job data, so use the action id from the job name
                    actionId = actionIdByJobKeyName;
                }

                // If the two don't match, we'll assume that an invalid one was entered into the job data and use the action id from the job name
                if (actionId != actionIdByJobKeyName)
                {
                    actionId = actionIdByJobKeyName;
                }

                // If we still don't have an action id, delete the job and bail out.
                if (actionId == 0)
                {
                    log.ErrorFormat("ActionID not provided for job with Key: {0}.", context.JobDetail.Key);

                    // Delete Quartz job so it does not continue to run.
                    context.Scheduler.DeleteJob(context.JobDetail.Key);

                    return;
                }

                // If the last purge hasn't finished, don't add another one.
                using (ActionQueueCollection actionQueueCollection = new ActionQueueCollection())
                {
                    SqlAdapter.Default.FetchEntityCollection(actionQueueCollection, new RelationPredicateBucket(ActionQueueFields.ActionID == actionId));
                    if (actionQueueCollection.Any())
                    {
                        ActionEntity action = ActionManager.GetAction(actionId);

                        using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                        {
                            List<ActionTask> tasks = ActionManager.LoadTasks(lifetimeScope, action);
                            if (tasks.Any(t => t.Entity.TaskIdentifier.ToUpperInvariant() == "PurgeDatabase".ToUpperInvariant()))
                            {
                                log.ErrorFormat("ActionID is already in the queue for job with Key: {0}.  Skipping adding this instance.", context.JobDetail.Key);
                                return;
                            }
                        }
                    }
                }

                // Dispatch the action
                ActionDispatcher.DispatchScheduledAction(actionId);

                log.InfoFormat("ActionJob: {0} Ended.", context.JobDetail.Key);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ActionJob: {0} threw the following error: {1}", context.JobDetail.Key, ex.Message);
                throw;
            }
        }
    }
}
