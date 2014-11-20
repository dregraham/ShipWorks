﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Settings.Defaults
{
    /// <summary>
    /// Control for editing shipping defaults
    /// </summary>
    public partial class ShippingDefaultsControl : UserControl
    {
        ShipmentType shipmentType;

        /// <summary>
        /// A profile has been edited in some way
        /// </summary>
        public event EventHandler ProfileEdited;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDefaultsControl()
        {
            InitializeComponent();

            toolStripAddSettingsLine.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// Gets a value indicating whether [are any rule filters disabled].
        /// </summary>
        public bool AreAnyRuleFiltersDisabled
        {
            get
            {
                return panelSettingsArea.Controls.OfType<ShippingDefaultsRuleControl>().Any(x => x.IsFilterDisabled);
            }
        }

        /// <summary>
        /// Load the settings for the given shipment type
        /// </summary>
        public void LoadSettings(ShipmentType shipmentType)
        {
            this.shipmentType = shipmentType;

            labelPrimaryProfileInfo.Text = string.Format(labelPrimaryProfileInfo.Text, shipmentType.ShipmentTypeName);
            linkCommonProfile.Left = labelPrimaryProfileInfo.Right;

            linkCommonProfile.Text = shipmentType.GetPrimaryProfile().Name;

            LoadRules();

            UpdateLayout();
        }

        /// <summary>
        /// Load all the rules that already exist
        /// </summary>
        private void LoadRules()
        {
            foreach (ShippingDefaultsRuleEntity rule in ShippingDefaultsRuleManager.GetRules(shipmentType.ShipmentTypeCode))
            {
                AddRuleControl(rule);
            }
        }

        /// <summary>
        /// Add a control for editing the given rule
        /// </summary>
        private void AddRuleControl(ShippingDefaultsRuleEntity rule)
        {
            ShippingDefaultsRuleControl ruleControl = new ShippingDefaultsRuleControl();
            ruleControl.Width = panelSettingsArea.Width;
            ruleControl.Dock = DockStyle.Top;

            ruleControl.Initialize(rule);
            ruleControl.DeleteClicked += new EventHandler(OnDeleteRule);
            ruleControl.MoveUpClicked += new EventHandler(OnMoveUpRule);
            ruleControl.MoveDownClicked += new EventHandler(OnMoveDownRule);
            ruleControl.MangeProfilesClicked += new EventHandler(OnManageProfiles);

            panelSettingsArea.Controls.Add(ruleControl);
            panelSettingsArea.Controls.SetChildIndex(ruleControl, 0);
        }

        /// <summary>
        /// Open the editor for the default profile
        /// </summary>
        private void OnLinkDefaultProfile(object sender, EventArgs e)
        {
            using (ShippingProfileEditorDlg dlg = new ShippingProfileEditorDlg(shipmentType.GetPrimaryProfile()))
            {
                dlg.ShowDialog(this);
            }

            if (ProfileEdited != null)
            {
                ProfileEdited(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Open the profile manager
        /// </summary>
        void OnManageProfiles(object sender, EventArgs e)
        {
            using (ShippingProfileManagerDlg dlg = new ShippingProfileManagerDlg(shipmentType.ShipmentTypeCode))
            {
                dlg.ShowDialog(this);
            }

            if (ProfileEdited != null)
            {
                ProfileEdited(this, EventArgs.Empty);
            }

            UpdateLayout();
        }

        /// <summary>
        /// Add a new defaults rule
        /// </summary>
        private void OnAddDefaultsRule(object sender, EventArgs e)
        {
            ShippingDefaultsRuleEntity rule = new ShippingDefaultsRuleEntity();
            rule.ShipmentType = (int) shipmentType.ShipmentTypeCode;
            rule.FilterNodeID = 0;
            rule.ShippingProfileID = 0;
            rule.Position = 999;

            ShippingDefaultsRuleManager.SaveRule(rule);

            AddRuleControl(rule);

            UpdateLayout();
        }

        /// <summary>
        /// Delete the rule that generated the event
        /// </summary>
        void OnDeleteRule(object sender, EventArgs e)
        {
            ShippingDefaultsRuleControl ruleControl = (ShippingDefaultsRuleControl) sender;
            ShippingDefaultsRuleEntity rule = ruleControl.Rule;

            ShippingDefaultsRuleManager.DeleteRule(rule);

            ruleControl.DeleteClicked -= this.OnDeleteRule;
            ruleControl.MoveUpClicked -= new EventHandler(OnMoveUpRule);
            ruleControl.MoveDownClicked -= new EventHandler(OnMoveDownRule);
            ruleControl.MangeProfilesClicked -= this.OnManageProfiles;

            panelSettingsArea.Controls.Remove(ruleControl);

            UpdateLayout();
        }

        /// <summary>
        /// Move the given rule down
        /// </summary>
        void OnMoveDownRule(object sender, EventArgs e)
        {
            ShippingDefaultsRuleControl ruleControl = (ShippingDefaultsRuleControl) sender;
            int index = panelSettingsArea.Controls.IndexOf(ruleControl);

            if (index > 0)
            {
                panelSettingsArea.Controls.SetChildIndex(ruleControl, index - 1);

                UpdateLayout();
            }
        }

        /// <summary>
        /// Move the given rule up
        /// </summary>
        void OnMoveUpRule(object sender, EventArgs e)
        {
            ShippingDefaultsRuleControl ruleControl = (ShippingDefaultsRuleControl)sender;
            int index = panelSettingsArea.Controls.IndexOf(ruleControl);

            if (index < panelSettingsArea.Controls.Count - 1)
            {
                panelSettingsArea.Controls.SetChildIndex(ruleControl, index + 1);

                UpdateLayout();
            }
        }

        /// <summary>
        /// Update control layout
        /// </summary>
        private void UpdateLayout()
        {
            foreach (ShippingDefaultsRuleControl ruleControl in panelSettingsArea.Controls)
            {
                ruleControl.UpdatePosition(panelSettingsArea.Controls.Count - panelSettingsArea.Controls.IndexOf(ruleControl), panelSettingsArea.Controls.Count);
            }

            panelSettingsArea.Height = panelSettingsArea.Controls.Count == 0 ? 0 : panelSettingsArea.Controls.OfType<Control>().Max(c => c.Bottom);
            toolStripAddSettingsLine.Top = panelSettingsArea.Bottom;
        }

        /// <summary>
        /// Save any changes that have not been saved so far
        /// </summary>
        public void SaveSettings()
        {
            foreach (ShippingDefaultsRuleControl ruleControl in panelSettingsArea.Controls)
            {
                ruleControl.SaveSettings();
            }
        }
    }
}
