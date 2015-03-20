extern alias rebex2015;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using rebex2015::Rebex;
using rebex2015::Rebex.Net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Saving;
using log4net;

namespace ShipWorks.Actions.Tasks.Common
{
    extern alias rebex2015;

    /// <summary>
    /// Task for ftping a chosen template
    /// </summary>
    [ActionTask("Upload with FTP", "Ftp", ActionTaskCategory.Output)]
    public class FtpFileTask : TemplateBasedTask
    {
        static int logIndex;

        /// <summary>
        /// The base temporary file name
        /// </summary>
        private readonly string rootTempFileName;

        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(FtpFileTask));

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileTask"/> class.
        /// </summary>
        public FtpFileTask()
        {
            rootTempFileName = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString());
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
                return "Upload using:";
            }
        }

        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public override bool EnablePostpone
        {
            get { return false; }
        }

        /// <summary>
        /// Create the editor
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new FtpFileTaskEditor(this);
        }

        /// <summary>
        /// Most be overridden by derived classes to do the actual work of outputing template content
        /// </summary>
        /// <exception cref="ActionTaskRunException"></exception>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            SaveTemplateResultsToTempFile(template, templateResults);

            UploadFiles();

            DeleteTempFiles();
        }

        /// <summary>
        /// Deletes the temporary files.
        /// </summary>
        private void DeleteTempFiles()
        {
            Directory.Delete(rootTempFileName, true);
        }

        /// <summary>
        /// Uploads the files.
        /// </summary>
        /// <exception cref="ActionTaskRunException"></exception>
        private void UploadFiles()
        {
            if (!FtpAccountID.HasValue)
            {
                throw new ActionTaskRunException("Ftp account not configured for this action.");
            }

            try
            {
                FtpAccountEntity ftpAccountEntity = FtpAccountManager.GetAccount(FtpAccountID.Value);
                IFtp ftp = FtpUtility.LogonToFtp(ftpAccountEntity);

                ftp.LogWriter = GetFtpLogWriter();
                ftp.PutFiles(string.Format("{0}\\*", rootTempFileName), "/", FtpBatchTransferOptions.Recursive, FtpActionOnExistingFiles.OverwriteAll);
            }
            catch (NetworkSessionException ex)
            {
                log.Error("Error transferring File", ex);
                throw new ActionTaskRunException(String.Format("Error uploading file. Ftp returned error: {0}", ex.Message));
            }
            catch (FileTransferException ex)
            {
                log.Error("Error transferring File", ex);
                throw new ActionTaskRunException(String.Format("Error uploading file. Ftp returned error: {0}", ex));
            }
        }

        /// <summary>
        /// Gets the FTP log writer.
        /// </summary>
        /// <returns></returns>
        private static FileLogWriter GetFtpLogWriter()
        {
            string logDirectoryPath = (string.Format("{0}\\TransferFilesTask", LogSession.LogFolder));
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
            FileLogWriter ftpLogWriter = new FileLogWriter(string.Format("{0}\\{1} - Log.txt", logDirectoryPath, logIndex.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')), LogLevel.Debug);
            logIndex++;
            return ftpLogWriter;
        }

        /// <summary>
        /// Saves the template results automatic temporary file.
        /// </summary>
        /// <exception cref="ActionTaskRunException"></exception>
        private void SaveTemplateResultsToTempFile(TemplateEntity template, IList<TemplateResult> templateResults)
        {
            try
            {
                TemplateTree editableTemplateTreeClone = TemplateManager.Tree.CreateEditableClone();
                TemplateEntity editableCopyOfTemplate = editableTemplateTreeClone.GetTemplate(template.TemplateID);

                SaveWriter writer = SaveWriter.Create(editableCopyOfTemplate, templateResults);

                editableCopyOfTemplate.SaveFileName = FtpFileName;
                editableCopyOfTemplate.SaveFileFolder = string.Format("{0}/{1}", rootTempFileName, FtpFolder);
                editableCopyOfTemplate.SaveFilePrompt = (int)SavePromptWhen.Never;

                writer.Save();
            }
            catch (SavingNoTemplateOutputException)
            {
                log.Info("SaveFileTask skipped due to no template output.");
            }
            catch (SaveException ex)
            {
                log.Error("Error saving file before FTP.", ex);

                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
