using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Templates.Processing;
using System.ComponentModel;
using System.Threading;
using ShipWorks.UI.Controls.Html;
using System.Windows.Forms;
using System.IO;
using ShipWorks.UI.Controls.Html.Core;
using ShipWorks.ApplicationCore;
using System.Diagnostics;
using Interapptive.Shared;
using System.Drawing;
using ShipWorks.Properties;
using ShipWorks.Templates.Media;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Drawing.Printing;
using ShipWorks.ApplicationCore.Interaction;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using log4net;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared.Win32;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Represents a print job that ShipWorks needs to fulfill.
    /// </summary>
    public class PrintJob
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintJob));

        // The template to use.  This is a cloned snapshot of the template at the time the job was created.
        TemplateEntity template;
        
        // The entities used in template processing.  Can be null if constructed with the final results directly
        List<long> entityKeys;

        // The settings of the print job
        PrintJobSettings jobSettings;

        // Template processing results
        IList<TemplateResult> templateResults;

        // Provides progress through the status of the print job
        ProgressProvider progressProvider = new ProgressProvider();

        // Unique ID of this print job
        Guid jobIdentifier = Guid.NewGuid();

        /// <summary>
        /// Raised when printing is complete.
        /// </summary>
        public event PrintActionCompletedEventHandler PrintCompleted;

        /// <summary>
        /// Raised when the preview window becomes visible
        /// </summary>
        public event PrintPreviewShownEventHandler PreviewShown;

        /// <summary>
        /// Raised when previewing is complete.
        /// </summary>
        public event PrintActionCompletedEventHandler PreviewCompleted;

        #region class PrintRequest

        /// <summary>
        /// Utility class for passing state across threads for a print request
        /// </summary>
        class PrintRequest
        {
            public PrintAction Action { get; set; }
            public Control HtmlOwner { get; set; }
            public object UserState { get; set; }
            public ApplicationBusyToken BusyToken { get; set; }
        }

        #endregion

        #region class BrowserResult

        /// <summary>
        /// Utility class for capturing the browser async response to a PrintRequest
        /// </summary>
        class BrowserResult
        {
            // The request that triggered the creation of this result
            PrintRequest request;

            /// <summary>
            /// Constructor
            /// </summary>
            public BrowserResult(PrintRequest request)
            {
                this.request = request;
            }

            /// <summary>
            /// The original request
            /// </summary>
            public PrintRequest PrintRequest { get { return request; } }

            /// <summary>
            /// Used to synchronize the initiating of the request and its completion
            /// </summary>
            public ManualResetEvent SyncObject { get; set; }

            /// <summary>
            /// Indicates that an error occurred within the browser
            /// </summary>
            public bool Error { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorLine { get; set; }

            /// <summary>
            /// Indicates if a print preview operation was canceled (the user chose not to continue with printing)
            /// </summary>
            public bool Canceled { get; set; }
        }

        #endregion

        /// <summary>
        /// Private constructor.  Use the Create method instead.
        /// </summary>
        private PrintJob()
        {

        }

        /// <summary>
        /// Create a new PrintJob based on the default settings from the template and the given set of keys
        /// </summary>
        public static PrintJob Create(TemplateEntity template, IEnumerable<long> entityKeys)
        {
            PrintJob job = new PrintJob();
            job.entityKeys = entityKeys.ToList();

            InitializeJob(job, template);

            return job;
        }

        /// <summary>
        /// Create a new PrintJob based on the default settings from the template and the given set of already processed input results
        /// </summary>
        public static PrintJob Create(TemplateEntity template, IList<TemplateResult> templateResults)
        {
            PrintJob job = new PrintJob();
            job.templateResults = templateResults.ToList();

            InitializeJob(job, template);

            return job;
        }

        /// <summary>
        /// Initialize the given job with the specified template and the template's default settings
        /// </summary>
        private static void InitializeJob(PrintJob job, TemplateEntity template)
        {
            job.template = template;

            // Initialize the settings
            if (template.Type == (int) TemplateType.Label)
            {
                job.jobSettings = new PrintJobSettings(new PrintJobLabelSettings(template.LabelSheetID));
            }
            else
            {
                job.jobSettings = new PrintJobSettings(new PrintJobPageSettings(template), template.Type == (int) TemplateType.Thermal);
            }

            TemplateComputerSettingsEntity settings = TemplateHelper.GetComputerSettings(template);
            job.Settings.PrinterName = settings.PrinterName;
            job.Settings.PaperSource = settings.PaperSource;

            job.Settings.Copies = template.PrintCopies;
            job.Settings.Collate = template.PrintCollate;
        }

        /// <summary>
        /// Exposes the current set of operations the print job is working on and their status.
        /// </summary>
        public ProgressProvider ProgressProvider
        {
            get { return progressProvider; }
        }

        /// <summary>
        /// Print syncronously.
        /// </summary>
        public void Print()
        {
            PrintRequest request = new PrintRequest { Action = PrintAction.Print };

            try
            {
                EnsureTemplateResults();

                PrintWorker(request);
            }
            catch (TemplateCancelException)
            {
                throw new InvalidOperationException("Should not be possible to cancel the syncronous operation.");
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to print using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in a PrintingException
                throw new PrintingException(message, ex);
            }
        }

        /// <summary>
        /// Print the job asynchronously.
        /// </summary>
        public void PrintAsync()
        {
            PrintAsync(null);
        }

        /// <summary>
        /// Preview the job asynchronously.
        /// </summary>
        public void PreviewAsync(Form parent)
        {
            PreviewAsync(parent, null);
        }

        /// <summary>
        /// Print the job asynchronously.  The userState is passed when the PrintCompleted callback is invoked.
        /// </summary>
        public void PrintAsync(object userState)
        {
            StartPrintRequest(PrintAction.Print, new Control(), userState);
        }

        /// <summary>
        /// Preview the job asynchronously.  The userState is passed when the PreviewCompleted callback is invoked.
        /// </summary>
        public void PreviewAsync(Form parent, object userState)
        {
            StartPrintRequest(PrintAction.Preview, parent, userState);
        }

        /// <summary>
        /// Start a print request using the given callback as the async function to execute when ready.
        /// </summary>
        private void StartPrintRequest(PrintAction action, Control htmlOwner, object userState)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            progressProvider.ProgressItems.Clear();

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(PrintRequestCallback), 
                new PrintRequest 
                {
                    Action = action, 
                    HtmlOwner = htmlOwner, 
                    UserState = userState,
                    BusyToken = ApplicationBusyManager.OperationStarting("printing")
                });
        }

        /// <summary>
        /// Worker thread for executing a print request
        /// </summary>
        private void PrintRequestCallback(object state)
        {
            PrintRequest request = (PrintRequest) state;

            // Determine the worker to execute and the completion handler to call at the end of the request
            Func<PrintRequest, bool> requestWorker = (request.Action == PrintAction.Print) ? (Func<PrintRequest, bool>) PrintWorker : (Func<PrintRequest, bool>) PreviewWorker;
            PrintActionCompletedEventHandler completionHandler = (request.Action == PrintAction.Print) ? PrintCompleted : PreviewCompleted;

            // The args for completion
            PrintActionCompletedEventArgs completionArgs;

            try
            {
                EnsureTemplateResults();

                // Initiate the worker.  It returns false if it was canceled
                bool canceled = !requestWorker(request);

                // Everything was fine
                completionArgs = new PrintActionCompletedEventArgs(request.Action, null, canceled, request.UserState);
            }
            catch (TemplateCancelException)
            {
                // Don't pass on the exception, mark it as cancelled
                completionArgs = new PrintActionCompletedEventArgs(request.Action, null, true, request.UserState);
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to print using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in a PrintingException
                completionArgs = new PrintActionCompletedEventArgs(request.Action, new PrintingException(message, ex), false, request.UserState);
            }
            catch (Exception ex)
            {
                // Pass on the unknown exception as an error as is
                completionArgs = new PrintActionCompletedEventArgs(request.Action, ex, false, request.UserState);
            }

            // Could be null if we are executing syncronously
            if (request.BusyToken != null)
            {
                ApplicationBusyManager.OperationComplete(request.BusyToken);
            }

            // Callback the completion handler
            if (completionHandler != null)
            {
                completionHandler(this, completionArgs);
            }
        }

        /// <summary>
        /// The worker thread that executes a print job.  Returns true if the worker completed, or false if it was cancelled.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool PrintWorker(PrintRequest printRequest)
        {
            // Add a progress item for the actual printing
            ProgressItem printProgress = new ProgressItem("Printing");
            progressProvider.ProgressItems.Add(printProgress);

            // "Normal" pringing
            if (template.Type != (int) TemplateType.Thermal)
            {
                printProgress.CanCancel = false;
                printProgress.Starting();
                printProgress.Detail = "Preparing printer...";

                // First we have to ensure that the printer settings on the computer are what we need them to be
                using (PrinterConfigurationContext configContext = PrinterConfigurationContext.Activate(
                    PrintJobPriority.Normal,
                    Settings.PrinterName,
                    Settings.PaperSource,
                    (decimal) EffectivePageSettings.PageWidth,
                    (decimal) EffectivePageSettings.PageHeight))
                {
                    // Create the communication bridge to the browser
                    BrowserCommunicationBridge browserBridge = CreateCommmunicationBridge();

                    // Create the html control that will be used
                    HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(printRequest.HtmlOwner);
                    htmlControl.ExternalObject = browserBridge;

                    // The index of the last one that was successful
                    int lastSuccessIndex = 0;

                    try
                    {
                        printProgress.Detail = "Sending content to printer...";

                        // Create the settings to use for formatting the html
                        TemplateResultFormatSettings settings = CreateResultFormatSettings();

                        while (settings.NextResultIndex < templateResults.Count)
                        {
                            // Load and prepare the html. Includes SureSize processing
                            PrepareHtml(browserBridge, htmlControl, TemplateResultUsage.Print, settings);

                            // Create the object to hold the results
                            BrowserResult browserResult = new BrowserResult(printRequest);
                            browserResult.SyncObject = new ManualResetEvent(false);

                            // Tag the bridge with the result so we have access to it in the callbacks
                            browserBridge.Tag = browserResult;

                            // Print the document using our print template
                            htmlControl.Print(TemplateOutputUtility.PrintTemplatePath);

                            // Wait for the browser to be done
                            browserResult.SyncObject.WaitOne();
                            browserResult.SyncObject.Close();

                            // Throw if there was an error
                            if (browserResult.Error)
                            {
                                throw new PrintingException(string.Format("{0} ({1})", browserResult.ErrorMessage, browserResult.ErrorLine));
                            }

                            // Update the last index that was successfully sent to the printer
                            lastSuccessIndex = settings.NextResultIndex - 1;

                            // Update how much we are done
                            printProgress.PercentComplete = (100 * settings.NextResultIndex) / templateResults.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        printProgress.Failed(ex);

                        throw;
                    }
                    finally
                    {
                        // Mark it as completed (if not already failed)
                        if (printProgress.Status != ProgressItemStatus.Failure)
                        {
                            printProgress.Completed();
                        }

                        // Log the result of the print job
                        PrintResultLogger.LogPrintJob(jobIdentifier, templateResults.Take(lastSuccessIndex + 1).ToList(), Settings, progressProvider);

                        htmlControl.ExternalObject = null;

                        // Has to be disposed on the UI thread
                        htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));

                        // Destroy the bridge
                        DestroyCommunicationBridge(browserBridge);
                    }
                }
            }

            // Thermal printing
            else
            {
                printProgress.Starting();
                printProgress.Detail = "Sending content to printer...";

                List<TemplateResult> succeeded = new List<TemplateResult>();

                try
                {
                    // For now thermal's always get printed as collated.  If we ever changed this, keep in mind that each thermal template
                    // result can contain multiple ThermalLabel tags that would have to be broken apart to be uncollated.
                    for (int i = 1; i <= Settings.Copies; i++)
                    {
                        // Print each template result to the thermal printer
                        foreach (TemplateResult result in templateResults)
                        {
                            if (PrintThermalHelper.Print(result, Settings.PrinterName, template.Name))
                            {
                                succeeded.Add(result);
                            }
                            else
                            {
                                log.WarnFormat("Skipping thermal label for result {0} due to no thermal data present in Label tag.", result.XPathSource.Input.ContextKeys[0]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    printProgress.Failed(ex);

                    throw;
                }
                finally
                {
                    // Mark it as completed (if not already failed)
                    if (printProgress.Status != ProgressItemStatus.Failure)
                    {
                        printProgress.Completed();
                    }

                    // Log the result of the print job
                    PrintResultLogger.LogPrintJob(jobIdentifier, succeeded, Settings, progressProvider);
                }
            }

            // Return true if we were not canceled
            return printProgress.Status != ProgressItemStatus.Canceled;
        }

        /// <summary>
        /// The worker thread that prepares and shows a preview. Returns true if the worker completed, or false if it was cancelled.
        /// </summary>
        private bool PreviewWorker(PrintRequest printRequest)
        {
            // Add a progress item for the actual printing
            ProgressItem previewProgress = new ProgressItem("Previewing");
            progressProvider.ProgressItems.Add(previewProgress);

            previewProgress.CanCancel = false;
            previewProgress.Starting();
            previewProgress.Detail = "Generating document content...";

            // Create the communication bridge to the browser
            BrowserCommunicationBridge browserBridge = CreateCommmunicationBridge();
            browserBridge.WindowPosition.ParentBounds = ((Form) printRequest.HtmlOwner).DesktopBounds;

            // Create the html control that will be used
            HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(printRequest.HtmlOwner);
            htmlControl.ExternalObject = browserBridge;

            // Turn on printing of backgrounds for the preview
            bool oldIEPrintBackground = PrinterConfigurationContext.SetIEPrintBackground(true);

            try
            {
                // Load and prepare the html. Includes SureSize processing
                PrepareHtml(browserBridge, htmlControl, TemplateResultUsage.PrintPreview, CreateResultFormatSettings());

                // Create the object to hold the results
                BrowserResult browserResult = new BrowserResult(printRequest);
                browserResult.SyncObject = new ManualResetEvent(false);

                // Tag the bridge with the result so we have access to it in the callbacks
                browserBridge.Tag = browserResult;

                // Preview the document using our print template
                previewProgress.Detail = "Showing preview...";
                htmlControl.Preview(TemplateOutputUtility.PrintTemplatePath);

                // Wait for the browser to be done
                browserResult.SyncObject.WaitOne();
                browserResult.SyncObject.Close();

                // Throw if there was an error
                if (browserResult.Error)
                {
                    throw new PrintingException(string.Format("{0} ({1})", browserResult.ErrorMessage, browserResult.ErrorLine));
                }

                // Return to indicate if the user canceled or wants to print
                return !browserResult.Canceled;
            }
            finally
            {
                // Restore the background printing setting
                PrinterConfigurationContext.SetIEPrintBackground(oldIEPrintBackground);

                htmlControl.ExternalObject = null;

                // Has to be disposed on the UI thread
                htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));

                // Destroy the bridge
                DestroyCommunicationBridge(browserBridge);
            }
        }

        /// <summary>
        /// Prepare the html in the given htmlControl for display
        /// </summary>
        private void PrepareHtml(BrowserCommunicationBridge bridge, HtmlControl htmlControl, TemplateResultUsage usage, TemplateResultFormatSettings settings)
        {
            // Capture what the start label position will be, in case its a label sheet
            LabelPosition startPosition = (settings.TemplateType == TemplateType.Label) ? new LabelPosition(settings.LabelPosition) : null;

            // Where we are starting from
            int startResultIndex = settings.NextResultIndex;

            htmlControl.Html = TemplateResultFormatter.FormatHtml(
                templateResults,
                usage,
                settings);

            // Count how many results we actually used
            int resultsUsed = settings.NextResultIndex - startResultIndex;

            htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

            // Process sure size now that its ready
            TemplateSureSizeProcessor.Process(htmlControl);

            // Update the document content information so the browser can display appropriately.  This is just used to warn the user if results were truncated and the preview isn't
            // going to show the whole thing at once.
            bridge.DocumentContent.TemplateResultsDisplayed = resultsUsed;
            bridge.DocumentContent.TemplateResultsTotal = templateResults.Count;

            // Set whether this is for a label sheet or not
            bridge.DocumentContent.IsLabelSheet = (settings.TemplateType == TemplateType.Label);

            // If its a label sheet, we have to set how many pages there should be.  Due to printer calibration and using margins for offsetting, IE can
            // think there is overflow when there really should not be.  This way we can force the # of pages to be truncated to this number.
            if (bridge.DocumentContent.IsLabelSheet)
            {
                int cellsUsed = (startPosition.Row - 1) * settings.LabelSheet.Columns + (startPosition.Column - 1) + resultsUsed;
                int pagesUsed = (int) Math.Ceiling((double) cellsUsed / (double) (settings.LabelSheet.Columns * settings.LabelSheet.Rows));

                bridge.DocumentContent.LabelSheetsRequired = pagesUsed;
            }
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
                throw new PrintingNoTemplateOutputException(TemplateHelper.NoResultsErrorMessage);
            }
        }

        /// <summary>
        /// Create the communication bridge to the browser and hook all the events we listen to.
        /// </summary>
        private BrowserCommunicationBridge CreateCommmunicationBridge()
        {
            BrowserSpoolerSettings spoolerSettings = new BrowserSpoolerSettings(template.Name, Settings.Copies, Settings.Collate);
            BrowserCommunicationBridge bridge = new BrowserCommunicationBridge(spoolerSettings, EffectivePageSettings);

            bridge.PrintingComplete += new EventHandler(OnBrowserCallbackPrintingComplete);

            bridge.PreviewShown += new EventHandler(OnBrowserCallbackPreviewShown);
            bridge.PreviewPrintNow += new CancelEventHandler(OnBrowserCallbackPreviewPrintNow);
            bridge.PreviewCancel += new EventHandler(OnBrowserCallbackPreviewCanceled);

            bridge.ShowSettings += new EventHandler(OnBrowserCallbackShowSettings);
            bridge.Error += new BrowserErrorEventHandler(OnBrowserCallbackError);

            return bridge;
        }

        /// <summary>
        /// Unhook all the events that we had hooked into for the communication bridge.
        /// </summary>
        private void DestroyCommunicationBridge(BrowserCommunicationBridge bridge)
        {
            bridge.Tag = null;

            bridge.PrintingComplete -= new EventHandler(OnBrowserCallbackPrintingComplete);

            bridge.PreviewShown -= new EventHandler(OnBrowserCallbackPreviewShown);
            bridge.PreviewPrintNow -= new CancelEventHandler(OnBrowserCallbackPreviewPrintNow);
            bridge.PreviewCancel -= new EventHandler(OnBrowserCallbackPreviewCanceled);

            bridge.ShowSettings -= new EventHandler(OnBrowserCallbackShowSettings);
            bridge.Error -= new BrowserErrorEventHandler(OnBrowserCallbackError);
        }

        /// <summary>
        /// Create the result format settings to use when formatting the template output results
        /// </summary>
        private TemplateResultFormatSettings CreateResultFormatSettings()
        {
            TemplateResultFormatSettings formatSettings = TemplateResultFormatSettings.FromTemplate(template);
            if (Settings.LabelSettings != null)
            {
                formatSettings.LabelSheet = LabelSheetManager.GetLabelSheet(Settings.LabelSettings.LabelSheetID);
                formatSettings.LabelPosition = Settings.LabelSettings.LabelPosition;
            }

            return formatSettings;
        }

        /// <summary>
        /// The browser control has completed printing of the document
        /// </summary>
        void OnBrowserCallbackPrintingComplete(object sender, EventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;
            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// The browser control has made the preview window visible to the user
        /// </summary>
        void OnBrowserCallbackPreviewShown(object sender, EventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;

            UpdateIEPreviewWindowIcon();

            if (PreviewShown != null)
            {
                PreviewShown(this, new PrintPreviewShownEventArgs(browserResult.PrintRequest.UserState));
            }
        }

        /// <summary>
        /// User clicked the Print button from the preview window
        /// </summary>
        void OnBrowserCallbackPreviewPrintNow(object sender, CancelEventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;

            bool showjobSettings = false;

            try
            {
                if (!PrintUtility.InstalledPrinters.Contains(Settings.PrinterName))
                {
                    showjobSettings = true;
                }
            }
            catch (PrintingException)
            {
                // If we couldn't even enumerate the printer list show the job settings
                showjobSettings = true;
            }

            // Have to present the user with 
            if (showjobSettings)
            {
                IntPtr ieHandle = FindIEPreviewWindowHandle();

                using (PrintJobSettingsDlg dlg = new PrintJobSettingsDlg(Settings))
                {
                    // For some reason showing this kills the icon we had set
                    Control htmlOwner = browserResult.PrintRequest.HtmlOwner;
                    htmlOwner.BeginInvoke(new MethodInvoker(UpdateIEPreviewWindowIcon));

                    if (dlg.ShowDialog(new NativeWindowHandle(ieHandle)) != DialogResult.OK)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            // Ready to go
            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// User closed the preview without wanting to print
        /// </summary>
        void OnBrowserCallbackPreviewCanceled(object sender, EventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;
            browserResult.Canceled = true;
            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// User wants to see the settings window
        /// </summary>
        void OnBrowserCallbackShowSettings(object sender, EventArgs e)
        {
            BrowserCommunicationBridge bridge = (BrowserCommunicationBridge) sender;
            BrowserResult browserResult = (BrowserResult) bridge.Tag;
            Control htmlOwner = browserResult.PrintRequest.HtmlOwner;

            IntPtr ieHandle = FindIEPreviewWindowHandle();

            using (PrintJobSettingsDlg dlg = new PrintJobSettingsDlg(Settings))
            {
                // For some reason showing this kills the icon we had set
                htmlOwner.BeginInvoke(new MethodInvoker(UpdateIEPreviewWindowIcon));

                dlg.AcceptButtonText = "OK";
                if (dlg.ShowDialog(new NativeWindowHandle(ieHandle)) == DialogResult.OK)
                {
                    // Update the new effective settings
                    bridge.PageSettings = EffectivePageSettings;

                    // If its a label template, we may have to regenerate the content completely of the label settings changed
                    if (template.Type == (int) TemplateType.Label && dlg.LabelSettingsChanged)
                    {
                        // Create a new html control to process the new results
                        HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(htmlOwner);

                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            // Load and prepare the html. Includes SureSize processing
                            PrepareHtml(bridge, htmlControl, TemplateResultUsage.PrintPreview, CreateResultFormatSettings());

                            // Create the new url to hold the results for the content source
                            string tempHtmlFile = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString("N") + ".tmp");
                            File.WriteAllText(tempHtmlFile, htmlControl.Html);

                            bridge.DocumentContent.ContentSource = tempHtmlFile;
                        }
                        finally
                        {
                            // Has to be disposed on the UI thread
                            htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));
                        }
                    }
                }
            }

            // And sometimes ive seen it go away after it closes
            htmlOwner.BeginInvoke(new MethodInvoker(UpdateIEPreviewWindowIcon));
        }

        /// <summary>
        /// An error occurred in the browser while trying to print or preview
        /// </summary>
        void OnBrowserCallbackError(object sender, BrowserErrorEventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;

            browserResult.Error = true;
            browserResult.ErrorMessage = e.Message;
            browserResult.ErrorLine = e.Line;

            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// Find the handle to the currently displayed IE print preview window
        /// </summary>
        private IntPtr FindIEPreviewWindowHandle()
        {
            // Need it as an object to pass by ref
            object data = IntPtr.Zero;

            // Try to find the running instance
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsCallback(EnumWindowsIEPreviewWindow), ref data);

            IntPtr hWnd = (IntPtr) (int) data;

            // If we don't find it, check that the EnumWindows function is using the correct window title
            Debug.Assert(hWnd != IntPtr.Zero);

            return hWnd;
        }

        /// <summary>
        /// Callback to pass to EnumWindows.  Looks for another version of the appication
        /// running, and if it finds one sets the window handle in lParam.
        /// </summary>
        private static bool EnumWindowsIEPreviewWindow(IntPtr hWnd, ref object lParam)
        {
            StringBuilder sb = new StringBuilder(50);
            NativeMethods.GetWindowText(hWnd, sb, sb.Capacity - 1);

            // Found it, stop the search
            if (sb.ToString() == "Print Preview")
            {
                // Also have to check that it belongs to this process
                int wndProcessID;
                NativeMethods.GetWindowThreadProcessId(hWnd, out wndProcessID);

                if (wndProcessID == Process.GetCurrentProcess().Id)
                {
                    lParam = (object) hWnd;

                    // Stop search
                    return false;
                }
            }

            // Continue search 
            return true;
        }

        /// <summary>
        /// Set our own icon for the IE window
        /// </summary>
        private void UpdateIEPreviewWindowIcon()
        {
            // Get the handle to the browser window
            IntPtr ieHandle = FindIEPreviewWindowHandle();

            // Clear the IE icon.
            NativeMethods.SendMessage(ieHandle, NativeMethods.WM_SETICON, (IntPtr) 0, Resources.printPreviewIcon.Handle);
            NativeMethods.SendMessage(ieHandle, NativeMethods.WM_SETICON, (IntPtr) 1, Resources.printPreviewIcon.Handle);
        }

        /// <summary>
        /// Return the effective page settings based on either the standard template page setup, or a label template label sheet.
        /// </summary>
        private BrowserPageSettings EffectivePageSettings
        {
            get
            {
                BrowserPageSettings effectiveSettings = (template.Type == (int) TemplateType.Label) ?
                    new BrowserPageSettings(Settings.LabelSettings, PrinterCalibration.Load(Settings.PrinterName, Settings.PaperSource)) : 
                    new BrowserPageSettings(Settings.PageSettings);

                return effectiveSettings;
            }
        }


        /// <summary>
        /// The template to use for processing
        /// </summary>
        public long TemplateID
        {
            get { return template.TemplateID; }
        }

        /// <summary>
        /// Allows for control over the settings of the print job
        /// </summary>
        public PrintJobSettings Settings
        {
            get { return jobSettings; }
        }
    }
}
