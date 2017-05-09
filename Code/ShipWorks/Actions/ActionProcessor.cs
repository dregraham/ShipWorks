using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autofac;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Responsible for running actions
    /// </summary>
    public class ActionProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ActionProcessor));
        private readonly TimeSpan maxWaitForPostponed = TimeSpan.FromSeconds(15);

        // Makes sure the database doesn't change while running actions
        private static ApplicationBusyToken busyToken;
        private static object runningLock = new object();

        private ActionQueueGateway gateway;

        /// <summary>
        /// Raised each time an action is attempted to be processed, regardless of the outcome
        /// </summary>
        public event ActionProcessedEventHandler ActionProcessed;

        /// <summary>
        /// Createa a new instance of the processor that will use the given gateway for its queue source
        /// </summary>
        public ActionProcessor(ActionQueueGateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            this.gateway = gateway;
        }

        /// <summary>
        /// The type of gateway this action processor will be using.
        /// </summary>
        public ActionQueueGatewayType GatewayType => gateway.GatewayType;

        /// <summary>
        /// Run any tasks that have been queued for running.  This method does not wait for the work to complete, the work is put on a background thread.
        /// </summary>
        public static void StartProcessing()
        {
            lock (runningLock)
            {
                // Already running
                if (busyToken != null)
                {
                    return;
                }

                if (ApplicationBusyManager.TryOperationStarting("running actions", out busyToken))
                {
                    if (Program.ExecutionMode.IsUIDisplayed)
                    {
                        ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(WorkerThread));
                    }
                    else
                    {
                        var thread = new Thread(ExceptionMonitor.WrapThread(WorkerThread));
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Worker thread for running action tasks
        /// </summary>
        private static async void WorkerThread(object state)
        {
            try
            {
                using (new LoggedStopwatch(log, "ActionRunTime"))
                {
                    List<Task> actionProcessorTasks = new List<Task>();

                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        foreach (ActionProcessor actionProcessor in lifetimeScope.Resolve<IActionProcessorFactory>().CreateStandard())
                        {
                            actionProcessorTasks.Add(Task.Run(() =>
                            {
                                if (actionProcessor.AnyWorkToDo())
                                {
                                    actionProcessor.ProcessQueues();
                                }
                            }));
                        }

                        await Task.WhenAll(actionProcessorTasks);
                    }
                }
            }
            finally
            {
                lock (runningLock)
                {
                    ApplicationBusyManager.OperationComplete(busyToken);
                    busyToken = null;
                }
            }
        }

        /// <summary>
        /// See if there is any work to do, and if so, cleanup any abandoned queues
        /// </summary>
        /// <returns>True if there is work to do</returns>
        private bool AnyWorkToDo()
        {
            bool anyWorkToDo = false;

            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "ActionProcessor.WorkerThread cleaning up queues.");
            sqlAdapterRetry.ExecuteWithRetry(() =>
            {
                using (DbConnection sqlConnection = SqlSession.Current.OpenConnection())
                {
                    anyWorkToDo = gateway.AnyWorkToDo(sqlConnection);
                    if (anyWorkToDo)
                    {
                        CleanupQueues(sqlConnection);
                    }
                }
            });

            return anyWorkToDo;
        }

        /// <summary>
        /// Cleans up queues
        /// </summary>
        private static void CleanupQueues(DbConnection sqlConnection)
        {
            // Null out any context ownership for contexts that are no longer active.  This should only happen if a ShipWorks blows up during processing a context.
            using (DbCommand cmd = DbCommandProvider.Create(sqlConnection))
            {
                cmd.CommandText = @"
                        UPDATE ActionQueue
                            SET ContextLock = NULL
                            WHERE ContextLock IS NOT NULL
                            AND APPLOCK_TEST('public', ContextLock, 'Exclusive', 'Session') = 1";

                int updated = DbCommandProvider.ExecuteNonQuery(cmd);
                if (updated > 0)
                {
                    log.InfoFormat("ContextLock removed from {0} queues", updated);
                }
            }

            // Now change any Postponed queues back to running if they don't have a ContextLock.  This would be for queues that were postponed, but then ShipWorks
            // blew up while they were being processed.
            using (DbCommand cmd = DbCommandProvider.Create(sqlConnection))
            {
                cmd.CommandText = @"
                        UPDATE ActionQueue
                            SET Status = @incomplete
                            WHERE Status = @postponed
                                AND ContextLock IS NULL";

                cmd.AddParameterWithValue("@incomplete", (int) ActionQueueStatus.Incomplete);
                cmd.AddParameterWithValue("@postponed", (int) ActionQueueStatus.Postponed);

                int updated = DbCommandProvider.ExecuteNonQuery(cmd);
                if (updated > 0)
                {
                    log.InfoFormat("Updated {0} ActionQueue records back to incomplete from postponed.", updated);
                }
            }
        }

        /// <summary>
        /// Process all the queues returned by the configured gateway
        /// </summary>
        public void ProcessQueues()
        {
            // Make sure we have the latest set of actions
            ActionManager.CheckForChangesNeeded();

            using (ActionProcessingContext context = new ActionProcessingContext())
            {
                // This loop is to keep processing until we didn't have any postponed steps to flush.  When we flush postponed steps
                // at the end of a context, it leaves the queue's they go with back in the "incomplete\running" state.  Relooping
                // after that picks those back up and kicks them off again.
                do
                {
                    // Make's sure we keep getting the next page of queues during a single iteration.
                    long lastQueueID = 0;

                    log.Debug($"Processing Queues for {EnumHelper.GetDescription(GatewayType)}");

                    // Fetch all queued actions
                    List<long> queueList = GetNextQueuePage(lastQueueID);

                    // This loop is for a single context to keep going while queues are still found to process
                    while (queueList.Count > 0)
                    {
                        // Execute all the tasks for each queued action instance
                        foreach (long queueID in queueList)
                        {
                            lastQueueID = queueID;
                            int postponedCount = context.Postponements.Count;

                            using (ActionRunner runner = new ActionRunner(queueID, context))
                            {
                                // Copy the queue at the start - this is just for the benefit if anyone using the processed event
                                ActionQueueEntity queueAtStart = EntityUtility.CloneEntity(runner.ActionQueue);

                                // Run the queue
                                ActionRunnerResult result = runner.RunQueue();

                                // Raise the event
                                ActionProcessedEventHandler handler = ActionProcessed;
                                if (handler != null)
                                {
                                    handler(this, new ActionProcessedEventArgs(queueID, result, queueAtStart, EntityUtility.CloneEntity(runner.ActionQueue)));
                                }
                            }

                            // If the the postponed count goes down, it means the queue consumed some previously postponed tasks.  When that happens,
                            // we want to start our processing loop all over.  This is because consuming postpone tasks sets there queues back in motion and ready to go.
                            // We want to go back and let those run before moving on again.
                            if (context.Postponements.Count < postponedCount)
                            {
                                log.InfoFormat("Restarting queue processing loop due to consumed postponed steps. {0}->{1}", postponedCount, context.Postponements.Count);

                                // This breaks the foreach loop, and forces us to get the queueList again from the beginning
                                lastQueueID = 0;
                                break;
                            }
                        }

                        // Get the next page
                        queueList = GetNextQueuePage(lastQueueID);

                        // If there aren't any more, but we have some postponed, then wait for more actions to come in.  For instance,
                        // to wait for more labels to be processed to fill in a label sheet
                        if (queueList.Count == 0 && context.Postponements.Count > 0 && gateway.CanNewQueuesArrive)
                        {
                            log.InfoFormat("Waiting for more actions to fill in postponed setups.");

                            Stopwatch timer = Stopwatch.StartNew();

                            while (timer.Elapsed < maxWaitForPostponed && queueList.Count == 0)
                            {
                                Thread.Sleep(TimeSpan.FromSeconds(2));
                                queueList = GetNextQueuePage(lastQueueID);
                            }
                        }
                    }

                    // If there are still postponed steps in the context, we have to force them to complete with what they can now
                } while (FlushPostponedQueues(context));
            }
        }

        /// <summary>
        /// Gets the next page of action queues to process.
        /// This is done withing a SqlAdapterRetry.
        /// </summary>
        private List<long> GetNextQueuePage(long lastQueueID)
        {
            List<long> page = new List<long>();

            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "ActionProcessor.GetNextQueuePage");
            sqlAdapterRetry.ExecuteWithRetry(() =>
            {
                page = gateway.GetNextQueuePage(lastQueueID);
            });

            return page;
        }

        /// <summary>
        /// Flush all the postponed queues in the context, forcing them to complete whether they are "full" or not.  Returns true if there were
        /// any to flush, and false otherwise.
        /// </summary>
        private bool FlushPostponedQueues(ActionProcessingContext context)
        {
            if (context.Postponements.Count == 0)
            {
                return false;
            }

            log.InfoFormat("Flushing all postponed queues");

            context.FlushingPostponed = true;

            // Just flush the last one.. to kind of "rewind" to the most recent, which will actually
            // keep things in order, since it'd be like when we were going the last one, it went
            // ahead without postponing.  We only flush the last one, since flushing it may release other steps
            // that full complete other postponed queues, rather than forcing them to flush now imcomplete.
            //
            //
            // For example, if an action was "Print Labels(4up), Print Report, Print Standard", and they printed 5 orders.  If we processed all the postponed now,
            // we would end up outputing:
            //  1.  A full sheet of 4 labels
            //  2.  Then we'd flush one sheet of 1 label
            //  3.  BUT then we'd flush a report with the 4 steps that had been postponed - the 5th queue hadn't gotten to the report step yet.
            //
            ActionPostponement postponement = context.Postponements.Last();
            context.Postponements.Remove(postponement);

            ActionQueueEntity queueAtStart = EntityUtility.CloneEntity(postponement.Queue);

            // Take the queue out of the postponed state
            postponement.Queue.Status = (int) ActionQueueStatus.Incomplete;
            postponement.Step.StepStatus = (int) ActionQueueStepStatus.Pending;

            // Run the queue
            using (ActionRunner runner = new ActionRunner(postponement.Queue, context))
            {
                ActionRunnerResult result = runner.RunQueue();

                ActionProcessedEventHandler handler = ActionProcessed;
                if (handler != null)
                {
                    handler(this, new ActionProcessedEventArgs(postponement.Queue.ActionQueueID, result, queueAtStart, EntityUtility.CloneEntity(runner.ActionQueue)));
                }
            }

            context.FlushingPostponed = false;

            return true;
        }
    }
}
