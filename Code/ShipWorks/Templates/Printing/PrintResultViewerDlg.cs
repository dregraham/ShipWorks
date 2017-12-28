using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Properties;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using ShipWorks.Templates.Processing;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Templates.Processing.TemplateXml;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Window for viewing a logged print output
    /// </summary>
    public partial class PrintResultViewerDlg : Form
    {
        long printResultID;
        PrintResultEntity printResult;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintResultViewerDlg(long printResultID)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

            this.printResultID = printResultID;
        }

        /// <summary>
        /// Don't show the window if the print has been deleted
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            printResult = (PrintResultEntity) DataProvider.GetEntity(printResultID);
            if (printResult == null)
            {
                MessageHelper.ShowInformation(this, "The selected print result has been deleted.");
                DialogResult = DialogResult.Cancel;
            }

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadHeader();
        }

        /// <summary>
        /// Load the print result header data
        /// </summary>
        private void LoadHeader()
        {
            entityImage.Image = EntityUtility.GetEntityImage(EntityUtility.GetEntityType(printResult.ContextObjectID));
            entityText.Text = ObjectLabelManager.GetLabel(printResult.ContextObjectID).LongText;

            printDate.Left = entityText.Right - 2;
            printDate.Text = "on " + StringUtility.FormatFriendlyDateTime(printResult.PrintDate);

            TemplateEntity template = TemplateManager.Tree.GetTemplate(printResult.TemplateID.Value);
            ObjectLabel label = ObjectLabelManager.GetLabel(printResult.TemplateID.Value);

            templateImage.Image = template != null ? TemplateHelper.GetTemplateImage(template) : Resources.delete2_16;
            templateName.Text = label.GetCustomText(false, false, true);

            printerName.Text = printResult.PrinterName;

            computerName.Left = printerName.Right - 2;
            computerName.Text = string.Format("on computer \\\\{0}", ComputerManager.GetComputer(printResult.ComputerID).Name);
        }

        /// <summary>
        /// The window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Make sure all our image resources are loaded
            DataResourceManager.LoadConsumerResourceReferences(printResult.PrintResultID);

            // Get the resource that contains the content
            DataResourceReference contentResource = DataResourceManager.LoadResourceReference(printResult.ContentResourceID);

            // If it was purged, show the placeholder.  We do this regardless of it was thermal or not.
            if (contentResource.IsPurgedPlaceholder)
            {
                htmlControl.Html = string.Format(TemplateHelper.ContentPurgedDisplayHtml, "print job");

                // Can't reprint a purged print job
                reprint.Enabled = false;
            }
            else
            {
                // Format the html for dispaly
                htmlControl.Html = TemplateResultFormatter.FormatHtml(
                    new TemplateResult[] { new TemplateResult((TemplateXPathNavigator) null, contentResource.ReadAllText()) },
                    TemplateResultUsage.ShipWorksDisplay,
                    TemplateResultFormatSettings.FromPrintResult(printResult));
            }

            // Process sure size now that its ready
            TemplateSureSizeProcessor.Process(htmlControl);
        }

        /// <summary>
        /// Open the print settings window to allow the user to reprint
        /// </summary>
        private void OnReprint(object sender, EventArgs e)
        {
            PrintJobSettings settings;

            if (printResult.LabelSheetID != null)
            {
                settings = new PrintJobSettings(new PrintJobLabelSettings(printResult.LabelSheetID.Value));
            }
            else
            {
                PrintJobPageSettings pageSettings = new PrintJobPageSettings(printResult);

                settings = new PrintJobSettings(pageSettings, printResult.TemplateType == (int) TemplateType.Thermal);
            }

            settings.PrinterName = printResult.PrinterName;
            settings.PaperSource = printResult.PaperSource;

            using (PrintJobSettingsDlg dlg = new PrintJobSettingsDlg(settings))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ReprintNow(settings);
                }
            }
        }

        /// <summary>
        /// Do the reprint now with the given page settings
        /// </summary>
        private void ReprintNow(PrintJobSettings settings)
        {
            ProgressProvider progressProvider = new ProgressProvider();

            ProgressItem progressItem = new ProgressItem("Printing");
            progressItem.CanCancel = false;
            progressItem.Detail = "Sending to printer...";
            progressItem.Starting();
            progressProvider.ProgressItems.Add(progressItem);

            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Printing";
            progressDlg.Description = "ShipWorks is reprinting.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            PrintJobReprinter printer = new PrintJobReprinter(entityText.Text, printResult, settings);
            printer.PrintCompleted += new AsyncCompletedEventHandler(OnPrintCompleted);
            printer.PrintAsync(delayer);

            delayer.ShowAfter(this, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Printing has completed
        /// </summary>
        void OnPrintCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((AsyncCompletedEventHandler) OnPrintCompleted, sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) e.UserState;
            delayer.NotifyComplete();

            PrintJobReprinter printer = (PrintJobReprinter) sender;
            printer.PrintCompleted -= this.OnPrintCompleted;

            // Check for errors
            if (e.Error != null)
            {
                // See if its an error we know how to handle.
                PrintingException printingEx = e.Error as PrintingException;
                if (printingEx != null)
                {
                    PrintingExceptionDisplay.ShowError(this, printingEx);
                }

                // Rethrow the error as is
                else
                {
                    throw new ApplicationException(e.Error.Message, e.Error);
                }
            }
        }
    }
}
