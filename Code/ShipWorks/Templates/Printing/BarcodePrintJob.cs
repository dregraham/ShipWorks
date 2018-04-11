﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Metrics;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Barcode print job
    /// </summary>
    public class BarcodePrintJob : IPrintJob
    {
        private readonly IPrintJobFactory printJobFactory;
        private readonly IEnumerable<BarcodePage> barcodePages;
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;
        private Form owner;
        private readonly IPrintJob printJob;

        public event PrintActionCompletedEventHandler PreviewCompleted;
        public event PrintActionCompletedEventHandler PrintCompleted;


        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodePrintJob(IPrintJobFactory printJobFactory,
            IEnumerable<BarcodePage> barcodePages,
            Func<string, ITrackedEvent> telemetryEventFunc)
        {
            this.printJobFactory = printJobFactory;
            this.barcodePages = barcodePages;
            this.telemetryEventFunc = telemetryEventFunc;
            this.printJob = printJobFactory.CreatePrintJob(CreateTemplateResults());
            this.PreviewCompleted = new PrintActionCompletedEventHandler(OnPreivewCompleted);
            this.PrintCompleted = new PrintActionCompletedEventHandler(OnPrintCompleted);
        }

        /// <summary>
        /// Preview the barcode print job
        /// </summary>
        public void PreviewAsync(Form parent)
        {
            owner = parent;
            printJob.PreviewCompleted += PreviewCompleted;
            printJob.PreviewAsync(parent);
        }

        /// <summary>
        /// Handle the preview completing
        /// </summary>
        private void OnPreivewCompleted(object sender, PrintActionCompletedEventArgs e)
        {
            if (owner != null && owner.InvokeRequired)
            {
                owner.BeginInvoke((PrintActionCompletedEventHandler) OnPreivewCompleted, sender, e);
                return;
            }

            printJob.PreviewCompleted -= PreviewCompleted;
            
            if (!e.Cancelled)
            {
                PrintAsync();
            }
        }
        
        /// <summary>
        /// Create a list of template results to display 
        /// </summary>
        private IList<TemplateResult> CreateTemplateResults() =>
            barcodePages.Select(p => p.GetTemplateResult()).ToList();

        /// <summary>
        /// Print the barcodes
        /// </summary>
        public void PrintAsync()
        {
            printJob.PrintCompleted += PrintCompleted;
            printJob.PrintAsync();
        }

        /// <summary>
        /// Record telemetry after printing
        /// </summary>
        private void OnPrintCompleted(object sender, PrintActionCompletedEventArgs e)
        {
            if (owner != null && owner.InvokeRequired)
            {
                owner.BeginInvoke((PrintActionCompletedEventHandler) OnPrintCompleted, sender, e);
                return;
            }

            printJob.PrintCompleted -= PrintCompleted;

            if (!e.Cancelled)
            {
                string result = "Success";
                if (e.Error != null)
                {
                    result = "Failed";
                }

                int barcodeCount = barcodePages.Sum(p => p.Barcodes.Where(b => !string.IsNullOrWhiteSpace(b.Barcode)).Count());
                int hotkeyCount = barcodePages.Sum(p => p.Barcodes.Where(b => !string.IsNullOrWhiteSpace(b.KeyboardHotkey)).Count());

                using (ITrackedEvent telemetryEvent = telemetryEventFunc("Shortcuts.Print"))
                {
                    telemetryEvent.AddProperty("Shortcuts.Print.Result", result);
                    telemetryEvent.AddProperty("Shortcuts.Print.Barcodes.Count", barcodeCount.ToString());
                    telemetryEvent.AddProperty("Shortcuts.Print.Hotkeys.Count", hotkeyCount.ToString());
                }
            }
        }
    }
}
