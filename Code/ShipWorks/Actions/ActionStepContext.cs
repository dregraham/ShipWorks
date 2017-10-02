using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Provides context for an ActionTask during the duration of running a step
    /// </summary>
    public class ActionStepContext : IActionStepContext
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionStepContext));

        ActionQueueEntity queue;
        ActionQueueStepEntity step;
        ActionProcessingContext context;

        string identifier;
        ActionStepPostponementActivity postponementActivity = ActionStepPostponementActivity.None;

        UnitOfWork2 commitWork = new UnitOfWork2();

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionStepContext(ActionQueueEntity queue, ActionQueueStepEntity step, ActionProcessingContext context)
        {
            this.queue = queue;
            this.step = step;
            this.context = context;
            this.identifier = string.Format("{0}_{1}", SqlUtility.GetTimestampValue(queue.ActionVersion), step.StepIndex);
        }

        /// <summary>
        /// The ActionQueue for the step being executed
        /// </summary>
        public ActionQueueEntity Queue
        {
            get { return queue; }
        }

        /// <summary>
        /// The step entity being executed
        /// </summary>
        public ActionQueueStepEntity Step
        {
            get { return step; }
        }

        /// <summary>
        /// Provides a way for steps to store a list of entities that need committed from the Run phase, to be committed in the Commit phase.  Anything in this list
        /// will be committed by the Commit phase by default, but that can be overridden.
        /// </summary>
        public UnitOfWork2 CommitWork
        {
            get { return commitWork; }
        }

        /// <summary>
        /// Indicates if its currently allowed to postpone, or if a step must execute regardless
        /// </summary>
        public bool CanPostpone
        {
            get { return !context.FlushingPostponed; }
        }

        /// <summary>
        /// Get the postponement state of the action
        /// </summary>
        public ActionStepPostponementActivity PostponementActivity
        {
            get { return postponementActivity; }
        }

        /// <summary>
        /// The postponement identifier used by the step to uniquely identify its postponement instances, if any
        /// </summary>
        public string PostponementIdentifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// Get all postponed data for the given step
        /// </summary>
        public IEnumerable<object> GetPostponedData()
        {
            return context.Postponements.Where(p => p.Identifier == identifier).Select(p => p.Data);
        }

        /// <summary>
        /// Postpone the current step and store the given data with it.  Postponing and consuming automatically restricts the visible scope
        /// to only the same step within the same action.
        /// </summary>
        public void Postpone(object data)
        {
            if (!CanPostpone)
            {
                throw new InvalidOperationException("It is currently illegal to postpone.  That is an application logic error, not a business error we can catch.");
            }

            if (postponementActivity != ActionStepPostponementActivity.None)
            {
                throw new InvalidOperationException("A step cannot consume postponed steps and postpone itself at the same time, or postpone itself more than once.");
            }

            postponementActivity = ActionStepPostponementActivity.Postponed;
            context.Postponements.Add(new ActionPostponement(identifier, step, data));

            log.InfoFormat("Postponing action step {0}", identifier);
        }

        /// <summary>
        /// Indicate to the step that we have decided to consume all data for previously postponed steps
        /// </summary>
        public void ConsumingPostponed()
        {
            if (postponementActivity != ActionStepPostponementActivity.None)
            {
                throw new InvalidOperationException("A step cannot consume postponed steps and postpone itself at the same time, or postpone itself more than once.");
            }

            // If there's nothing to consume, just ignore this
            if (!GetPostponedData().Any())
            {
                return;
            }

            postponementActivity = ActionStepPostponementActivity.Consumed;

            log.InfoFormat("Consuming postponed steps for {0}", identifier);
        }

        /// <summary>
        /// Using this method ensures that all tasks occurring within the same action get the exact same copy of a template
        /// </summary>
        public TemplateEntity GetTemplate(long templateID)
        {
            TemplateEntity template;
            if (!context.TemplateCache.TryGetValue(templateID, out template))
            {
                // We need the template
                template = TemplateManager.Tree.GetTemplate(templateID);
                if (template == null)
                {
                    ObjectLabel label = ObjectLabelManager.GetLabel(templateID, true);

                    if (label != null)
                    {
                        throw new ActionTaskRunException(string.Format("The template '{0}' used by the task has been deleted.", label.ShortText));
                    }
                    else
                    {
                        throw new ActionTaskRunException("No template has been configured for the task.");
                    }
                }

                context.TemplateCache[templateID] = template;

                // Go ahead and cache the label sheet too
                if (template.Type == (int) TemplateType.Label)
                {
                    GetLabelSheet(template.LabelSheetID);
                }
            }

            return template;
        }

        /// <summary>
        /// Using this method ensures that all tasks occurring within the same action get the exact same copy of a label sheet
        /// </summary>
        public LabelSheetEntity GetLabelSheet(long labelSheetID)
        {
            LabelSheetEntity sheet;
            if (!context.LabelSheetCache.TryGetValue(labelSheetID, out sheet))
            {
                // We need the sheet
                sheet = LabelSheetManager.GetLabelSheet(labelSheetID);
                if (sheet == null)
                {
                    throw new ActionTaskRunException("The label sheet used by the task template been deleted.");
                }

                sheet = new LabelSheetEntity(sheet.Fields.Clone());
                sheet.IsNew = false;

                context.LabelSheetCache[labelSheetID] = sheet;
            }

            return sheet;
        }

        /// <summary>
        /// Add to the list of messages generated during the action run.  Any task that produces email
        /// messages should use this method.
        /// </summary>
        public void AddGeneratedEmail(EmailOutboundEntity message)
        {
            AddGeneratedEmail(new EmailOutboundEntity[] { message });
        }

        /// <summary>
        /// Add to the list of messages generated during the action run.  Any task that produces email
        /// messages should use this method.
        /// </summary>
        public void AddGeneratedEmail(IEnumerable<EmailOutboundEntity> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            context.EmailGenerated.AddRange(messages);
        }
    }
}
