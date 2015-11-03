using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using Interapptive.Shared;
using ShipWorks.Common.IO.Hardware.Printers;
using System.Diagnostics;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Used to set user printer settings for the duration of a ShipWorks print job.  This is needed because
    /// internet explorer does not provide a way to set the settings directly.
    /// </summary>
    public sealed class PrinterConfigurationContext : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PrinterConfigurationContext));

        bool active = true;

        string requestedPrinter;

        bool needRestorePrinter = false;
        string defaultPrinter;

        bool needRestorePaper = false;
        int paperSource;
        int paperSizeValue;
        int paperSizeLength;
        int paperSizeWidth;
        int paperOrientation;

        bool printBackground;

        // The print job queues that are pending, organized by priority
        static Dictionary<PrintJobPriority, Queue<ManualResetEvent>> prioritizedQueues = new Dictionary<PrintJobPriority, Queue<ManualResetEvent>>();

        // Event is signaled when the queues have pending requests
        static ManualResetEvent activationRequestsExistEvent = new ManualResetEvent(false);

        // Event is in the signaled state when there is no active context
        static ManualResetEvent noActiveContextEvent = new ManualResetEvent(true);

        static int requestCount = 0;

        #region class NativePaperSettings

        class NativePaperSettings
        {
            public int PaperSize { get; set; }
            public int PaperWidth { get; set; }
            public int PaperLength { get; set; }
        }

        #endregion

        #region class PrinterSettingsProxy

        /// <summary>
        /// A proxy to access printer settings through to improve performance
        /// </summary>
        class PrinterSettingsProxy
        {
            PrinterSettings printerSettings;

            string printerName;
            PageSettings pageSettings;
            PaperSize paperSize;
            bool? landscape;
            PaperSource paperSource;

            /// <summary>
            /// Constructor
            /// </summary>
            public PrinterSettingsProxy(PrinterSettings settings)
            {
                this.printerSettings = settings;
            }

            /// <summary>
            /// The name of the configured printer
            /// </summary>
            public string PrinterName
            {
                get
                {
                    if (printerName == null)
                    {
                        printerName = printerSettings.PrinterName;
                    }

                    return printerName;
                }
            }

            /// <summary>
            /// The default PaperSize of the printer
            /// </summary>
            public PaperSize PaperSize
            {
                get
                {
                    if (paperSize == null)
                    {
                        paperSize = InternalPageSettings.PaperSize;
                    }

                    return paperSize;
                }
            }

            /// <summary>
            /// The default PaperSource of the printer
            /// </summary>
            public PaperSource PaperSource
            {
                get
                {
                    if (paperSource==null)
                    {
                        paperSource = InternalPageSettings.PaperSource;
                    }

                    return paperSource;
                }
            }

            /// <summary>
            /// Determines if the printer is landscape
            /// </summary>
            public bool Landscape
            {
                get
                {
                    if (landscape == null)
                    {
                        landscape = InternalPageSettings.Landscape;
                    }

                    return landscape.Value;
                }
            }

            /// <summary>
            /// Our internal lazy PageSettings
            /// </summary>
            private PageSettings InternalPageSettings
            {
                get
                {
                    if (pageSettings == null)
                    {
                        pageSettings = printerSettings.DefaultPageSettings;
                    }

                    return pageSettings;
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        static PrinterConfigurationContext()
        {
            Thread thread = new Thread(ExceptionMonitor.WrapThread(ActivationQueueWorker));
            thread.IsBackground = true;
            thread.Name = "Printer Activation Queue Worker";

            thread.Start();
        }

        /// <summary>
        /// Must be created through a static call to Activate
        /// </summary>
        private PrinterConfigurationContext()
        {

        }

        /// <summary>
        /// Apply the specified configuration settings.  Only one context can be active at a time.  If another context
        /// is already active, this call blocks until its available.  If multiple activations are pending, the higher
        /// priority ones are services first.
        /// </summary>
        public static PrinterConfigurationContext Activate(PrintJobPriority priority, string printerName, int paperSource, decimal paperWidth, decimal paperHeight)
        {
            log.InfoFormat("Requesting printer configuration access: {0}, {1}, {2}, {3}", printerName, paperSource, paperWidth, paperHeight);

            int requestNumber = Interlocked.Increment(ref requestCount);

            // Make sure the printer is even valid on the computer, and return its settings
            PrinterSettingsProxy printerSettings = ValidatePrinter(printerName);

            // Request access to make the change
            WaitHandle waitHandle = RequestActivationAccess(priority);

            log.InfoFormat("Waiting on activation of request {0} ({1})", requestNumber, priority);

            // Wait for access to activate
            waitHandle.WaitOne();
            waitHandle.Close();

            log.InfoFormat("Granted activation of request {0} ({1})", requestNumber, priority);

            // Create the context that will be used to restore
            PrinterConfigurationContext context = new PrinterConfigurationContext();

            try
            {
                // Apply the setting for the background printing
                context.printBackground = SetIEPrintBackground(true);

                // First set the default printer
                ActivatePrinterSettings(context, printerSettings);

                // Then do the paper settings
                ActivatePaperSettings(context, printerSettings, paperSource, paperWidth, paperHeight);

                return context;
            }
            catch (Exception)
            {
                try
                {
                    // Try to put it back the way it was
                    context.Restore();
                }
                catch (Exception ex)
                {
                    log.Error("Could not restore context after failed activation.", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Restore the context to its original state
        /// </summary>
        private void Restore()
        {
            if (!active)
            {
                return;
            }

            try
            {
                // Restore the setting of background printing
                SetIEPrintBackground(printBackground);

                if (needRestorePaper)
                {
                    log.InfoFormat("Restoring settings of printer '{0}'.", requestedPrinter);
                    SetPaperSettings(requestedPrinter, paperSource, paperSizeValue, paperSizeLength, paperSizeWidth, paperOrientation);
                }

                // Restore the printer if necessary
                if (needRestorePrinter)
                {
                    log.InfoFormat("Restoring printer to '{0}'.", defaultPrinter);
                    SetDefaultPrinter(defaultPrinter);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to restore printer settings.", ex);

                throw;
            }
            finally
            {
                log.DebugFormat("Printer activation context released.");

                // No longer considered active
                active = false;
                noActiveContextEvent.Set();
            }
        }

        /// <summary>
        /// Active the initial request for changing the printer for a print job
        /// </summary>
        private static void ActivatePrinterSettings(PrinterConfigurationContext context, PrinterSettingsProxy printerSettings)
        {
            // We'll need this even if we don't change it, so that the paper settings restoration has access to it
            context.defaultPrinter = new PrinterSettings().PrinterName;
            context.requestedPrinter = printerSettings.PrinterName;

            // See if we need to change printers
            if (context.defaultPrinter != context.requestedPrinter)
            {
                log.InfoFormat("Change printer from {0} to {1}.", context.defaultPrinter, context.requestedPrinter);

                // Change the printer
                SetDefaultPrinter(context.requestedPrinter);

                // Update the context to know to restore it
                context.needRestorePrinter = true;
            }
        }

        /// <summary>
        /// Activate the initial request for changing the page settings for a print job
        /// </summary>
        private static void ActivatePaperSettings(PrinterConfigurationContext context, PrinterSettingsProxy printerSettings, int paperSource, decimal paperWidth, decimal paperHeight)
        {
            // Normalize the orientation
            decimal desiredHeight = Math.Max(paperWidth, paperHeight);
            decimal desiredWidth = Math.Min(paperWidth, paperHeight);
            bool isLandscape = (desiredWidth != paperWidth);

            // Get the width and height of the current page
            decimal width = printerSettings.PaperSize.Width / (decimal) 100;
            decimal height = printerSettings.PaperSize.Height / (decimal) 100;

            // Check to see whether we need to change the paper size
            bool needSetPaperSize = false;
            if (width != desiredWidth || height != desiredHeight)
            {
                needSetPaperSize = true;

                // The inches did not match... but i have seen where the PaperSize has values
                // in tenths of mm where Kind is custom
                if (printerSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    width = printerSettings.PaperSize.Width / (decimal) 254;
                    height = printerSettings.PaperSize.Height / (decimal) 254;

                    // If these match, then we dont need to set the paper size
                    if (width == desiredWidth && height == desiredHeight)
                    {
                        needSetPaperSize = false;
                    }
                }
            }

            // See if we need to change any settings
            if (paperSource != printerSettings.PaperSource.RawKind ||
                isLandscape != printerSettings.Landscape ||
                needSetPaperSize)
            {
                // Set the paper settings
                NativePaperSettings oldSettings = SetPaperSettings(
                    printerSettings.PrinterName,
                    paperSource,
                    0,
                    (int) (desiredHeight * 254),
                    (int) (desiredWidth * 254),
                    isLandscape ? 2 : 1);

                // Update the context to know its paper settings need restored
                context.needRestorePaper = true;

                context.paperSource = printerSettings.PaperSource.RawKind;
                context.paperOrientation = printerSettings.Landscape ? 2 : 1;

                context.paperSizeValue = oldSettings.PaperSize;
                context.paperSizeLength = oldSettings.PaperLength;
                context.paperSizeWidth = oldSettings.PaperWidth;
            }
        }

        /// <summary>
        /// Validates that the given printer exists on the computer
        /// </summary>
        private static PrinterSettingsProxy ValidatePrinter(string printerName)
        {
            if (string.IsNullOrEmpty(printerName))
            {
                throw new PrintingException("A printer has not been selected for printing.");
            }

            if (!PrintUtility.InstalledPrinters.Contains(printerName))
            {
                throw new PrintingException(string.Format("The printer '{0}' could not be found.", printerName));
            }

            // See if we can get the current printer settings
            try
            {
                PrinterSettings settings = new PrinterSettings();
                settings.PrinterName = printerName;

                var unused = settings.GetHdevmode();

                return new PrinterSettingsProxy(settings);
            }
            catch (InvalidPrinterException ex)
            {
                log.Error("ValidatePrinter '" + printerName + "'", ex);

                throw new PrintingException(string.Format("The printer '{0}' is not accessible.", printerName));
            }
            catch (Win32Exception ex)
            {
                log.Error("ValidatePrinter '" + printerName + "'", ex);

                throw new PrintingException(string.Format("The printer '{0}' is not accessible.", printerName));
            }
        }

        /// <summary>
        /// Changes to the specified printer
        /// </summary>
        private static void SetDefaultPrinter(string printerName)
        {
            if (!PrintSetDefaultPrinter(printerName, false))
            {
                ThrowFromLastWin32Error(printerName, string.Format("ShipWorks could not use the printer '{0}'.", printerName));
            }
        }

        /// <summary>
        /// Changes to the specified PaperSource
        /// </summary>
        [NDependIgnoreTooManyParams]
        private static NativePaperSettings SetPaperSettings(
            string printer,
            int paperSource,
            int size,
            int length,
            int width,
            int orientation)
        {
            log.InfoFormat("Set paper settings: Source = {0}, Size = {1}, Length = {2}, Width = {3}, Orientation = {4}", paperSource, size, length, width, orientation);

            int oldPaperSize = size;
            int oldPaperLength = length;
            int oldPaperWidth = width;

            // Change the system printer settings
            bool success = PrintSetPaperSettings(
                printer,
                paperSource,
                size, length, width, orientation,
                ref oldPaperSize, ref oldPaperLength, ref oldPaperWidth,
                false);

            if (!success)
            {
                ThrowFromLastWin32Error(printer, "ShipWorks could not configure the paper settings.");
            }

            return new NativePaperSettings { PaperSize = oldPaperSize, PaperLength = oldPaperLength, PaperWidth = oldPaperWidth };
        }

        /// <summary>
        /// Set printing of background colors and images in IE.  The value of the previous setting is returned
        /// </summary>
        public static bool SetIEPrintBackground(bool newValue)
        {
            bool oldValue = false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true))
            {
                if (key != null)
                {
                    oldValue = ((string) key.GetValue("Print_Background", "no") == "yes");

                    key.SetValue("Print_Background", newValue ? "yes" : "no");
                }
            }

            // This is really only needed for IE8+ - but it doesnt hurt to just always do it
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", true))
            {
                if (key != null)
                {
                    oldValue = ((string) key.GetValue("Print_Background", "no") == "yes");

                    key.SetValue("Print_Background", newValue ? "yes" : "no");
                }
            }
            return oldValue;
        }

        /// <summary>
        /// Throw an exception that is based on the Win32 GetLastError.
        /// </summary>
        private static void ThrowFromLastWin32Error(string printer, string message)
        {
            // Create this exception because it converts the code to a message
            Win32Exception win32Error = new Win32Exception(Marshal.GetLastWin32Error());

            // Access denied
            if (win32Error.NativeErrorCode == 5)
            {
                throw new PrinterConfigurationSecurityException(printer);
            }
            // Everything else
            else
            {
                string error = win32Error.Message;

                // "RPC server is unavailable."
                if (win32Error.NativeErrorCode == 1722)
                {
                    error = "The printer could not be found.";
                }

                throw new PrintingException(string.Format("{0}\n\n{1} ({2})",
                    message, error, win32Error.NativeErrorCode));
            }
        }

        /// <summary>
        /// Requests accesss to activate printer settings at the given priority level.  When the return handle is signaled, access has been granted.
        /// </summary>
        private static WaitHandle RequestActivationAccess(PrintJobPriority priority)
        {
            lock (prioritizedQueues)
            {
                Queue<ManualResetEvent> queue;
                if (!prioritizedQueues.TryGetValue(priority, out queue))
                {
                    queue = new Queue<ManualResetEvent>();
                    prioritizedQueues[priority] = queue;
                }

                // Create an even to represent this request
                ManualResetEvent activationEvent = new ManualResetEvent(false);

                // Add it to the queue to be scheduled
                queue.Enqueue(activationEvent);

                // Signal tha requests exist
                activationRequestsExistEvent.Set();

                return activationEvent;
            }
        }

        /// <summary>
        /// Worker thread for processing the activation request queues
        /// </summary>
        private static void ActivationQueueWorker()
        {
            while (true)
            {
                log.InfoFormat("PrinterActivation: Waiting for work");

                // There have to be requests to service, and there cant be an active context out there
                WaitHandle.WaitAll(new WaitHandle[] { activationRequestsExistEvent, noActiveContextEvent });

                lock (prioritizedQueues)
                {
                    ActivateNextInQueue();

                    int count = 0;
                    foreach (Queue<ManualResetEvent> queue in prioritizedQueues.Values)
                    {
                        count += queue.Count;
                    }

                    // If there are no more, clear the event
                    if (count == 0)
                    {
                        log.DebugFormat("PrinterActivation: No more requests.");
                        activationRequestsExistEvent.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Activate the next waiting item in the activation queue
        /// </summary>
        private static void ActivateNextInQueue()
        {
            // Go through each priority level from highest to lowset to find the next request to fulfill
            foreach (PrintJobPriority priorityLevel in Enum.GetValues(typeof(PrintJobPriority)).Cast<PrintJobPriority>().OrderByDescending(p => (int) p))
            {
                Queue<ManualResetEvent> queue;
                if (prioritizedQueues.TryGetValue(priorityLevel, out queue))
                {
                    if (queue.Count > 0)
                    {
                        log.InfoFormat("PrinterActivation: Scheduling work at priority {0}", priorityLevel);

                        // There is now outstanding work
                        noActiveContextEvent.Reset();

                        // This guy can now go
                        ManualResetEvent activationEvent = queue.Dequeue();
                        activationEvent.Set();

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Restore the configuration settings.
        /// </summary>
        public void Dispose()
        {
            Restore();
        }

        /// <summary>
        /// Sets the default printer
        /// </summary>
        [DllImport(@"ShipWorks.Native.dll", SetLastError = true, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintSetDefaultPrinter([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.Bool)] bool notify);

        /// <summary>
        /// Sets the default paper size
        /// </summary>
        [NDependIgnoreTooManyParams]
        [DllImport(@"ShipWorks.Native.dll", SetLastError = true, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintSetPaperSettings(
            [MarshalAs(UnmanagedType.LPStr)] string name,
            int paperSource,
            int size,
            int length,
            int width,
            int orientation,
            ref int oldsize,
            ref int oldlength,
            ref int oldwidth,
            [MarshalAs(UnmanagedType.Bool)] bool notify);

    }
}
