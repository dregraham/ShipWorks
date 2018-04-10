using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IDictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutData;
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;
        private const string HTMLContent = "<html><head><title></title><style>body {{font-family:Arial; text-align:center;}}table {{margin-bottom:40px;}} td {{text-align:center;}} .barcode {{font-family:Free 3 of 9 Extended;font-size:36pt;}}</style></head><body>{0}</body></html>";
        private Form owner;
        private readonly IPrintJob printJob;

        public event PrintActionCompletedEventHandler PreviewCompleted;
        public event PrintActionCompletedEventHandler PrintCompleted;


        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodePrintJob(IPrintJobFactory printJobFactory,
            IDictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutData,
            Func<string, ITrackedEvent> telemetryEventFunc)
        {
            this.printJobFactory = printJobFactory;
            this.shortcutData = shortcutData;
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
        private IList<TemplateResult> CreateTemplateResults()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> section in shortcutData)
            {
                builder.AppendLine($"<H1>{section.Key}</H1>");

                foreach ((string Name, string Barcode, string KeyboardShortcut) shortcut in section.Value)
                {
                    string barcode = string.IsNullOrWhiteSpace(shortcut.Barcode) ? string.Empty : $"*{shortcut.Barcode}*";
                    if (!string.IsNullOrWhiteSpace(barcode) || !string.IsNullOrWhiteSpace(shortcut.KeyboardShortcut))
                    {
                        builder.AppendLine(CreateBarcodeElement(shortcut.Name, barcode.ToUpper(), shortcut.KeyboardShortcut));
                    }
                }
            }

            return new List<TemplateResult>()
            {
                new TemplateResult(null,string.Format(HTMLContent, builder.ToString()))
            };
        }

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

                int barcodeCount = shortcutData.Sum(s => s.Value.Count(v => !string.IsNullOrWhiteSpace(v.Barcode)));
                int hotkeyCount = shortcutData.Sum(s => s.Value.Count(v => !string.IsNullOrWhiteSpace(v.KeyboardShortcut)));

                using (ITrackedEvent telemetryEvent = telemetryEventFunc("Shortcuts.Print"))
                {
                    telemetryEvent.AddProperty("Shortcuts.Print.Result", result);
                    telemetryEvent.AddProperty("Shortcuts.Print.Barcodes.Count", barcodeCount.ToString());
                    telemetryEvent.AddProperty("Shortcuts.Print.Hotkeys.Count", hotkeyCount.ToString());
                }
            }
        }

        /// <summary>
        /// Create the barcode element
        /// </summary>
        private static string CreateBarcodeElement(string name, string barcode, string shortcutKey) =>
            $"<table><tr><td> {name} </td></tr><tr><td class='barcode'>{barcode}</td></tr><tr><td>{shortcutKey}</td></tr></table>";
    }
}
