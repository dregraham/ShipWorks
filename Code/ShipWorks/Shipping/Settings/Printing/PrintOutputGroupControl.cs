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
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// UserControl for displaying a single Print Output instance
    /// </summary>
    public partial class PrintOutputGroupControl : UserControl, IPrintWithTemplates
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintOutputGroupControl));

        ShippingPrintOutputEntity outputGroup;

        /// <summary>
        /// User has clicked the delete button on the rule line
        /// </summary>
        public event EventHandler DeleteClicked;

        /// <summary>
        /// The number of rules in the group has changed
        /// </summary>
        public event EventHandler RuleCountChanged;

        /// <summary>
        /// Raised when reloading of groups is required, like when a concurrency exception indicates that our local stuff is out-of-sync
        /// </summary>
        public event EventHandler ReloadRequired;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintOutputGroupControl()
        {
            InitializeComponent();

            toolStripAdd.Renderer = new NoBorderToolStripRenderer();
            toolStripDeleteRename.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// The output group that is loaded in the control
        /// </summary>
        public ShippingPrintOutputEntity OutputGroup
        {
            get { return outputGroup; }
        }

        /// <summary>
        /// Initialize the control to display the settings of the given output group
        /// </summary>
        public void Initialize(ShippingPrintOutputEntity outputGroup)
        {
            this.outputGroup = outputGroup;

            LoadRules();

            UpdateLayout();
        }

        /// <summary>
        /// Load all the rules that already exist
        /// </summary>
        private void LoadRules()
        {
            foreach (ShippingPrintOutputRuleEntity rule in outputGroup.Rules)
            {
                AddRuleControl(rule);
            }
        }

        /// <summary>
        /// Add a control for editing the given rule
        /// </summary>
        private void AddRuleControl(ShippingPrintOutputRuleEntity rule)
        {
            PrintOutputRuleControl ruleControl = new PrintOutputRuleControl();
            ruleControl.Width = panelMain.Width;
            ruleControl.Dock = DockStyle.Top;

            ruleControl.Initialize(rule);
            ruleControl.DeleteClicked += new EventHandler(OnDeleteRule);

            panelMain.Controls.Add(ruleControl);
            panelMain.Controls.SetChildIndex(ruleControl, 0);
        }

        /// <summary>
        /// Update the display of this group, which is the child at the given index within its parent
        /// </summary>
        public void UpdateDisplay()
        {
            labeName.Text = outputGroup.Name;
            toolStripDeleteRename.Left = labeName.Right;
        }

        /// <summary>
        /// Rename the output group
        /// </summary>
        private void OnRenameGroup(object sender, EventArgs e)
        {
            using (PrintOutputGroupNameDlg dlg = new PrintOutputGroupNameDlg())
            {
                dlg.PrintOutputName = outputGroup.Name;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    outputGroup.Name = dlg.PrintOutputName.Trim();

                    try
                    {
                        ShippingPrintOutputManager.SaveOutputGroup(outputGroup);

                        UpdateDisplay();
                    }
                    catch (ORMConcurrencyException ex)
                    {
                        log.Error("Renaming print output group", ex);

                        MessageHelper.ShowError(this, "The group could not be renamed.  Another user may have deleted the group.");

                        RaiseReloadRequired();
                    }
                }
            }
        }

        /// <summary>
        /// User clicked delete
        /// </summary>
        private void OnDeleteGroup(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
            {
                DeleteClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Add a new defaults rule
        /// </summary>
        private void OnAddRule(object sender, EventArgs e)
        {
            ShippingPrintOutputRuleEntity rule = new ShippingPrintOutputRuleEntity();
            rule.ShippingPrintOutput = outputGroup;
            rule.FilterNodeID = 0;
            rule.TemplateID = 0;

            try
            {
                ShippingPrintOutputManager.SaveOutputGroup(outputGroup);

                AddRuleControl(rule);

                UpdateLayout();

                if (RuleCountChanged != null)
                {
                    RuleCountChanged(this, EventArgs.Empty);
                }
            }
            catch (SqlForeignKeyException ex)
            {
                log.Error("Adding new rule", ex);

                MessageHelper.ShowError(this, "The print group has been deleted by another user.");

                RaiseReloadRequired();
            }
        }

        /// <summary>
        /// Delete the rule that generated the event
        /// </summary>
        void OnDeleteRule(object sender, EventArgs e)
        {
            PrintOutputRuleControl ruleControl = (PrintOutputRuleControl) sender;
            ShippingPrintOutputRuleEntity rule = ruleControl.Rule;

            ShippingPrintOutputManager.DeleteRule(rule);

            ruleControl.DeleteClicked -= this.OnDeleteRule;

            panelMain.Controls.Remove(ruleControl);

            UpdateLayout();

            if (RuleCountChanged != null)
            {
                RuleCountChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update control layout
        /// </summary>
        private void UpdateLayout()
        {
            foreach (PrintOutputRuleControl ruleControl in panelMain.Controls)
            {
                ruleControl.UpdateDisplay();
            }

            panelMain.Height = panelMain.Controls.Count == 0 ? 0 : panelMain.Controls.OfType<Control>().Max(c => c.Bottom);
            toolStripAdd.Top = panelMain.Bottom;

            Height = toolStripAdd.Bottom + 16;
        }

        /// <summary>
        /// Raise the ReloadRequired event, which indicates that an exception occurred that indicates a big failure that mandates
        /// reloading all the groups.
        /// </summary>
        private void RaiseReloadRequired()
        {
            if (ReloadRequired != null)
            {
                ReloadRequired(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Save any unchanged settings
        /// </summary>
        public void SaveSettings()
        {
            foreach (PrintOutputRuleControl ruleControl in panelMain.Controls)
            {
                ruleControl.SaveSettingsToEntity();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    try
                    {
                        ShippingPrintOutputManager.SaveOutputGroup(outputGroup, adapter);
                    }
                    catch (ORMConcurrencyException ex)
                    {
                        log.Error("Saving output group", ex);

                        // Skip this error.  It means that a group we are trying to save couldn't be saved b\c it was
                        // deleted somewhere else.  If that happens, for these settings, we just let it go.
                        ShippingPrintOutputManager.CheckForChangesNeeded();
                    }

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Gets whether any of the rules use disabled filters
        /// </summary>
        public bool AreAnyRuleFiltersDisabled
        {
            get
            {
                return panelMain.Controls.OfType<PrintOutputRuleControl>().Any(x => x.IsFilterDisabled);
            }
        }

        /// <summary>
        /// Gets whether any of the rules' filters have changed
        /// </summary>
        public bool AreAnyRuleFiltersChanged
        {
            get
            {
                return panelMain.Controls.OfType<PrintOutputRuleControl>().Any(r => r.HasFilterChanged);
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
    }
}
