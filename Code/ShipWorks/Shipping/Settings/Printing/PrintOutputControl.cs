using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Actions;
using ShipWorks.Templates.Printing;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// UserControl for editing print output settings for a shipment type
    /// </summary>
    public partial class PrintOutputControl : UserControl, IPrintWithTemplates
    {
        ShipmentType shipmentType;

        ActionEntity actionPrint;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintOutputControl()
        {
            InitializeComponent();

            toolStripAddPrintOutput.Renderer = new NoBorderToolStripRenderer();

            // only show the Install Missing if the secret handshake is done
            installMissingLink.Visible = InterapptiveOnly.MagicKeysDown;
        }

        /// <summary>
        /// Load the settings for the given shipment type
        /// </summary>
        public void LoadSettings(ShipmentType shipmentType)
        {
            this.shipmentType = shipmentType;

            actionPrint = ShippingActionUtility.GetPrintAction(shipmentType.ShipmentTypeCode);
            printActionBox.Checked = actionPrint.Enabled;

            LoadOutputGroups();

            UpdateLayout();
        }

        /// <summary>
        /// Load all the output groups into the UI
        /// </summary>
        private void LoadOutputGroups()
        {
            foreach (ShippingPrintOutputEntity outputGroup in ShippingPrintOutputManager.GetOutputGroups(shipmentType.ShipmentTypeCode))
            {
                AddOutputGroupControl(outputGroup);
            }
        }

        /// <summary>
        /// Add a control for editing the given output group
        /// </summary>
        private void AddOutputGroupControl(ShippingPrintOutputEntity outputGroup)
        {
            PrintOutputGroupControl outputGroupControl = new PrintOutputGroupControl();
            outputGroupControl.Width = panelMain.Width;
            outputGroupControl.Dock = DockStyle.Top;

            outputGroupControl.Initialize(outputGroup);
            outputGroupControl.DeleteClicked += new EventHandler(OnDeleteGroup);
            outputGroupControl.RuleCountChanged += new EventHandler(OnRuleCountChanged);
            outputGroupControl.ReloadRequired += new EventHandler(OnReloadRequired);

            panelMain.Controls.Add(outputGroupControl);
            panelMain.Controls.SetChildIndex(outputGroupControl, 0);
        }

        /// <summary>
        /// Remove the given output group control from the UI
        /// </summary>
        private void RemoveOutputGroupControl(PrintOutputGroupControl outputGroupControl)
        {
            outputGroupControl.DeleteClicked -= this.OnDeleteGroup;
            outputGroupControl.RuleCountChanged -= new EventHandler(OnRuleCountChanged);
            outputGroupControl.ReloadRequired -= new EventHandler(OnReloadRequired);

            panelMain.Controls.Remove(outputGroupControl);
        }

        /// <summary>
        /// Add a new output group
        /// </summary>
        private void OnAddGroup(object sender, EventArgs e)
        {
            ShippingPrintOutputEntity outputGroup = new ShippingPrintOutputEntity();
            outputGroup.ShipmentType = (int) shipmentType.ShipmentTypeCode;

            using (PrintOutputGroupNameDlg dlg = new PrintOutputGroupNameDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    outputGroup.Name = dlg.PrintOutputName.Trim();
                }
                else
                {
                    return;
                }
            }
            
            // Add an initial rule
            ShippingPrintOutputRuleEntity rule = new ShippingPrintOutputRuleEntity();
            rule.ShippingPrintOutput = outputGroup;
            rule.FilterNodeID = 0;
            rule.TemplateID = 0;

            ShippingPrintOutputManager.SaveOutputGroup(outputGroup);

            AddOutputGroupControl(outputGroup);

            UpdateLayout();
        }

        /// <summary>
        /// Delete the output group that generated the event
        /// </summary>
        void OnDeleteGroup(object sender, EventArgs e)
        {
            PrintOutputGroupControl outputGroupControl = (PrintOutputGroupControl) sender;
            ShippingPrintOutputEntity outputGroup = outputGroupControl.OutputGroup;

            ShippingPrintOutputManager.DeleteOutputGroup(outputGroup);

            RemoveOutputGroupControl(outputGroupControl);

            UpdateLayout();
        }

        /// <summary>
        /// A rule has been added or removed from one of the groups
        /// </summary>
        void OnRuleCountChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        /// <summary>
        /// Something has changed with a child group that indicates we should just do a total reload
        /// </summary>
        void OnReloadRequired(object sender, EventArgs e)
        {
            panelMain.SuspendLayout();

            while (panelMain.Controls.Count > 0)
            {
                RemoveOutputGroupControl(panelMain.Controls[0] as PrintOutputGroupControl);
            }

            ShippingPrintOutputManager.CheckForChangesNeeded();
            LoadOutputGroups();

            panelMain.ResumeLayout();

            UpdateLayout();
        }

        /// <summary>
        /// Update control layout
        /// </summary>
        private void UpdateLayout()
        {
            foreach (PrintOutputGroupControl outputGroupControl in panelMain.Controls)
            {
                outputGroupControl.UpdateDisplay();
            }

            panelMain.Height = panelMain.Controls.Count == 0 ? 0 : panelMain.Controls.OfType<Control>().Max(c => c.Bottom);
            toolStripAddPrintOutput.Top = panelMain.Bottom;
        }

        /// <summary>
        /// Save any unchanged settings
        /// </summary>
        public void SaveSettings()
        {
            foreach (PrintOutputGroupControl outputGroupControl in panelMain.Controls)
            {
                outputGroupControl.SaveSettings();
            }

            // Print action enabled
            actionPrint.Enabled = printActionBox.Checked;
            if (actionPrint.IsDirty)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(actionPrint);
                }

                ActionManager.CheckForChangesNeeded();
            }
        }

        /// <summary>
        /// Gets whether any of the rules use disabled filters
        /// </summary>
        public bool AreAnyRuleFiltersDisabled
        {
            get
            {
                return panelMain.Controls.OfType<PrintOutputGroupControl>().Any(x => x.AreAnyRuleFiltersDisabled);
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are any rule filters that have changed.
        /// </summary>
        public bool AreAnyRuleFiltersChanged
        {
            get
            {
                return panelMain.Controls.OfType<PrintOutputGroupControl>().Any(r => r.AreAnyRuleFiltersChanged);
            }
        }

        /// <summary>
        /// Return all the template id's that the user has chosen to be used as templaets to print with
        /// </summary>
        IEnumerable<long> IPrintWithTemplates.TemplatesToPrintWith
        {
            // Reverse is to get them in screen top-down order - since z-ordering has them in the opposite order then are on screen
            get { return panelMain.Controls.OfType<IPrintWithTemplates>().Reverse().SelectMany(i => i.TemplatesToPrintWith); }
        }

        /// <summary>
        /// Install the missing default output groups
        /// </summary>
        private void OnInstallMissingClick(object sender, EventArgs e)
        {
            ShipmentPrintHelper.InstallDefaultRules(shipmentType.ShipmentTypeCode, true, this);
            OnReloadRequired(null, EventArgs.Empty);
        }
    }
}
