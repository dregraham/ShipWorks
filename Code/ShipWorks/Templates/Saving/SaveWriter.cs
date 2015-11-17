using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;
using ShipWorks.Common.Threading;
using System.Threading;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Controls.Html;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using System.IO;
using log4net;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using System.Net;
using Interapptive.Shared;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// Encapsulates the writing out of a template save file request.
    /// </summary>
    public class SaveWriter
    {        
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SaveWriter));

        // The template to use.  This is a cloned snapshot of the template at the time the job was created.
        TemplateEntity template;

        // The entities used in template processing
        List<long> entityKeys;

        // Template processing results
        IList<TemplateResult> templateResults;

        // Provides progress through the status of the save
        ProgressProvider progressProvider = new ProgressProvider();

        /// <summary>
        /// Raised when a file name or folder name is needed to continue the save process.  Must be handled.
        /// </summary>
        public event SavePromptForFileEventHandler PromptForFile;
        
        /// <summary>
        /// Raised when saving is complete.
        /// </summary>
        public event SaveCompletedEventHandler SaveCompleted;

        // If there are multiple results, and the user wants prompted just for the folder, this will be the folder
        string promptOnceFolder;

        // Tracks the file names that have been saved so far
        List<string> savedFiles = new List<string>();

        // The userState for the save operation
        object userState;

        /// <summary>
        /// Private constructor.  Use the Create method instead.
        /// </summary>
        private SaveWriter()
        {

        }

        /// <summary>
        /// Create a new SaveWriter based on the default settings from the template and the given set of keys
        /// </summary>
        public static SaveWriter Create(TemplateEntity template, IEnumerable<long> entityKeys)
        {
            SaveWriter writer = new SaveWriter();
            writer.entityKeys = entityKeys.ToList();

            Initialize(writer, template);

            return writer;
        }

        /// <summary>
        /// Create a new SaveWriter based on the default settings from the template and the given set of already processed input results
        /// </summary>
        public static SaveWriter Create(TemplateEntity template, IList<TemplateResult> templateResults)
        {
            SaveWriter writer = new SaveWriter();
            writer.templateResults = templateResults.ToList();

            Initialize(writer, template);

            return writer;
        }

        /// <summary>
        /// Initialize the given job with the specified template and the template's default settings
        /// </summary>
        private static void Initialize(SaveWriter writer, TemplateEntity template)
        {
            if (template.Type == (int) TemplateType.Thermal)
            {
                throw new SaveException(
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be saved.");
            }

            writer.template = template;
            writer.template.IsNew = false;
        }

        /// <summary>
        /// Exposes the current set of operations the saving is working on and their status.
        /// </summary>
        public ProgressProvider ProgressProvider
        {
            get { return progressProvider; }
        }

        /// <summary>
        /// Save synchronously
        /// </summary>
        public void Save()
        {
            try
            {
                EnsureTemplateResults();

                SaveWorker();
            }
            catch (TemplateCancelException)
            {
                throw new InvalidOperationException("Should not be possible to cancel the synchronous operation.");
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to print using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in a SaveException
                throw new SaveException(message, ex);
            }
            catch (Exception ex)
            {
                if (ShouldHandleNonTemplateException(ex))
                {
                    throw new SaveException(ex.Message, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initiates the save process asyncronously.
        /// </summary>
        public void SaveAsync(object userState)
        {
            progressProvider.ProgressItems.Clear();
            this.userState = userState;

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(SaveRequestCallback, "saving"));
        }

        /// <summary>
        /// Worker thread for executing a save request
        /// </summary>
        private void SaveRequestCallback(object state)
        {
            // The args for completion
            SaveCompletedEventArgs completionArgs;

            try
            {
                EnsureTemplateResults();

                // Initiate the worker.  It returns false if it was canceled
                bool canceled = !SaveWorker();

                // Everything was fine
                completionArgs = new SaveCompletedEventArgs(savedFiles, null, canceled, userState);
            }
            catch (TemplateCancelException)
            {
                // Don't pass on the exception, mark it as cancelled
                completionArgs = new SaveCompletedEventArgs(savedFiles, null, true, userState);
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to save using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in a SaveException
                completionArgs = new SaveCompletedEventArgs(savedFiles, new SaveException(message, ex), false, userState);
            }
            catch (Exception ex)
            {
                if (ShouldHandleNonTemplateException(ex))
                {
                    // Wrap known exceptions
                    completionArgs = new SaveCompletedEventArgs(savedFiles, new SaveException(ex.Message, ex), false, userState);
                }
                else
                {
                    // Pass on the unknown exception as an error as is
                    completionArgs = new SaveCompletedEventArgs(savedFiles, ex, false, userState);
                }
            }

            // Callback the completion handler
            if (SaveCompleted != null)
            {
                SaveCompleted(this, completionArgs);
            }

            this.userState = null;
        }

        /// <summary>
        /// Ensure that the template results have been processed
        /// </summary>
        private void EnsureTemplateResults()
        {
            if (templateResults != null)
            {
                return;
            }

            // Need the progress item
            ProgressItem progress = new ProgressItem("Preparing");
            progressProvider.ProgressItems.Add(progress);

            // Prepare the results
            templateResults = TemplateProcessor.ProcessTemplate(template, entityKeys, progress);

            if (templateResults.Count == 0)
            {
                throw new SavingNoTemplateOutputException(TemplateHelper.NoResultsErrorMessage);
            }
        }

        /// <summary>
        /// Do the actual work of the save. Returns false if canceled, true otherwise.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool SaveWorker()
        {
            // Create the html control that will be used
            // Add a progress item for the actual saving
            ProgressItem saveProgress = new ProgressItem("Saving");
            progressProvider.ProgressItems.Add(saveProgress);
            saveProgress.Starting();

            HtmlControl htmlControl = null;

            bool processHtml = TemplateHelper.GetEffectiveOutputFormat(template) == TemplateOutputFormat.Html;
            if (processHtml)
            {
                htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(null);
            }

            try
            {
                saveProgress.Detail = "Saving files...";

                // Create the settings to use for formatting the html
                TemplateResultFormatSettings settings = TemplateResultFormatSettings.FromTemplate(template);

                while (settings.NextResultIndex < templateResults.Count && !progressProvider.CancelRequested)
                {
                    int startResult = settings.NextResultIndex;
                    int endResult;

                    string contentToSave;

                    // If the results are saved as HTML, we have to do all our html processing
                    if (processHtml)
                    {
                        htmlControl.Html = TemplateResultFormatter.FormatHtml(
                            templateResults,
                            TemplateResultUsage.Save,
                            settings);

                        htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                        // Process sure size now that its ready
                        TemplateSureSizeProcessor.Process(htmlControl);

                        // Since it may be opened or viewed later in word, we have to do this, since word screws up without it
                        HtmlUtility.SetExplicitImageSizes(htmlControl);

                        // The final html is the content to save
                        contentToSave = htmlControl.Html;
                        endResult = settings.NextResultIndex;
                    }
                    // Otherwise just determine the results to use ourself
                    else
                    {
                        contentToSave = templateResults[settings.NextResultIndex++].ReadResult();
                        endResult = startResult + 1;
                    }

                    // Determine the filename
                    string filename = AcquireFilename(templateResults.Skip(startResult).Take(endResult - startResult), templateResults.Count);

                    // Null means the user canceled while being prompted.
                    if (filename == null)
                    {
                        progressProvider.Cancel();
                    }
                    else
                    {
                        // Save the file
                        SaveFile(contentToSave, filename);

                        // Add to the list of files that were saved
                        savedFiles.Add(filename);

                        // Update how much we are done
                        saveProgress.PercentComplete = (100 * settings.NextResultIndex)  / templateResults.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                saveProgress.Failed(ex);

                throw;
            }
            finally
            {
                // Has to be disposed on the UI thread
                if (htmlControl != null)
                {
                    htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));
                }
            }

            // Mark it as completed
            saveProgress.Completed();

            // Return true if we were not canceled
            return saveProgress.Status != ProgressItemStatus.Canceled;
        }

        /// <summary>
        /// Acquire the filename based on the given results, and given that there were the given number of total results from the selection.
        /// Returns null if the user is prompted but cancels.
        /// </summary>
        [NDependIgnoreLongMethod]
        private string AcquireFilename(IEnumerable<TemplateResult> results, int totalResults)
        {
            List<long> entityKeys = new List<long>();
            foreach (TemplateResult result in results)
            {
                entityKeys.AddRange(result.XPathSource.Input.ContextKeys);
            }

            // We will always need to process the name
            string name = TemplateTokenProcessor.ProcessTokens(template.SaveFileName, entityKeys);
            string folder = TemplateTokenProcessor.ProcessTokens(template.SaveFileFolder, entityKeys);

            name = name.Trim();
            folder = folder.Trim();

            if (name.Length == 0)
            {
                name = template.Name;
            }

            // Strip all illegals from the name, and everything but the \ from the folder.  We do this because
            // after token processing you could have an item name like "Big Ball!@#$?" be the filename - and we don't
            // want it to just fail.
            name = PathUtility.CleanFileName(name);
            folder = PathUtility.CleanPath(folder);

            // Add the extension if there is not one
            if (name.Length != 0 && !Path.HasExtension(name))
            {
                name += "." + TemplateHelper.GetDefaultFileExtension(TemplateHelper.GetEffectiveOutputFormat(template));
            }

            if (folder.Length == 0)
            {
                folder = TemplateHelper.DefaultTemplateSaveDirectory;
            }

            // Ensure the folder is a folder
            if (!Path.IsPathRooted(folder))
            {
                Path.Combine(TemplateHelper.DefaultTemplateSaveDirectory, folder);
            }

            if (!folder.EndsWith(@"\"))
            {
                folder += @"\";
            }

            SavePromptWhen promptWhen = (SavePromptWhen) template.SaveFilePrompt;

            // Never prompt
            if (promptWhen == SavePromptWhen.Never)
            {
                if (Path.GetFileNameWithoutExtension(name).Length == 0)
                {
                    throw new SaveException("The template is configured to never prompt when saving but a default file name is not configured.");
                }

                // When not prompting we automatically ensure the name is unique amongst files already saved.
                return EnsureUniqueFileName(Path.Combine(folder, name));
            }

            // Prompt once for folder
            else if (promptWhen == SavePromptWhen.Once && totalResults > 1)
            {
                if (Path.GetFileNameWithoutExtension(name).Length == 0)
                {
                    throw new SaveException("The template is configured to only prompt once for the folder, but a default file name is not configured.");
                }

                if (promptOnceFolder == null)
                {
                    promptOnceFolder = RaisePromptForFile(null, folder, SaveFileNamePart.Folder);

                    // User canceled
                    if (promptOnceFolder == null)
                    {
                        return null;
                    }
                }

                // When not prompting we automatically ensure the name is unique amongst files already saved.
                return EnsureUniqueFileName(Path.Combine(promptOnceFolder, name));
            }

            // Prompt for the file
            else
            {
                return RaisePromptForFile(name, folder, SaveFileNamePart.FullName);
            }
        }

        /// <summary>
        /// Ensures that the given filename is unique among the filenames we have already saved in this operation.  This ensures that for a multiple
        /// selection where the user is not being prompted, but where they are stupid and don't have the template configured with a tokenized
        /// name that is unique per selected item, that they still get a file per template result.
        /// </summary>
        private string EnsureUniqueFileName(string filename)
        {
            if (!savedFiles.Contains(filename))
            {
                return filename;
            }

            string directory = Path.GetDirectoryName(filename);
            string extention = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);

            int index = 1;
            string nameToTry;

            do
            {
                nameToTry = Path.Combine(directory, name) + string.Format(" ({0}){1}", index++, extention);
            }
            while (savedFiles.Contains(nameToTry));

            return nameToTry;
        }

        /// <summary>
        /// Raise the event that must be handled to ask the user for the filename.  Returns null if the user cancels.
        /// </summary>
        [NDependIgnoreLongMethod]
        private string RaisePromptForFile(string name, string folder, SaveFileNamePart part)
        {
            SavePromptForFileEventHandler handler = PromptForFile;
            if (handler == null)
            {
                throw new InvalidOperationException("Must handle the PromptForFile event on SaveWriter.");
            }

            string folderToDelete = null;

            // The browser windows cannot select the initial folder if it does not exist.  Try to create it, so that it can be selected.
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                try
                {
                    int index = folder.Length - 1;
                    string baseMissingDirectory = folder;

                    // We need to find the shortest possible path that does not exist - that't the one we'll need to cleanup.
                    while (index > 0)
                    {
                        string folderToCheck = folder.Substring(0, index + 1);

                        if (Directory.Exists(folderToCheck))
                        {
                            break;
                        }

                        baseMissingDirectory = folderToCheck;
                        index = folder.LastIndexOf(@"\", index - 1);
                    }

                    log.InfoFormat("Creating folder for default prompt selection. '{0}' ('{1}')", folder, baseMissingDirectory);

                    Directory.CreateDirectory(folder);
                    folderToDelete = baseMissingDirectory;
                }
                catch (IOException ex)
                {
                    log.Error("Could not create folder for default prompt selection.", ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    log.Error("Could not create folder for default prompt selection.", ex);
                }
            }

            try
            {
                SavePromptForFileEventArgs args = new SavePromptForFileEventArgs(name, folder, part, GetFileFilter(), userState);
                handler(this, args);

                if (args.Cancel)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(args.ResultName))
                {
                    throw new InvalidOperationException("Cancel was not indicated but no result path was given.");
                }

                return args.ResultName;
            }
            finally
            {
                if (folderToDelete != null)
                {
                    try
                    {
                        Directory.Delete(folderToDelete, true);
                    }
                    catch (IOException ex)
                    {
                        log.Error("Could not delete folder for default prompt selection.", ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        log.Error("Could not delete folder for default prompt selection.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Get the file filter that can be used to filter file extensions based on the template's output settings
        /// </summary>
        private string GetFileFilter()
        {
            string filter;

            switch (TemplateHelper.GetEffectiveOutputFormat(template))
            {
                case TemplateOutputFormat.Html:
                    filter = "HTML Files (*.htm; *.html)|*.htm;*.html";
                    break;

                case TemplateOutputFormat.Xml:
                    filter = "XML Files (*.xml)|*.xml";
                    break;

                case TemplateOutputFormat.Text:
                    filter = "Text Files (*.txt)|*.txt" + "|" +
                             "CSV Files (*.csv)|*.csv";
                    break;

                default:
                    throw new InvalidOperationException("Unhandled TemplateOutputFormat value.");
            }

            // Always let them see all files if they want
            filter += "|All Files (*.*)|*.*";

            return filter;
        }

        /// <summary>
        /// Get the encoding to use for saving files
        /// </summary>
        private Encoding GetFileSaveEncoding()
        {
            Encoding encoding = null;

            // We have to create these manually in order to specify if the BOM should be written
            switch (template.OutputEncoding.ToLowerInvariant())
            {
                case "utf-8":
                    encoding = new UTF8Encoding(template.SaveFileBOM);
                    break;

                case "utf-16":
                    encoding = new UnicodeEncoding(false, template.SaveFileBOM);
                    break;

                case "utf-32":
                    encoding = new UTF32Encoding(false, template.SaveFileBOM);
                    break;
            }

            // For all other encodings that don't support BOM, just use the factory
            if (encoding == null)
            {
                try
                {
                    encoding = Encoding.GetEncoding(template.OutputEncoding);
                }
                catch (ArgumentException ex)
                {
                    throw new SaveException(string.Format("'{0}' is not a supported encoding.", template.OutputEncoding), ex);
                }
            }

            return encoding;
        }

        /// <summary>
        /// Save the given result content
        /// </summary>
        private void SaveFile(string contentToSave, string filename)
        {
            // Ensure we have access to create the directory
            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            // For any non-html output, just save it as-is
            if (TemplateHelper.GetEffectiveOutputFormat(template) != TemplateOutputFormat.Html)
            {
                using (StreamWriter writer = new StreamWriter(filename, false, GetFileSaveEncoding()))
                {
                    writer.Write(contentToSave);
                }
            }
            // For html files we will need to copy all local resources, and potentiall the online ones as well.
            else
            {
                // Windows knows how to manage html files and folders with the filename and a _files as a single unit
                string resourceSubDirectory  = Path.GetFileNameWithoutExtension(filename) + "_files";
                string resourceFolder = Path.Combine(Path.GetDirectoryName(filename), resourceSubDirectory);

                TemplateHtmlImageProcessor imageProcessor = new TemplateHtmlImageProcessor();
                imageProcessor.LocalImages = true;
                imageProcessor.OnlineImages = template.SaveFileOnlineResources;

                // Process all the images in the document
                contentToSave = imageProcessor.Process(contentToSave, (HtmlAttribute attribute, Uri srcUri, string imageName) =>
                    {
                        // Ensure the target folder exists.  We do this lazily so that if there were no images to process
                        // we dont create it without reason.
                        Directory.CreateDirectory(resourceFolder);

                        // Try to copy it to the dest folder.  May fail if local file is not present or internet is down
                        try
                        {
                            // WebRequest works both on file:// and http:// uris
                            WebRequest webRequest = WebRequest.Create(srcUri);

                            using (WebResponse webResponse = webRequest.GetResponse())
                            {
                                using (Stream resourceStream = webResponse.GetResponseStream())
                                {
                                    StreamUtility.WriteToFile(resourceStream, Path.Combine(resourceFolder, imageName));
                                }
                            }

                            // Update to the new relatively qualified src attribute
                            attribute.Value = Path.Combine(resourceSubDirectory, imageName);
                        }
                        catch (Exception ex)
                        {
                            // We are here b\c a copy operation failed.  Missing one file does
                            // not fail the whole process.  Lots could go wrong, so for now I am having
                            // it just catch the general Exception case.
                            log.Error(string.Format("Error localizing URI '{0}'.", srcUri), ex);
                        }
                    });

                // Save the update html content
                using (StreamWriter writer = new StreamWriter(filename, false, GetFileSaveEncoding()))
                {
                    writer.Write(contentToSave);
                }
            }
        }

        /// <summary>
        /// Indicates if an Exception should be translated to a SaveException
        /// </summary>
        private static bool ShouldHandleNonTemplateException(Exception ex)
        {
            return ex is IOException || ex is UnauthorizedAccessException;
        }
    }
}
