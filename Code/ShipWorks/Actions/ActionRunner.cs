using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Email;
using System.Windows.Forms;
using ShipWorks.Users;
using ShipWorks.Data.Connection;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;
using ShipWorks.Filters;
using log4net;
using System.Data.SqlClient;
using System.Transactions;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using ShipWorks.Data.Model;
using ShipWorks.SqlServer.Filters;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Utility;
using System.ComponentModel;
using Interapptive.Shared;
using ShipWorks.Users.Security;
using ShipWorks.Users.Audit;
using ShipWorks.Actions.Triggers;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Responsible for running the steps of a single ActionQueue at a time
    /// </summary>
    public class ActionRunner : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionRunner));

        // The queue we are gonig to run
        ActionQueueEntity queue;

        // The result of runnning.  We may know this up front depending on how LoadQueue does
        ActionRunnerResult result;

        // The app resource lock taken during the duration of processing
        SqlEntityLock entityLock;

        // The context that holds state accross multiple runners
        ActionProcessingContext context;

        /// <summary>
        /// Raised any time an action step is ran successfully
        /// </summary>
        public static event EventHandler ActionStepRan;

        /// <summary>
        /// Will throw a SqlAppResouceLockException of the queueID is already locked by another ActionRunner
        /// </summary>
        public ActionRunner(long queueID, ActionProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            try
            {
                this.entityLock = new SqlEntityLock(queueID, "Run ActionQueue");
                this.context = context;

                queue = LoadQueue(queueID);
            }
            catch (SqlAppResourceLockException)
            {
                result = ActionRunnerResult.Locked;
                log.InfoFormat("Could not obtain lock for running action queue item {0}", queueID);
            }
        }

        /// <summary>
        /// Will throw a SqlAppResouceLockException of the queueID is already locked by another ActionRunner
        /// </summary>
        public ActionRunner(ActionQueueEntity queue, ActionProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            if (!context.FlushingPostponed)
            {
                throw new InvalidOperationException("It is only valid to construct a runner with a queue instance that was postponed and is now being flushed.");
            }

            // We should only be loading a queue that was already previously loaded by this context
            if (queue.ContextLock != context.ContextLockName)
            {
                throw new InvalidOperationException("Should not be constructing from a queue object that the context didn't already own.");
            }

            try
            {
                this.entityLock = new SqlEntityLock(queue.ActionQueueID, "Run ActionQueue");
                this.context = context;

                this.queue = queue;
            }
            catch (SqlAppResourceLockException)
            {
                result = ActionRunnerResult.Locked;
                log.InfoFormat("Could not obtain lock for running action queue item {0}", queue.ActionQueueID);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (entityLock != null)
            {
                entityLock.Dispose();
                entityLock = null;
            }
        }

        /// <summary>
        /// The queue that the runner is going to run.  Will be null if the constructor could not load it for any reason.
        /// </summary>
        public ActionQueueEntity ActionQueue
        {
            get { return queue; }
        }

        /// <summary>
        /// Load the ActionQueue of the given ID.  If there is a problem for any reason, returns null.
        /// </summary>
        private ActionQueueEntity LoadQueue(long queueID)
        {
            ActionQueueEntity queue = new ActionQueueEntity(queueID);
            SqlAdapter.Default.FetchEntity(queue);

            // Make sure someone else did not already run it by checking that it still exists.  We could have taken the lock after
            // another ShipWorks took the lock, processed it, and then released the lock.
            if (queue.Fields.State != EntityState.Fetched)
            {
                log.InfoFormat("Action queue item {0} appears to already be complete.", queueID);

                result = ActionRunnerResult.Missing;
                return null;
            }
            else
            {
                // If its for a different computer we can't do it
                ComputerActionPolicy computerActionPolicy = new ComputerActionPolicy(queue.InternalComputerLimitedList);
                if (!computerActionPolicy.IsComputerAllowed(UserSession.Computer))
                {
                    result = ActionRunnerResult.WrongComputer;
                    return null;
                }

                // If it was just recently dispatched, then we have to fill in the queue steps
                if (queue.Status == (int) ActionQueueStatus.Dispatched)
                {
                    // Prepare to run by generating all the steps for the queue
                    if (!PrepareToRunQueuedAction(queue))
                    {
                        // Delete the queue item, since we failed to generate its steps
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.DeleteEntity(queue);
                        }

                        return null;
                    }
                }
                else
                {
                    // Load all the steps
                    ActionQueueStepCollection steps = ActionQueueStepCollection.Fetch(SqlAdapter.Default, ActionQueueStepFields.ActionQueueID == queue.ActionQueueID);

                    // Order them by there index
                    steps.Sort(ActionQueueStepFields.StepIndex.FieldIndex, ListSortDirection.Ascending);

                    queue.Steps.Clear();
                    queue.Steps.AddRange(steps);
                }
            }

            return queue;
        }

        /// <summary>
        /// Mark the given queue item as running and generate its action queue steps
        /// </summary>
        private bool PrepareToRunQueuedAction(ActionQueueEntity queue)
        {
            // Get the action so we can get the steps
            ActionEntity action = ActionManager.GetAction(queue.ActionID);

            if (action != null)
            {
                // Special case for filter content trigger - the store limitation is not detected at the time they are dispatched like the 
                // rest of the actions.
                if (action.TriggerType == (int) ActionTriggerType.FilterContentChanged)
                {
                    // See if this action is store limited, and there is an object to test against
                    if (action.StoreLimited && queue.ObjectID != null)
                    {
                        // Get the store(s) represented by this ObjectID.  Should be only 1, except for customers
                        List<long> storeKeys = DataProvider.GetRelatedKeys(queue.ObjectID.Value, EntityType.StoreEntity);

                        if (storeKeys.Intersect(action.StoreLimitedList).Count() == 0)
                        {
                            log.InfoFormat("Skipping action {0} due to store limitation.", action.Name);

                            result = ActionRunnerResult.Missing;
                            return false;
                        }
                    }
                }

                if (GenerateActionQueueSteps(queue, action))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                log.InfoFormat("Action queue item {0} was not ran because action {0} appears to have gone away.", queue.ActionQueueID, action.Name);

                result = ActionRunnerResult.Missing;
                return false;
            }
        }

        /// <summary>
        /// Generate the steps that will be executed for the given queue item
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool GenerateActionQueueSteps(ActionQueueEntity queue, ActionEntity action)
        {
            List<ActionTask> tasks = ActionManager.LoadTasks(action);

            // If there are not any tasks, there's no reason to execute the queue\action
            if (tasks.Count == 0)
            {
                log.InfoFormat("Ignoring action '{0}' since it has no tasks.", action.Name);

                result = ActionRunnerResult.NoTasks;
                return false;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // If dispatched from a filter trigger the name and version won't be set yet
                queue.ActionName = action.Name;
                queue.ActionVersion = action.RowVersion;

                queue.Status = (int) ActionQueueStatus.Incomplete;

                // Convert each task to a step to be executed
                foreach (ActionTask task in tasks)
                {
                    ActionQueueStepEntity step = new ActionQueueStepEntity();
                    queue.Steps.Add(step);

                    step.StepStatus = (int) ActionQueueStepStatus.Pending;
                    step.StepIndex = task.Entity.StepIndex;
                    step.StepName = ActionTaskManager.GetBinding(task).BaseName;

                    step.TaskIdentifier = task.Entity.TaskIdentifier;
                    step.TaskSettings = task.Entity.TaskSettings;

                    step.InputSource = task.Entity.InputSource;
                    step.InputFilterNodeID = task.Entity.InputFilterNodeID;

                    step.FilterCondition = task.Entity.FilterCondition;
                    step.FilterConditionNodeID = task.Entity.FilterConditionNodeID;

                    step.FlowSuccess = task.Entity.FlowSuccess;
                    step.FlowSkipped = task.Entity.FlowSkipped;
                    step.FlowError = task.Entity.FlowError;

                    step.AttemptDate = DateTime.UtcNow;
                    step.AttemptError = "";
                    step.AttemptCount = 0;
                }

                adapter.SaveAndRefetch(queue);
                adapter.Commit();
            }

            return true;
        }

        /// <summary>
        /// Run the steps for the configured queue
        /// </summary>
        [NDependIgnoreLongMethod]
        public ActionRunnerResult RunQueue()
        {
            // If we couldn't load the queue there was nothing to do
            if (queue == null)
            {
                return result;
            }

            // If trying to run a Postponed one, just get out.  We should only get here in error processing, b\c the standard gateway filters these out.
            if (queue.Status == (int) ActionQueueStatus.Postponed)
            {
                return ActionRunnerResult.Postponed;
            }

            // Not owned, we can take ownership
            if (queue.ContextLock == null)
            {
                // Note, we don't need to save this to the database right away.  It will either save after the first step completes, or if for some reason the first step
                // never completes and it never saves, its not a big deal - it will just get picked up by a different context.
                queue.ContextLock = context.ContextLockName;
            }
            else if (queue.ContextLock != context.ContextLockName)
            {
                // Locked by another context, we can't do anyting with it
                queue = null;
                return ActionRunnerResult.Locked;
            }

            log.InfoFormat("Running tasks for action '{0}', {1} from queued",
                queue.ActionName,
                ((TimeSpan) (DateTime.UtcNow - queue.TriggerDate)).TotalSeconds);

            // This would only be from Pre alpha5
            if (queue.NextStep == -1)
            {
                throw new InvalidOperationException("Action error leftover from pre alpha5.  It will need to be deleted.");
            }

            // If we are resuming from being postponed, we first need to advance to the next step, since we're still pointed to the previously postponed but now
            // finished step
            if (queue.Status == (int) ActionQueueStatus.ResumeFromPostponed)
            {
                AdvanceQueueToNextStep();

                // If it's now incomplete (and thus ready to keep going), then run it
                if (queue.Status == (int) ActionQueueStatus.Incomplete)
                {
                    RunStepsLoop();
                }
            }

            // Standard flow
            else if (queue.Status == (int) ActionQueueStatus.Incomplete)
            {
                RunStepsLoop();
            }

            // If it was in error, prepare it to be reran
            else if (queue.Status == (int) ActionQueueStatus.Error)
            {

                // Get the next error that will be processed
                ActionQueueStepEntity nextError = queue.Steps[queue.NextStep];
                bool wasSuspended = nextError.FlowError == (int) ActionTaskFlowOption.Suspend;

                // Determine if there are other errors that didn't stop the flow that still need to rerun
                var nonFlowStopErrors = queue.Steps.Where(s => s.StepStatus == (int) ActionQueueStepStatus.Error && s.FlowError != (int) ActionTaskFlowOption.Suspend).Select(s => s.StepIndex).ToList();

                // Run the steps
                queue.Status = (int) ActionQueueStatus.Incomplete;
                RunStepsLoop();

                // If we moved on from a suspended error, and the loop was reconfigured to flow through the unsuspending errors, run the loop again to try them.  It won't retry 
                // the one we did in the last loop, since the only way one of the nonFlowStopErrors will be active is if the previous one succeeded
                if (wasSuspended && nonFlowStopErrors.Contains(queue.NextStep))
                {
                    // Run the steps
                    queue.Status = (int) ActionQueueStatus.Incomplete;
                    RunStepsLoop();
                }
            }

            else
            {
                throw new InvalidOperationException("Queue is in an invalid state to be ran: " + queue.Status);
            }

            return ActionRunnerResult.Ran;
        }

        /// <summary>
        /// The primary loop for running steps out of the given queue.
        /// </summary>
        private void RunStepsLoop()
        {
            // Keep executing steps until we are done or postponed
            while (queue.Status == (int) ActionQueueStatus.Incomplete)
            {
                ActionStepContext stepContext = new ActionStepContext(queue, queue.Steps[queue.NextStep], context);

                try
                {
                    RunStep(stepContext);

                    // If successful raise the event
                    if (stepContext.Step.StepStatus == (int) ActionQueueStepStatus.Success)
                    {
                        // Raise the event for anyone interested in knowing an action just ran
                        EventHandler handler = ActionStepRan;
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }

                    // If any previously postponed stuff was consumed by this last step, and the queues of the previously postponed steps were suspended, we need to stop processing this queue.  
                    // This will let the ActionProcessor first go back and finish the queues that used to be postponed before continuing with us.  That way we maintain action processing order.
                    if (stepContext.PostponementActivity == ActionStepPostponementActivity.Consumed)
                    {
                        log.InfoFormat("Step consumed postponed queues, bailing on current queue.");
                        return;
                    }
                }
                catch (ActionRunnerFilterUpdateException ex)
                {
                    log.WarnFormat("Stopping steps loop due to not updating filters. ({0})", ex.Message);

                    return;
                }
            }
        }

        /// <summary>
        /// Run the step referred to be the given context.  The input keys will be determined using the task settings and the specified objectid
        /// </summary>
        [NDependIgnoreLongMethod]
        private void RunStep(ActionStepContext stepContext)
        {
            ActionQueueStepEntity step = stepContext.Step;
            log.InfoFormat("ActionStep - Start ({0}) {1} - {2}", step.StepIndex, queue.ActionName, step.StepName);

            step.AttemptDate = DateTime.UtcNow;
            step.AttemptCount++;
            step.AttemptError = "";

            // See if it will be skipped due to a filter condition
            bool filtersUpdated;
            bool skipStep = !CheckStepFilterCondition(step, queue.ObjectID, out filtersUpdated);

            // Create an instance of the task that created this step - we need information from it
            ActionTask actionTask = ActionManager.InstantiateTask(step.TaskIdentifier, step.TaskSettings);

            // If the task reads filter contents as a part of its execution, and we've not yet made sure filters are updated, we need to do it now
            if (!skipStep && actionTask.ReadsFilterContents && !filtersUpdated && !FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromMinutes(1), queue.QueueVersion))
            {
                throw new ActionRunnerFilterUpdateException("Filters were busy updating and the step was postponed.");
            }

            using (AuditBehaviorScope auditScope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, GetAuditStepReason(step)))
            {
                SqlAdapter adapter = null;

                // This try just has a finally, and basically just recreates the using pattern for the 'adapter'.
                try
                {
                    // Only run it if it's not skipped
                    if (skipStep)
                    {
                        step.StepStatus = (int) ActionQueueStepStatus.Skipped;
                    }
                    else
                    {
                        // This try is to catch exceptions thrown getting input keys and running the task
                        try
                        {
                            log.InfoFormat("ActionStep - Getting input");

                            List<long> inputKeys = GetStepInputKeys(step, actionTask, queue.ObjectID);

                            // Run the task.  If it requires input, and there is no input, don't even try to run it
                            if (step.InputSource != (int)ActionTaskInputSource.Nothing && inputKeys.Count == 0)
                            {
                                log.WarnFormat("ActionStep - Skipping due to no input");
                            }
                            else
                            {
                                // First we "Run" the task outside of a transaction.  The task should not do anytihng to save to the database here, but it can feel free to 
                                // do anything it needs with external resources.
                                log.InfoFormat("ActionStep - Start - Phase1 (Run)");
                                actionTask.Run(inputKeys, stepContext);
                                log.InfoFormat("ActionStep - Finished - Phase1 (Run)");

                                // Start Transaction - AFTER the 'Run' phase
                                adapter = new SqlAdapter(true);

                                // Here the task commits anything it needs saved.  If its a short task, then it could do its actual "Run" here too.
                                log.InfoFormat("ActionStep - Start - Phase2 (Commit)");
                                actionTask.Commit(inputKeys, stepContext);
                                log.InfoFormat("ActionStep - Finished  - Phase2 (Commit)");
                            }

                            // If we get either its either postponed or successful
                            step.StepStatus = (stepContext.PostponementActivity == ActionStepPostponementActivity.Postponed) ?
                                (int) ActionQueueStepStatus.Postponed :
                                (int) ActionQueueStepStatus.Success;

                        }
                        catch (ActionTaskRunException ex)
                        {
                            // Dispose the adapter if it was created, because that would mean the exception was
                            // thrown during the commit step.  A new adapter will be created below.
                            if (adapter != null)
                            {
                                adapter.Dispose();
                                adapter = null;
                            }

                            step.StepStatus = (int) ActionQueueStepStatus.Error;
                            step.AttemptError = ex.Message;
                        }
                    }

                    // If the transaction hasn't already been started yet (due to skip, or exception), start it now
                    if (adapter == null)
                    {
                        adapter = new SqlAdapter(true);
                    }

                    // If postponed steps were consumed, we now have to mark them with the actual result
                    if (stepContext.PostponementActivity == ActionStepPostponementActivity.Consumed)
                    {
                        var consumed = context.Postponements.Where(p => p.Identifier == stepContext.PostponementIdentifier).ToList();

                        // Mark each postponed step with the same result as the current step, since they were processed together as a unit
                        foreach (ActionPostponement postponement in consumed)
                        {
                            context.Postponements.Remove(postponement);

                            // Update the consumed step with the same results as this step
                            postponement.Step.StepStatus = step.StepStatus;
                            postponement.Step.AttemptError = step.AttemptError;

                            // Update the queue so next time it gets processed it knows it needs to resume from being
                            // postponed by advancing to the next step.  We do it this way instead of just advancing right now
                            // so that all advancement and queue status changes only take place in the main ActionProcessing loop
                            // when the queue is the primary ActionRunner focused queue.
                            postponement.Queue.Status = (int) ActionQueueStatus.ResumeFromPostponed;

                            adapter.SaveAndRefetch(postponement.Step, false);
                            adapter.SaveAndRefetch(postponement.Queue, false);
                        }
                    }

                    // Advance the queue on to the next step
                    AdvanceQueueToNextStep();

                    // Commit the transaction
                    adapter.Commit();
                }
                finally
                {
                    // Implementing using pattern ourselves due to weirdness with nesting and catch blocks
                    if (adapter != null)
                    {
                        adapter.Dispose();
                        adapter = null;
                    }
                }
            }
        }

        /// <summary>
        /// Advance the queue to the next step.  If completed, the queue is deleted
        /// </summary>
        private void AdvanceQueueToNextStep()
        {
            // Get the step we are on right now
            ActionQueueStepEntity step = queue.Steps[queue.NextStep];

            // Determine what the next step will be
            queue.NextStep = DetermineNextStep(step);

            // If the flow is complete or suspended, we need to update the queue state
            if (queue.NextStep == -1 || queue.NextStep == step.StepIndex)
            {
                bool anyPostponed = queue.Steps.Any(s => s.StepStatus == (int) ActionQueueStepStatus.Postponed);
                bool anyErrors = queue.Steps.Any(s => s.StepStatus == (int) ActionQueueStepStatus.Error);

                if (anyPostponed)
                {
                    queue.Status = (int) ActionQueueStatus.Postponed;
                }
                else if (anyErrors)
                {
                    queue.Status = (int) ActionQueueStatus.Error;
                }
                else
                {
                    Debug.Assert(queue.NextStep == -1);
                    queue.Status = (int) ActionQueueStatus.Success;
                }
            }
            else
            {
                queue.Status = (int) ActionQueueStatus.Incomplete;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // If sucess, we don't need the queue anymore at all.
                if (queue.Status == (int) ActionQueueStatus.Success)
                {
                    // Done this way so the entity can still be used.
                    adapter.DeleteEntity(new ActionQueueEntity(queue.ActionQueueID));
                }
                else
                {
                    // If we are in error, and the flow is complete, then readjust the flow so that next time through it will go from error to error
                    if (queue.Status == (int) ActionQueueStatus.Error && queue.NextStep == -1)
                    {
                        var errorSteps = queue.Steps.Where(s => s.StepStatus == (int) ActionQueueStepStatus.Error).ToList();

                        // Start the queue off on the first error
                        queue.NextStep = errorSteps[0].StepIndex;

                        // Go through each step in error
                        for (int i = 0; i < errorSteps.Count; i++)
                        {
                            var errorStep = errorSteps[i];

                            // If this is the last one then on success we quit
                            if (i == errorSteps.Count - 1)
                            {
                                errorStep.FlowSuccess = (int) ActionTaskFlowOption.Quit;
                            }
                            // Otherwise it will advance to the next error step
                            else
                            {
                                errorStep.FlowSuccess = -(errorSteps[i + 1].StepIndex + 1);
                            }

                            // Error just does the same as success
                            errorStep.FlowError = (int) errorStep.FlowSuccess;

                            adapter.SaveAndRefetch(errorStep, false);
                        }
                    }

                    adapter.SaveAndRefetch(queue, false);
                    adapter.SaveAndRefetch(step, false);
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Determine the next step to run based on the status of the specified step.  If there are no more steps to run
        /// -1 is returned. 
        /// </summary>
        private static int DetermineNextStep(ActionQueueStepEntity step)
        {
            ActionQueueEntity queue = step.ActionQueue;

            int flowOption;

            switch ((ActionQueueStepStatus) step.StepStatus)
            {
                case ActionQueueStepStatus.Success:
                    flowOption = step.FlowSuccess;
                    break;

                case ActionQueueStepStatus.Skipped:
                    flowOption = step.FlowSkipped;
                    break;

                case ActionQueueStepStatus.Error:
                    flowOption = step.FlowError;
                    break;

                case ActionQueueStepStatus.Postponed:
                    flowOption = (int) ActionTaskFlowOption.Suspend;
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Invalid step status {0}", step.StepStatus));
            }

            // When suspending we don't change steps
            if (flowOption == (int) ActionTaskFlowOption.Suspend)
            {
                return step.StepIndex;
            }

            if (flowOption == (int) ActionTaskFlowOption.Quit)
            {
                return -1;
            }

            if (flowOption == (int) ActionTaskFlowOption.NextStep)
            {
                int currentIndex = queue.Steps.IndexOf(step);

                if (currentIndex == queue.Steps.Count - 1)
                {
                    return -1;
                }
                else
                {
                    return currentIndex + 1;
                }
            }

            // If the user configured a specific step to jump to, then the flow option is the negative of the next step to execute.
            return -(flowOption + 1);
        }

        /// <summary>
        /// See if this step has a filter condition, and if so, if it passes.  filtersUpdated indicates if filter contents were updated during execution
        /// of the function.
        /// </summary>
        private static bool CheckStepFilterCondition(ActionQueueStepEntity step, long? objectID, out bool filtersUpdated)
        {
            filtersUpdated = false;

            // If there is no filter condition, or no object to check, it's all good
            if (!step.FilterCondition || objectID == null)
            {
                return true;
            }

            // Special case for top-level filters, who don't have rows in the FilterNodeContentDetail table
            if (!BuiltinFilter.IsTopLevelKey(step.FilterConditionNodeID))
            {
                long? contentID = FilterHelper.GetFilterNodeContentID(step.FilterConditionNodeID);

                // If the filter has gone away, we can't do anything
                if (contentID == null)
                {
                    log.InfoFormat("Step {0} skipped due to filtering and filter appears to be deleted.", step.StepName);
                    return false;
                }

                if (!FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromMinutes(1), step.ActionQueue.QueueVersion))
                {
                    throw new ActionRunnerFilterUpdateException("Filters were busy updating so the task was postponed.");
                }

                // Filter contents have been updated
                filtersUpdated = true;

                // Now, we can finally check if it exists in the filter
                if (!FilterHelper.IsObjectInFilterContent(objectID.Value, contentID.Value))
                {
                    log.InfoFormat("Step {0} skipped due to filtering and object not in filter.", step.StepName);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determine the input keys that should be used for the given task and instance of that task.  Returns null if and only if there was
        /// an error determing the input and the instance has been marked and saved as in error.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static List<long> GetStepInputKeys(ActionQueueStepEntity step, ActionTask actionTask, long? objectID)
        {
            ActionTaskInputSource inputSource = (ActionTaskInputSource) step.InputSource;

            List<long> input = new List<long>();

            // We need to create the base object so we can know if it needs input
            if (inputSource == ActionTaskInputSource.Nothing)
            {
                return input;
            }

            // Use the ObjectID as the input
            if (inputSource == ActionTaskInputSource.TriggeringRecord)
            {
                if (objectID == null)
                {
                    throw new InvalidOperationException("Cannot use triggering record as data source when there is no triggering record.");
                }

                if (DataProvider.GetEntity(objectID.Value) == null)
                {
                    throw new ActionTaskRunException("The record that triggered the task has been deleted, so the task cannot run.");
                }

                input.Add(objectID.Value);
            }
            // Input source is whatever we have saved for the selection of this task
            else if (inputSource == ActionTaskInputSource.Selection)
            {
                ResultsetFields resultFields = new ResultsetFields(1);
                resultFields.DefineField(ActionQueueSelectionFields.ObjectID, 0, "ObjectID", "");

                // The dispatcher adds the selection in user-sort order, so they get assigned lowest ActionQueueSelectionID's first
                SortExpression sort = new SortExpression(ActionQueueSelectionFields.ActionQueueSelectionID | SortOperator.Ascending);

                RelationPredicateBucket bucket = new RelationPredicateBucket(ActionQueueSelectionFields.ActionQueueID == step.ActionQueue.ActionQueueID);
                using (SqlDataReader reader = (SqlDataReader) SqlAdapter.Default.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, sort, true))
                {
                    while (reader.Read())
                    {
                        input.Add(reader.GetInt64(0));
                    }
                }
            }
            else
            {
                long inputFilterNodeID = step.InputFilterNodeID;

                ResultsetFields resultFields = new ResultsetFields(1);
                RelationPredicateBucket bucketToUse = null;

                // If it's not a top level key then we have to load the results from the filter content results
                if (!BuiltinFilter.IsTopLevelKey(inputFilterNodeID))
                {
                    long? contentID = FilterHelper.GetFilterNodeContentID(inputFilterNodeID);
                    if (contentID == null)
                    {
                        throw new ActionTaskRunException("The filter for the input to the task has been deleted.");
                    }

                    // Load all ID's in the filter.
                    resultFields.DefineField(FilterNodeContentDetailFields.ObjectID, 0, "ObjectID", "");
                    bucketToUse = new RelationPredicateBucket(FilterNodeContentDetailFields.FilterNodeContentID == contentID);
                }
                else
                {
                    // Load directly from the orders table
                    if (BuiltinFilter.GetTopLevelKey(FilterTarget.Orders) == inputFilterNodeID)
                    {
                        resultFields.DefineField(OrderFields.OrderID, 0, "ObjectID", "");
                        bucketToUse = new RelationPredicateBucket();

                    }
                    // Load directly from the customers table
                    else if (BuiltinFilter.GetTopLevelKey(FilterTarget.Customers) == inputFilterNodeID)
                    {
                        resultFields.DefineField(CustomerFields.CustomerID, 0, "ObjectID", "");
                        bucketToUse = new RelationPredicateBucket();
                    }
                }

                // If we have a bucket to use to do the query, query now
                if (bucketToUse != null)
                {
                    using (SqlDataReader reader = (SqlDataReader) SqlAdapter.Default.FetchDataReader(resultFields, bucketToUse, CommandBehavior.CloseConnection, 0, true))
                    {
                        while (reader.Read())
                        {
                            input.Add(reader.GetInt64(0));
                        }
                    }
                }
            }

            // If the task required input of a certain type, convert it now
            if (actionTask.InputEntityType != null)
            {
                input = DataProvider.GetRelatedKeys(input, actionTask.InputEntityType.Value);
            }

            // Get the binding for this task, so we know what the storetype and storeid limitations are
            ActionTaskDescriptorBinding binding = ActionTaskManager.GetBinding(actionTask);

            // If the task is store-specific, we have to limit the input to only those stores
            if (binding.StoreTypeCode != null || binding.StoreID != null)
            {
                List<long> storeKeys = new List<long>();

                if (binding.StoreID != null)
                {
                    storeKeys.Add(binding.StoreID.Value);
                }
                else
                {
                    storeKeys.AddRange(StoreManager.GetAllStores().Where(s => s.TypeCode == (int) binding.StoreTypeCode.Value).Select(s => s.StoreID));
                }

                List<long> storeInput = new List<long>();

                // We have to go through each one and determine what the store is
                foreach (long key in input)
                {
                    if (DataProvider.GetRelatedKeys(key, EntityType.StoreEntity).Intersect(storeKeys).Count() > 0)
                    {
                        storeInput.Add(key);
                    }
                }

                input = storeInput;
            }

            return input;
        }

        /// <summary>
        /// Get the text to use as the reason for the given step
        /// </summary>
        private AuditReason GetAuditStepReason(ActionQueueStepEntity step)
        {
            return new AuditReason(
                AuditReasonType.Action,
                string.Format("Action '{0}' - Step {1}: {2}", queue.ActionName, step.StepIndex + 1, step.StepName));
        }
    }
}
