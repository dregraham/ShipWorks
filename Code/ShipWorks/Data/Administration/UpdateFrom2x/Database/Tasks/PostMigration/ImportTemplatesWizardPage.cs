using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using System.IO;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Data;
using System.Data.SqlClient;
using log4net;
using ICSharpCode.SharpZipLib.Zip;
using Interapptive.Shared.Utility;
using System.Transactions;
using Interapptive.Shared;
using ShipWorks.Data.Connection;
using ShipWorks.Templates;
using ShipWorks.Templates.Distribution;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.IO.Zip;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Post migration step for importing v2 templates into v3
    /// </summary>
    public partial class ImportTemplatesWizardPage : WizardPage
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ImportTemplatesWizardPage));

        static string importRootFolder = "ShipWorks2";

        // So we know when we can move on
        bool nextMovesOn = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportTemplatesWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The hardcoded name of the folder that all templates imported from v2 during upgrade are imported into
        /// </summary>
        public static string ImportRootFolderName
        {
            get { return importRootFolder; }
        }

        /// <summary>
        /// Stepping into
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                ChooseInitialImportTemplateMethod();
            }

            // When we click next, we do the import, not move on
            nextMovesOn = false;
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (nextMovesOn)
            {
                return;
            }

            // Check the backup file
            if (radioImportTemplatesBackupFile.Checked)
            {
                if (string.IsNullOrWhiteSpace(importTemplatesBackupFile.Text) || !File.Exists(importTemplatesBackupFile.Text))
                {
                    MessageHelper.ShowInformation(this, "Please select a ShipWorks backup file.");
                    e.NextPage = this;
                    return;
                }
            }

            // Check the folder
            else if (radioImportTemplatesAppData.Checked)
            {
                if (string.IsNullOrWhiteSpace(importTemplatesAppDataFolder.Text) || !Directory.Exists(importTemplatesAppDataFolder.Text))
                {
                    MessageHelper.ShowInformation(this, "Please select a ShipWorks Application Data folder.");
                    e.NextPage = this;
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            // Stay on this page
            e.NextPage = this;

            // Create the progress provider and window
            ProgressProvider progressProvider = new ProgressProvider();
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Importing Templates";
            progressDlg.Description = "ShipWorks is importing old templates.";
            progressDlg.Show(this);

            // Used for async invoke
            MethodInvoker<ProgressProvider, string> invoker = new MethodInvoker<ProgressProvider, string>(AsyncImportTemplates);

            // Pass along user state
            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["invoker"] = invoker;
            userState["progressDlg"] = progressDlg;

            // Kick off the async upgrade process
            invoker.BeginInvoke(
                progressDlg.ProgressProvider, 
                radioDontImport.Checked ? null : (radioImportTemplatesBackupFile.Checked ? importTemplatesBackupFile.Text : importTemplatesAppDataFolder.Text),
                new AsyncCallback(OnAsyncComplete), userState);
        }

        /// <summary>
        /// Method meant to be called from an async invoker to update the database in the background
        /// </summary>
        [NDependIgnoreLongMethod]
        private void AsyncImportTemplates(ProgressProvider progressProvider, string path)
        {
            var progressImport = progressProvider.ProgressItems.Add("Import Templates");
            progressImport.CanCancel = false;

            bool isBackupFile = path != null && path.EndsWith("swb");
            if (isBackupFile)
            {
                var progressExtract = new ProgressItem("Decompress Backup");
                progressProvider.ProgressItems.Insert(0, progressExtract);

                path = ExtractTemplatesFromBackup(path, progressExtract);
            }

            var progressInstall = progressProvider.ProgressItems.Add("Install Templates");
            progressInstall.CanCancel = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                if (!progressProvider.CancelRequested)
                {
                    progressImport.Starting();
                    progressImport.Detail = "Initializing...";

                    // Create the editable tree we can install into
                    TemplateManager.CheckForChangesNeeded();
                    TemplateTree tree = TemplateManager.Tree.CreateEditableClone();

                    // If we've already installed them, delete what was there
                    TemplateFolderEntity folder = tree.AllFolders.FirstOrDefault(f => f.FullName == importRootFolder);
                    if (folder != null)
                    {
                        progressImport.Detail = "Deleting previous templates...";
                        TemplateEditingService.DeleteFolder(folder);
                    }

                    // null path means they chose not to import
                    if (path != null)
                    {
                        ImportTemplates(tree, path, progressImport);
                    }

                    progressImport.Completed();
                    progressImport.Detail = "Done";

                    // Install the default set of templates
                    progressInstall.Starting();
                    progressInstall.Detail = "Installing ShipWorks 3 templates...";

                    // Install\update the builtin templates
                    BuiltinTemplates.UpdateTemplates(this);
                }

                else
                {
                    throw new OperationCanceledException();
                }

                scope.Complete();
            }

            // Get the core template tree back up to date
            TemplateManager.CheckForChangesNeeded();

            progressInstall.PercentComplete = 100;
            progressInstall.Completed();
            progressInstall.Detail = "Done";
        }

        /// <summary>
        /// Extract templates from the given backup file and return there location
        /// </summary>
        private static string ExtractTemplatesFromBackup(string path, ProgressItem progress)
        {
            string tempPath = DataPath.CreateUniqueTempPath();

            progress.Starting();

            using (ZipReader reader = new ZipReader(path))
            {
                // Progress
                reader.Progress += delegate(object sender, ZipReaderProgressEventArgs args)
                {
                    progress.PercentComplete = (int) (((float) args.TotalBytesProcessed / (float) args.TotalBytesTotal) * 100);
                    progress.Detail = string.Format("{0} processed", StringUtility.FormatByteCount(args.TotalBytesProcessed));

                    args.Cancel = progress.IsCancelRequested;
                };

                foreach (ZipReaderItem item in reader.ReadItems())
                {
                    // We are only looking for templates
                    if (item.Name.StartsWith(@"Application Data\Templates\"))
                    {
                        log.InfoFormat("Extracting '{0}'...", item.Name);

                        string targetPath = Path.Combine(tempPath, item.Name);
                        item.Extract(targetPath);
                    }
                }
            }

            if (!progress.IsCancelRequested)
            {
                progress.Detail = "Done";
                progress.PercentComplete = 100;
            }

            progress.Completed();

            return Path.Combine(tempPath, "Application Data");
        }

        /// <summary>
        /// Import the templates from the given path
        /// </summary>
        private void ImportTemplates(TemplateTree tree, string path, ProgressItem progress)
        {
            List<string> templateList = GenerateTemplateList(path);

            // If we didn't find them in the path provided, maybe they are in a Templates subdirectory.  This would be the case
            // if the selected the root Application Data folder.
            if (templateList.Count == 0)
            {
                path = Path.Combine(path, "Templates");

                templateList = GenerateTemplateList(path);
            }

            if (templateList.Count == 0)
            {
                throw new NotFoundException("No templates were found in the selected location.");
            }

            // Create the installer
            TemplateInstaller installer = new TemplateInstaller(path, TemplateVersionType.Version2);

            // add each template to be installed
            foreach (string templateName in templateList)
            {
                installer.AddToInstallQueue(templateName, importRootFolder + @"\" + templateName);
            }

            int count = 0;
            installer.TemplateInstalling += (object sender, TemplateInstallingEventArgs e) =>
                {
                    progress.Detail = string.Format("{0}...", e.OriginalFullName);
                    progress.PercentComplete = (100 * count++) / templateList.Count;
                };

            // Kick of the install
            installer.InstallQueuedTemplates(tree);
        }

        /// <summary>
        /// Generate the list of 2x templates to import based on the given path
        /// </summary>
        private List<string> GenerateTemplateList(string path)
        {
            List<string> templates = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    foreach (string subFolder in Directory.GetDirectories(directory))
                    {
                        if (File.Exists(Path.Combine(subFolder, "template.xsl")))
                        {
                            templates.Add(string.Format("{0}\\{1}", Path.GetFileName(directory), Path.GetFileName(subFolder)));
                        }
                    }
                }
            }

            return templates;
        }

        /// <summary>
        /// The background import has completed
        /// </summary>
        private void OnAsyncComplete(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new AsyncCallback(OnAsyncComplete), result);
                return;
            }

            Dictionary<string, object> userState = (Dictionary<string, object>) result.AsyncState;
            MethodInvoker<ProgressProvider, string> invoker = (MethodInvoker<ProgressProvider, string>) userState["invoker"];
            ProgressDlg progressDlg = (ProgressDlg) userState["progressDlg"];

            try
            {
                invoker.EndInvoke(result);

                // Now when we click next, we actually move on
                nextMovesOn = true;

                progressDlg.FormClosed += (object sender, FormClosedEventArgs e) => { Wizard.MoveNext(); };

                progressDlg.CloseForced();
            }
            catch (OperationCanceledException)
            {
                progressDlg.ProgressProvider.Cancel();
                progressDlg.CloseForced();
            }
            catch (NotFoundException)
            {
                progressDlg.CloseForced();

                DialogResult questionResult = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                    "No templates were found in the selected " + ((radioImportTemplatesAppData.Checked) ? "folder" : "backup file") + ".\n\n" +
                    "You can continue and ShipWorks will create the default ShipWorks 3 templates, or you can cancel and browse for your templates in another location.\n\n" +
                    "Continue without importing your templates?");

                if (questionResult == DialogResult.OK)
                {
                    radioDontImport.Checked = true;
                    Wizard.MoveNext();
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlScriptException || ex is SqlException || ex is ZipException || ex is IOException)
                {
                    log.ErrorFormat("An error occurred template import.", ex);
                    progressDlg.ProgressProvider.Terminate(ex);

                    MessageHelper.ShowError(progressDlg, string.Format("{0}", ex.Message));
                    progressDlg.CloseForced();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Choose the initial import template method
        /// </summary>
        private void ChooseInitialImportTemplateMethod()
        {
            ShipWorks2xApplicationDataSource source = ConfigurationMigrationState.ApplicationDataSource;

            switch (source.SourceType)
            {
                case ShipWorks2xApplicationDataSourceType.AppDataFolder:
                    radioImportTemplatesAppData.Checked = true;
                    importTemplatesAppDataFolder.Text = source.Path;
                    break;

                case ShipWorks2xApplicationDataSourceType.BackupFile:
                    radioImportTemplatesBackupFile.Checked = true;
                    importTemplatesBackupFile.Text = source.Path;
                    break;

                default:
                    radioDontImport.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// The method for importing templates is changing
        /// </summary>
        private void OnChangeImportTemplateMethod(object sender, EventArgs e)
        {
            importTemplatesAppDataFolder.Enabled = radioImportTemplatesAppData.Checked;
            browseAppData.Enabled = radioImportTemplatesAppData.Checked;

            importTemplatesBackupFile.Enabled = radioImportTemplatesBackupFile.Checked;
            browseBackupFile.Enabled = radioImportTemplatesBackupFile.Checked;
        }

        /// <summary>
        /// Browse for the location of the app data folder
        /// </summary>
        private void OnBrowseApplicationData(object sender, EventArgs e)
        {
            openAppDataFolder.SelectedPath = importTemplatesAppDataFolder.Text;

            if (openAppDataFolder.ShowDialog(this) == DialogResult.OK)
            {
                importTemplatesAppDataFolder.Text = openAppDataFolder.SelectedPath;
            }
        }

        /// <summary>
        /// Browse for the backup file to import templates
        /// </summary>
        private void OnBrowseBackupFile(object sender, EventArgs e)
        {
            openBackupFileDialog.FileName = importTemplatesBackupFile.Text;

            if (openBackupFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                importTemplatesBackupFile.Text = openBackupFileDialog.FileName;
            }
        }

    }
}
