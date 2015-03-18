using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Interaction;
using System.Threading;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;
using ShipWorks.Actions.Tasks;
using ShipWorks.Filters;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;
using ShipWorks.Data.Utility;
using System.Transactions;
using System.Data.SqlClient;
using ShipWorks.SqlServer.Filters;
using ShipWorks.Data.Adapter;
using ShipWorks.Stores;
using ShipWorks.Common.Threading;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Responsible for running actions
    /// </summary>
    public class ActionProcessor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionProcessor));

        // Makes sure the database doesn't change while running actions
        static ApplicationBusyToken busyToken;
        static object runningLock = new object();

        ActionQueueGateway gateway;

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
        private static void WorkerThread(object state)
        {
            try
            {
                bool anyWorkToDo = false;

                SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "ActionProcessor.WorkerThread cleaning up queues.");
                sqlAdapterRetry.ExecuteWithRetry(() => 
                    {
                        using (SqlConnection sqlConnection = SqlSession.Current.OpenConnection())
                        {
                            anyWorkToDo = AnyWorkToDo(sqlConnection);
                            if (anyWorkToDo)
                            {
                                CleanupQueues(sqlConnection);
                            }
                        }
                    });

                if (!anyWorkToDo)
                {
                    return;
                }

                log.InfoFormat("Starting action processor");
                ActionProcessor processor = new ActionProcessor(new ActionQueueGatewayStandard());
                processor.ProcessQueues();
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
        /// Checks the queue to see if there's any work to do.  
        /// </summary>
        private static bool AnyWorkToDo(SqlConnection sqlConnection)
        {
            // First see if there are any to process
            using (SqlCommand cmd = SqlCommandProvider.Create(sqlConnection))
            {
                // Only process the scheduled actions based on the action manager configuration
                cmd.CommandText = "SELECT TOP(1) ActionQueueID FROM ActionQueue WHERE ActionQueueType = @ExecutionModeActionQueueType";
                cmd.Parameters.AddWithValue("@ExecutionModeActionQueueType", (int) ActionManager.ExecutionModeActionQueueType);

                // If the UI isn't running somehwere, and we are the background process, go ahead and do UI actions too since it's not open
                if (!Program.ExecutionMode.IsUISupported && !UserInterfaceExecutionMode.IsProcessRunning)
                {
                    // Additionally process UI actions if the UI is not running
                    cmd.CommandText += " OR ActionQueueType = @UIActionQueueType";
                    cmd.Parameters.AddWithValue("@UIActionQueueType", (int) ActionQueueType.UserInterface);
                }

                if (SqlCommandProvider.ExecuteScalar(cmd) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Cleans up queues
        /// </summary>
        private static void CleanupQueues(SqlConnection sqlConnection)
        {
            // Null out any context ownership for contexts that are no longer active.  This should only happen if a ShipWorks blows up during processing a context.
            using (SqlCommand cmd = SqlCommandProvider.Create(sqlConnection))
            {
                cmd.CommandText = @"
                        UPDATE ActionQueue
                            SET ContextLock = NULL
                            WHERE ContextLock IS NOT NULL 
                            AND APPLOCK_TEST('public', ContextLock, 'Exclusive', 'Session') = 1";

                int updated = SqlCommandProvider.ExecuteNonQuery(cmd);
                if (updated > 0)
                {
                    log.InfoFormat("ContextLock removed from {0} queues", updated);
                }
            }

            // Now change any Postponed queues back to running if they don't have a ContextLock.  This would be for queues that were postponed, but then ShipWorks
            // blew up while they were being processed.
            using (SqlCommand cmd = SqlCommandProvider.Create(sqlConnection))
            {
                cmd.CommandText = @"
                        UPDATE ActionQueue 
                            SET Status = @incomplete
                            WHERE Status = @postponed 
                                AND ContextLock IS NULL";

                cmd.Parameters.AddWithValue("@incomplete", (int)ActionQueueStatus.Incomplete);
                cmd.Parameters.AddWithValue("@postponed", (int)ActionQueueStatus.Postponed);

                int updated = SqlCommandProvider.ExecuteNonQuery(cmd);
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
                            TimeSpan maxWait = TimeSpan.FromSeconds(15);

                            while (timer.Elapsed < maxWait && queueList.Count == 0)
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
