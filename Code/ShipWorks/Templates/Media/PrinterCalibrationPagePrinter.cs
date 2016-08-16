using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Printing;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Responsible for printing the page used for printer calibration
    /// </summary>
    public class PrinterCalibrationPagePrinter
    {
        /// <summary>
        /// Raised when the async print operation has completed.
        /// </summary>
        public event AsyncCompletedEventHandler PrintCompleted;

        #region PrintRequest

        class PrintRequest
        {
            public string Printer { get; set; }
            public int PaperSource { get; set; }
            public double PageWidth { get; set; }
            public double PageHeight { get; set; }
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
        /// Print the calibration page for the given printer, paper source, and page settings
        /// </summary>
        public void PrintAsync(string printer, int paperSource, double pageWidth, double pageHeight, object userState)
        {
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(PrintRequestCallback),
                new PrintRequest
                {
                    Printer = printer,
                    PaperSource = paperSource,
                    PageWidth = pageWidth,
                    PageHeight = pageHeight,
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
                ExecuteRequest(request);

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
        private void ExecuteRequest(PrintRequest request)
        {
            // First we have to ensure that the printer settings on the computer are what we need them to be
            using (PrinterConfigurationContext configContext = PrinterConfigurationContext.Activate(
                PrintJobPriority.Normal,
                request.Printer,
                request.PaperSource,
                (decimal) request.PageWidth,
                (decimal) request.PageHeight))
            {
                BrowserCommunicationBridge bridge = new BrowserCommunicationBridge(
                    new BrowserSpoolerSettings("ShipWorks - Calibration", 1, false),
                    new BrowserPageSettings(request.PageWidth, request.PageHeight));

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
                    htmlControl.Html = GenerateCalibrateHtml(request.PageWidth, request.PageHeight);
                    htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

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
        /// Generate the html that will output the centered cross
        /// </summary>
        [NDependIgnoreLongMethod]
        private string GenerateCalibrateHtml(double pageWidth, double pageHeight)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html>");
            sb.Append("<style>   body {margin:0; padding:0; background-color:white; } </style>");
            sb.Append("<body>");

            decimal xMiddle = (decimal) pageWidth / 2;
            decimal yMiddle = (decimal) pageHeight / 2;

            decimal xStart = xMiddle - 1m;
            decimal yStart = yMiddle - 1m;

            // Add the main x in the middle
            AppendHtmlLine(sb, xMiddle, yStart, xMiddle, yMiddle + 1m, "B");
            AppendHtmlLine(sb, xStart, yMiddle, xMiddle + 1m, yMiddle, "A");

            // Do all the lines on the x axis
            for (decimal x = -.9m; x <= .9m; x += .1m)
            {
                if (x != 0)
                {
                    decimal height;
                    string label;

                    if ((Math.Abs(x * 10) % 2) == 0)
                    {
                        height = .1m;
                        label = ((int) (x * 10)).ToString();
                    }
                    else
                    {
                        height = .05m;
                        label = "";
                    }

                    if (x == .2m)
                    {
                        label = "";
                    }

                    AppendHtmlLine(sb, xMiddle + x, yMiddle - height, xMiddle + x, yMiddle + height, label);
                }
            }

            // Do all the lines on the y axis
            for (decimal y = -.9m; y <= .9m; y += .1m)
            {
                if (y != 0)
                {
                    decimal width;
                    string label;

                    if ((Math.Abs(y * 10) % 2) == 0)
                    {
                        width = .1m;
                        label = ((int) (-y * 10)).ToString();
                    }
                    else
                    {
                        width = .05m;
                        label = "";
                    }

                    if (y == .2m)
                    {
                        label = "";
                    }

                    AppendHtmlLine(sb, xMiddle - width, yMiddle + y, xMiddle + width, yMiddle + y, label);
                }
            }

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        /// <summary>
        /// Append an HTML line via an absolte position div
        /// </summary>
        [NDependIgnoreTooManyParams]
        private void AppendHtmlLine(StringBuilder sb, decimal xStart, decimal yStart, decimal xStop, decimal yStop, string label)
        {
            decimal width = xStop - xStart;
            decimal height = yStop - yStart;

            decimal length = width > 0 ? width : height;
            string lengthSide = width > 0 ? "width" : "height";
            string borderSide = width > 0 ? "top" : "left";

            sb.AppendFormat(@"
                <div style='position: absolute; padding: 0; margin: 0;  font-size: 0%;
                            left: {0}in; top: {1}in;
                            {2}: {3}in;
                            border-{4}: 1px solid black;'>
                    &nbsp;
                </div>",
                xStart, yStart,
                lengthSide, length,
                borderSide);

            if (!string.IsNullOrEmpty(label))
            {
                decimal labelX = (width > 0) ? xStart + width + .10m : xStart - .03m;
                decimal labelY = (width > 0) ? yStart - .06m : yStart + height + .05m;

                sb.AppendFormat(@"
                <div style='position: absolute; padding: 0; margin: 0; 
                            font: normal 7.0pt Tahoma;
                            left: {0}in; top: {1}in;'>
                    {2}
                </div>",
                    labelX, labelY, label);
            }
        }
    }
}
