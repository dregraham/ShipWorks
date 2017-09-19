using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Provides context for an ActionTask during the duration of running a step
    /// </summary>
    public interface IActionStepContext
    {
        /// <summary>
        /// Indicates if its currently allowed to postpone, or if a step must execute regardless
        /// </summary>
        bool CanPostpone { get; }

        /// <summary>
        /// Provides a way for steps to store a list of entities that need committed from the Run phase, to be committed in the Commit phase.  Anything in this list
        /// will be committed by the Commit phase by default, but that can be overridden.
        /// </summary>
        UnitOfWork2 CommitWork { get; }

        /// <summary>
        /// Get the postponement state of the action
        /// </summary>
        ActionStepPostponementActivity PostponementActivity { get; }

        /// <summary>
        /// The postponement identifier used by the step to uniquely identify its postponement instances, if any
        /// </summary>
        string PostponementIdentifier { get; }

        /// <summary>
        /// The ActionQueue for the step being executed
        /// </summary>
        ActionQueueEntity Queue { get; }

        /// <summary>
        /// The step entity being executed
        /// </summary>
        ActionQueueStepEntity Step { get; }

        /// <summary>
        /// Add to the list of messages generated during the action run.  Any task that produces email
        /// messages should use this method.
        /// </summary>
        void AddGeneratedEmail(EmailOutboundEntity message);

        /// <summary>
        /// Add to the list of messages generated during the action run.  Any task that produces email
        /// messages should use this method.
        /// </summary>
        void AddGeneratedEmail(IEnumerable<EmailOutboundEntity> messages);

        /// <summary>
        /// Indicate to the step that we have decided to consume all data for previously postponed steps
        /// </summary>
        void ConsumingPostponed();

        /// <summary>
        /// Using this method ensures that all tasks occurring within the same action get the exact same copy of a label sheet
        /// </summary>
        LabelSheetEntity GetLabelSheet(long labelSheetID);

        /// <summary>
        /// Get all postponed data for the given step
        /// </summary>
        IEnumerable<object> GetPostponedData();

        /// <summary>
        /// Using this method ensures that all tasks occurring within the same action get the exact same copy of a template
        /// </summary>
        TemplateEntity GetTemplate(long templateID);

        /// <summary>
        /// Postpone the current step and store the given data with it.  Postponing and consuming automatically restricts the visible scope
        /// to only the same step within the same action.
        /// </summary>
        void Postpone(object data);
    }
}