using System;
using System.Collections.Generic;
using System.IO;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Saving;
using log4net;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for ftping a chosen template
    /// </summary>
    [ActionTask("Transfer file(s)", "Ftp")]
    public class FtpFileTask : TemplateBasedTask
    {
        private string baseTempFileName;
        //private int tempFileIndex = 0;

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FtpFileTask));

        public FtpFileTask()
        {
            baseTempFileName = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Gets or sets the FTP account unique identifier.
        /// </summary>
        public long? FtpAccountID { get; set; }

        /// <summary>
        /// Gets or sets the FTP folder.
        /// </summary>
        public string FtpFolder { get; set; }

        /// <summary>
        /// Gets or sets the name of the FTP file.
        /// </summary>
        public string FtpFileName { get; set; }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Ftp file(s) using:";
            }
        }
		
        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public override bool EnablePostpone
        {
            get { return false; }
        }

        public override ActionTaskEditor CreateEditor()
        {
            return new FtpFileTaskEditor(this);
        }

        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            TemplateTree editableTemplateTreeClone = TemplateManager.Tree.CreateEditableClone();
            TemplateEntity editableCopyOfTemplate = editableTemplateTreeClone.GetTemplate(template.TemplateID);

            // Create the print job using the default settings from the template
            SaveWriter writer = SaveWriter.Create(editableCopyOfTemplate, templateResults);

            try
            {
                editableCopyOfTemplate.SaveFileName = FtpFileName;
                editableCopyOfTemplate.SaveFileFolder = baseTempFileName + "/" + FtpFolder;
                editableCopyOfTemplate.SaveFilePrompt = (int)SavePromptWhen.Never;

                writer.PromptForFile += OnSavePromptForFile;

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


        private void OnSavePromptForFile(object sender, SavePromptForFileEventArgs e)
        {
            //string fileName = Path.Combine(baseTempFileName, FtpFolder);

            //e.ResultName = fileName;

            //tempFileIndex++;
        }
    }
}
