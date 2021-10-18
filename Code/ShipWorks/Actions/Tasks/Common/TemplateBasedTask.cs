using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Xml.XPath;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task that uses a template to do its job
    /// </summary>
    public abstract class TemplateBasedTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateBasedTask));

        long templateID = 0;
        long originalTemplateID = 0;

        const long maxReportPostpone = 1000;

        /// <summary>
        /// Create the editor
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new TemplateBasedTaskEditor(this);
        }

        /// <summary>
        /// The maximum number of inputs to keep postponing for a report
        /// </summary>
        public static long MaxReportPostpone
        {
            get { return maxReportPostpone; }
        }

        /// <summary>
        /// Most be overridden by derived classes to do the actual work of outputing template content
        /// </summary>
        protected abstract void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context);

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            TemplateEntity template = context.GetTemplate(TemplateID);

            try
            {
                // Postponement disabled or Stanard\thermal templates just get executed right away, nothing to wait for
                if (!EnablePostpone || template.Type == (int) TemplateType.Standard || template.Type == (int) TemplateType.Thermal)
                {
                    ProcessTemplateResults(
                        template,
                        TemplateProcessor.ProcessTemplate(template, inputKeys), 
                        context);
                }
                else
                {
                    // If its a report, we postpone so it all gets grouped together
                    if (template.Type == (int) TemplateType.Report)
                    {
                        // Get any postponed data we've previously stored away, pluse what we're working with now
                        List<long> combinedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).Concat(inputKeys).ToList();

                        // To avoid postponing forever on big selections, we only postpone up to maxReportPostpone
                        if (context.CanPostpone && combinedKeys.Count < maxReportPostpone)
                        {
                            context.Postpone(inputKeys);
                        }
                        else
                        {
                            context.ConsumingPostponed();

                            // Process
                            ProcessTemplateResults(
                                template,
                                TemplateProcessor.ProcessTemplate(template, combinedKeys),
                                context);
                        }
                    }
                    // If it's a label, we have to wait until a sheet can be filled
                    else if (template.Type == (int) TemplateType.Label)
                    {
                        // Process the results for the current set of inputs
                        IList<TemplateResult> currentResults = TemplateProcessor.ProcessTemplate(template, inputKeys);

                        // Get any postponed results we've previously stored away, and combine it with the current results
                        List<TemplateResult> combinedResults = context.GetPostponedData().SelectMany(d => (IList<TemplateResult>) d).Concat(currentResults).ToList();

                        // It's possible to have no inputs depending on what was selected and the template context
                        if (combinedResults.Count > 0)
                        {
                            LabelSheetEntity sheet = context.GetLabelSheet(template.LabelSheetID);
                            int cells = sheet.Rows * sheet.Columns;

                            // If it doesn't fit perfectly and we're allowed to postpone, then postpone
                            if (context.CanPostpone && combinedResults.Count % cells > 0)
                            {
                                context.Postpone(currentResults);
                            }
                            // Otherwise go now
                            else
                            {
                                context.ConsumingPostponed();

                                // Process
                                ProcessTemplateResults(template, combinedResults, context);
                            }
                        }
                        else
                        {
                            log.WarnFormat("Not printing labels for task {0}.{1} due to no input", context.Queue.ActionID, context.Step.StepIndex);
                        }
                    }
                }
            }
            catch (TemplateException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// The object is being deserialized into its values
        /// </summary>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            originalTemplateID = templateID;
        }

        /// <summary>
        /// The template that will be used for printing
        /// </summary>
        public long TemplateID
        {
            get { return templateID; }
            set { templateID = value; }
        }


        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public virtual bool EnablePostpone
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Save the reference to the selected template
        /// </summary>
        protected override void SaveExtraState(ActionEntity action, ISqlAdapter adapter)
        {
            if (originalTemplateID != templateID)
            {
                DeleteExtraState();
            }

            if (templateID != 0)
            {
                log.DebugFormat("Set template usage {0}", templateID);

                ObjectReferenceManager.SetReference(Entity.ActionTaskID, "ActionTaskTemplate", templateID, GetObjectReferenceReason(action));
            }
        }

        /// <summary>
        /// Delete the reference to the selected template
        /// </summary>
        protected override void DeleteExtraState()
        {
            if (originalTemplateID != 0)
            {
                log.DebugFormat("Release template usage {0}", originalTemplateID);
                ObjectReferenceManager.ClearReference(Entity.ActionTaskID, "ActionTaskTemplate");
            }
        }
    }
}
