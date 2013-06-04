using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Window for selecting a printer for a template that has not had one selected, or has an invalid one selected.
    /// </summary>
    public partial class TemplatePrinterSelectionDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplatePrinterSelectionDlg));

        List<TemplateEntity> toConfigure;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplatePrinterSelectionDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Looks for templates to be printed with in the collection if IPrintWithTemplates interfaces and ensures they are all configured with valid printers.
        /// </summary>
        public static bool EnsureConfigured(IWin32Window owner, IEnumerable<IPrintWithTemplates> templateProviders)
        {
            if (templateProviders == null)
            {
                return true;
            }

            HashSet<long> templateKeys = new HashSet<long>();

            foreach (IPrintWithTemplates provider in templateProviders)
            {
                foreach (long key in provider.TemplatesToPrintWith)
                {
                    templateKeys.Add(key);
                }
            }

            List<TemplateEntity> templates = new List<TemplateEntity>();

            foreach (long key in templateKeys)
            {
                TemplateEntity template = TemplateManager.Tree.GetTemplate(key);

                if (template != null)
                {
                    templates.Add(template);
                }
            }

            return EnsureConfigured(owner, templates.ToArray());
        }

        /// <summary>
        /// Ensures the given templates are all configured with valid printers.  If any are not configured on exit false is returned.
        /// </summary>
        public static bool EnsureConfigured(IWin32Window owner, params TemplateEntity[] templates)
        {
            if (templates == null || templates.Length == 0)
            {
                return true;
            }

            List<TemplateEntity> unconfigured = new List<TemplateEntity>();
            List<string> installedPrinters = null;

            // Add in any template with an unconfigured printer
            foreach (TemplateEntity template in templates.Distinct())
            {
                var settings = TemplateHelper.GetComputerSettings(template);

                if (string.IsNullOrWhiteSpace(settings.PrinterName))
                {
                    unconfigured.Add(template);
                }
                else
                {
                    try
                    {
                        if (installedPrinters == null)
                        {
                            installedPrinters = PrintUtility.InstalledPrinters;
                        }

                        if (!installedPrinters.Contains(settings.PrinterName))
                        {
                            unconfigured.Add(template);
                        }
                    }
                    catch (PrintingException ex)
                    {
                        log.Error("Could not load printer list", ex);

                        installedPrinters = new List<string>();
                        unconfigured.Add(template);
                    }
                }
            }

            if (unconfigured.Count > 0)
            {
                using (TemplatePrinterSelectionDlg dlg = new TemplatePrinterSelectionDlg())
                {
                    dlg.toConfigure = unconfigured;

                    return dlg.ShowDialog(owner) == DialogResult.OK;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            const int maxPanelHeight = 400;

            if (toConfigure == null || toConfigure.Count == 0)
            {
                throw new InvalidOperationException("toConfigure should have already been initialized to a non-empty list.");
            }

            labelInfo.Text += string.Format("{0}:", toConfigure.Count > 1 ? "s" : "");

            int top = 0;

            foreach (TemplateEntity template in toConfigure)
            {
                TemplateSelectPrinterControl selectControl = new TemplateSelectPrinterControl(template);
                selectControl.Location = new Point(0, top);
                panelContainer.Controls.Add(selectControl);

                top = selectControl.Bottom;
            }

            int idealPanelHeight = Math.Min(maxPanelHeight, top);
            this.Height = idealPanelHeight + 114;

            // If it got cutoff we need to allow scrolling
            if (idealPanelHeight == maxPanelHeight)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            }
        }

        /// <summary>
        /// Exit the window with the current settings
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (panelContainer.Controls.OfType<TemplateSelectPrinterControl>().Any(c => !c.IsPrinterSelected))
            {
                MessageHelper.ShowError(this,
                    string.Format("Please select a printer for {0} template.", toConfigure.Count == 1 ? "the" : "each"));

                return;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (TemplateEntity template in toConfigure)
                {
                    adapter.SaveAndRefetch(TemplateHelper.GetComputerSettings(template), false);
                }

                adapter.Commit();
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                foreach (TemplateEntity template in toConfigure)
                {
                    TemplateHelper.GetComputerSettings(template).RollbackChanges();
                }
            }
        }
    }
}
