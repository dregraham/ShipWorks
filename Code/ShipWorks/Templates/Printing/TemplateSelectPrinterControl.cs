using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing.Printing;
using Interapptive.Shared.UI;
using ShipWorks.Templates.Media;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// User control representing a single template that needs to have a printer selected for it
    /// </summary>
    public partial class TemplateSelectPrinterControl : UserControl
    {
        TemplateEntity template;

        bool isConfigured = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateSelectPrinterControl(TemplateEntity template)
        {
            InitializeComponent();

            this.template = template;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (Parent.Controls.IndexOf(this) > 0)
            {
                kryptonBorderEdge.Visible = true;
            }

            templateIcon.Image = TemplateHelper.GetTemplateImage(template);
            templateName.Text = template.FullName;

            UpdatePrinterDisplay();
        }

        /// <summary>
        /// Update the display to show the current template settings printer selection
        /// </summary>
        private void UpdatePrinterDisplay()
        {
            var settings = TemplateHelper.GetComputerSettings(template);
            if (string.IsNullOrWhiteSpace(settings.PrinterName))
            {
                labelMissing.Left = printerName.Left;
                labelMissing.Text = "None Selected";
                labelMissing.Visible = true;

                printerName.Visible = false;
            }
            else
            {
                printerName.Text = settings.PrinterName;
                printerName.ForeColor = isConfigured ? SystemColors.ControlText : SystemColors.GrayText;
                printerName.Visible = true;

                labelMissing.Left = printerName.Right;
                labelMissing.Visible = !isConfigured;
            }
        }

        /// <summary>
        /// Indicates if the user has successfully configured a printer
        /// </summary>
        public bool IsPrinterSelected
        {
            get { return isConfigured; }
        }

        /// <summary>
        /// Use the default printer
        /// </summary>
        private void OnUseDefault(object sender, EventArgs e)
        {
            var templateSettings = TemplateHelper.GetComputerSettings(template);
            var printerSettings = PrinterSettingFactory.GetDefaultPrinterSettings();

            if (printerSettings.IsValid)
            {
                templateSettings.PrinterName = printerSettings.PrinterName;
                isConfigured = true;

                UpdatePrinterDisplay();
            }
            else
            {
                MessageHelper.ShowError(this, "ShipWorks could not find a default printer on your computer.");
            }
        }

        /// <summary>
        /// Let the user choose the specific printer to use
        /// </summary>
        private void OnChoosePrinter(object sender, EventArgs e)
        {
            var settings = TemplateHelper.GetComputerSettings(template);
            using (PrinterSelectionDlg dlg = new PrinterSelectionDlg(settings.PrinterName, settings.PaperSource))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    settings.PrinterName = dlg.PrinterName;
                    settings.PaperSource = dlg.PaperSource;
                    isConfigured = true;

                    UpdatePrinterDisplay();
                }
            }
        }
    }
}
