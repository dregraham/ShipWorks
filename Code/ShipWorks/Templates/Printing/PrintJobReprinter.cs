using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Templates.Media;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Templates.Processing;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared;
using ShipWorks.Common.Threading;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Templates.Processing.TemplateXml;

namespace ShipWorks.Templates.Printing
{
    class PrintJobReprinter
    {
        string jobName;
        PrintResultEntity printResult;
        PrintJobSettings settings;

        TemplateResult templateResult;

        /// <summary>
        /// Raised when the async print operation has completed.
        /// </summary>
        public event AsyncCompletedEventHandler PrintCompleted;

        #region PrintRequest

        class PrintRequest
        {
            public object UserState { get; set; }
        }

        #endregion

        #region class BrowserResult

        /// <summary>
        /// Utility class for capturing the browser async response to a PrintRequest
        /// </summary>
        class BrowserResult
        {
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
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobReprinter(string jobName, PrintResultEntity printResult, PrintJobSettings settings)
        {
            this.jobName = jobName;
            this.settings = settings;

            this.printResult = new PrintResultEntity(printResult.Fields.Clone()) { IsNew = false };
        }

        /// <summary>
        /// Print the calibration page for the given printer, paper source, and page settings
        /// </summary>
        public void PrintAsync(object userState)
        {
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(PrintRequestCallback, "printing"),
                new PrintRequest
                {
                    UserState = userState
                });
        }

        /// <summary>
        /// Callback on another thread that does the actual printing
        /// </summary>
        private void PrintRequestCallback(object state)
        {
            PrintRequest request = (PrintRequest) state;

            AsyncCompletedEventArgs completionArgs;

            try
            {
                ExecuteRequest();

                // Everything was fine
                completionArgs = new AsyncCompletedEventArgs(null, false, request.UserState);
            }
            catch (Exception ex)
            {
                // Pass on the unknown exception as an error as is
                completionArgs = new AsyncCompletedEventArgs(ex, false, request.UserState);
            }

            // Callback the completion handler
            AsyncCompletedEventHandler handler = PrintCompleted;
            if (handler != null)
            {
                handler(this, completionArgs);
            }
        }

        /// <summary>
        /// Execute the specified print request
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ExecuteRequest()
        {
            // Ensure all the resources (images) needed are loaded
            DataResourceManager.LoadConsumerResourceReferences(printResult.PrintResultID);

            // Get the resource that contains the content
            DataResourceReference contentResource = DataResourceManager.LoadResourceReference(printResult.ContentResourceID);

            // Create the TemplateResult instance based on the previous content
            templateResult = new TemplateResult((TemplateXPathNavigator) null, contentResource.ReadAllText());

            // If its not thermal, print normally
            if (printResult.TemplateType != (int) TemplateType.Thermal)
            {
                BrowserPageSettings browserPageSettings = EffectivePageSettings;

                // First we have to ensure that the printer settings on the computer are what we need them to be
                using (PrinterConfigurationContext configContext = PrinterConfigurationContext.Activate(
                    PrintJobPriority.Normal,
                    settings.PrinterName,
                    settings.PaperSource,
                    (decimal) browserPageSettings.PageWidth,
                    (decimal) browserPageSettings.PageHeight))
                {
                    BrowserSpoolerSettings spoolerSettings = new BrowserSpoolerSettings(jobName, settings.Copies, settings.Collate);
                    BrowserCommunicationBridge bridge = new BrowserCommunicationBridge(spoolerSettings, browserPageSettings);

                    HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(null);
                    htmlControl.ExternalObject = bridge;

                    bridge.Error += new BrowserErrorEventHandler(OnPrintingError);
                    bridge.PrintingComplete += new EventHandler(OnPrintingComplete);

                    // Create the object to hold the results and Tag the bridge with the result so we have access to it in the callbacks
                    BrowserResult browserResult = new BrowserResult { SyncObject = new ManualResetEvent(false) };
                    bridge.Tag = browserResult;

                    try
                    {
                        // Start loading of the html content and wait for document completion
                        htmlControl.Html = GetPrintHtml();
                        htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                        // Process sure size now that its ready
                        TemplateSureSizeProcessor.Process(htmlControl);

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
                    }
                    finally
                    {
                        htmlControl.ExternalObject = null;

                        // Has to be disposed on the UI thread
                        htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));

                        bridge.Tag = null;
                        bridge.Error -= new BrowserErrorEventHandler(OnPrintingError);
                        bridge.PrintingComplete -= new EventHandler(OnPrintingComplete);
                    }
                }
            }
            // Print thermal
            else
            {
                for (int i = 1; i <= settings.Copies; i++)
                {
                    PrintThermalHelper.Print(templateResult, settings.PrinterName, jobName);
                }
            }
        }

        /// <summary>
        /// Get the full HTML to be printed
        /// </summary>
        private string GetPrintHtml()
        {
            TemplateResultFormatSettings formatSettings = TemplateResultFormatSettings.FromPrintResult(printResult);
            if (formatSettings.TemplateType == TemplateType.Label)
            {
                formatSettings.LabelPosition = settings.LabelSettings.LabelPosition;
            }

            return TemplateResultFormatter.FormatHtml(
                new TemplateResult[] { templateResult },
                TemplateResultUsage.Print,
                formatSettings);
        }

        /// <summary>
        /// Printing has completed
        /// </summary>
        private void OnPrintingComplete(object sender, EventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;
            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// Printing completed with errors
        /// </summary>
        private void OnPrintingError(object sender, BrowserErrorEventArgs e)
        {
            BrowserResult browserResult = (BrowserResult) ((BrowserCommunicationBridge) sender).Tag;

            browserResult.Error = true;
            browserResult.ErrorMessage = e.Message;
            browserResult.ErrorLine = e.Line;

            browserResult.SyncObject.Set();
        }

        /// <summary>
        /// Return the effective page settings based on either the standard template page setup, or a label template label sheet.
        /// </summary>
        private BrowserPageSettings EffectivePageSettings
        {
            get
            {
                BrowserPageSettings effectiveSettings = (settings.LabelSettings != null) ?
                    new BrowserPageSettings(settings.LabelSettings, PrinterCalibration.Load(settings.PrinterName, settings.PaperSource)) : 
                    new BrowserPageSettings(settings.PageSettings);

                return effectiveSettings;
            }
        }
    }
}
