using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Saving;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for saving with a chosen template
    /// </summary>
    [ActionTask("Save a file", "Save File", ActionTaskCategory.Output)]
    public class SaveFileTask : TemplateBasedTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SaveFileTask));

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Save using:";
            }
        }

        /// <summary>
        /// Do the print for the given template and input
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            try
            {
                // Create the print job using the default settings from the template
                SaveWriter writer = SaveWriter.Create(template, templateResults);

                writer.PromptForFile += (object sender, SavePromptForFileEventArgs e) =>
                    {
                        throw new SaveException(string.Format("Template '{0}' is configured to prompt for filenames when saving, which isn't allowed for action tasks.", template.FullName));
                    };

                writer.Save();
            }
            catch (SavingNoTemplateOutputException)
            {
                log.Info("SaveFileTask skipped due to no template output.");
            }
            catch (SaveException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
