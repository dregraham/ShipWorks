using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Core.Messaging;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messages;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// User control for configuring rules on which shipping provider to use
    /// </summary>
    public partial class ShippingProviderControl : UserControl
    {
        List<ShipmentType> activeShipmentTypes;
        private MessengerToken carrierConfiguredToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProviderControl()
        {
            InitializeComponent();

            toolStripFakeDelete.Renderer = new NoBorderToolStripRenderer();
            toolStripAddRule.Renderer = new NoBorderToolStripRenderer();

            carrierConfiguredToken = Messenger.Current.Handle<CarrierConfiguredMessage>(this, HandleCarrierConfigured);
        }

        private void HandleCarrierConfigured(CarrierConfiguredMessage message)
        {
            SetDefaultShipmentType();
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public void LoadSettings(List<ShipmentType> shipmentTypes)
        {
            shipmentTypeCombo.DisplayMember = "Key";
            shipmentTypeCombo.ValueMember = "Value";

            UpdateActiveProviders(shipmentTypes);

            SetDefaultShipmentType();

            LoadRules();

            UpdateLayout();
        }

        private void SetDefaultShipmentType()
        {
            shipmentTypeCombo.SelectedValue = (ShipmentTypeCode) ShippingSettings.Fetch().DefaultType;
            if (shipmentTypeCombo.SelectedIndex < 0)
            {
                shipmentTypeCombo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Update the shipment types available in the provider comboboxes
        /// </summary>
        public void UpdateActiveProviders(List<ShipmentType> shipmentTypes)
        {
            activeShipmentTypes = shipmentTypes;

            ShipmentTypeCode? selected = shipmentTypeCombo.SelectedValue != null ? (ShipmentTypeCode) shipmentTypeCombo.SelectedValue : (ShipmentTypeCode?) null;

            shipmentTypeCombo.DataSource = shipmentTypes.Select(t => new KeyValuePair<string, ShipmentTypeCode>(t.ShipmentTypeName, t.ShipmentTypeCode)).ToList();

            if (selected != null)
            {
                shipmentTypeCombo.SelectedValue = selected.Value;
            }

            if (shipmentTypeCombo.SelectedIndex < 0)
            {
                shipmentTypeCombo.SelectedIndex = 0;
            }

            foreach (ShippingProviderRuleControl ruleControl in panelMain.Controls)
            {
                ruleControl.UpdateActiveProviders(shipmentTypes);
            }
        }

        /// <summary>
        /// Load all the rules that already exist
        /// </summary>
        private void LoadRules()
        {
            foreach (ShippingProviderRuleEntity rule in ShippingProviderRuleManager.GetRules())
            {
                AddRuleControl(rule);
            }
        }

        /// <summary>
        /// Add a control for editing the given rule
        /// </summary>
        private void AddRuleControl(ShippingProviderRuleEntity rule)
        {
            ShippingProviderRuleControl ruleControl = new ShippingProviderRuleControl();
            ruleControl.Width = panelMain.Width;
            ruleControl.Dock = DockStyle.Top;

            ruleControl.Initialize(rule, activeShipmentTypes);
            ruleControl.DeleteClicked += new EventHandler(OnDeleteRule);

            panelMain.Controls.Add(ruleControl);
            panelMain.Controls.SetChildIndex(ruleControl, 0);
        }

        /// <summary>
        /// Add a new rule
        /// </summary>
        private void OnAddRule(object sender, EventArgs e)
        {
            ShippingProviderRuleEntity rule = new ShippingProviderRuleEntity();
            rule.FilterNodeID = 0;
            rule.ShipmentType = (int) ShipmentTypeCode.None;

            ShippingProviderRuleManager.SaveRule(rule);

            AddRuleControl(rule);

            UpdateLayout();
        }

        /// <summary>
        /// Delete the rule that generated the event
        /// </summary>
        void OnDeleteRule(object sender, EventArgs e)
        {
            ShippingProviderRuleControl ruleControl = (ShippingProviderRuleControl) sender;
            ShippingProviderRuleEntity rule = ruleControl.Rule;

            ShippingProviderRuleManager.DeleteRule(rule);

            ruleControl.DeleteClicked -= this.OnDeleteRule;

            panelMain.Controls.Remove(ruleControl);

            UpdateLayout();
        }

        /// <summary>
        /// Update control layout
        /// </summary>
        private void UpdateLayout()
        {
            labelDefaultType.Text = (panelMain.Controls.Count == 0) ? "Use provider" : "Otherwise use";
            shipmentTypeCombo.Left = labelDefaultType.Right;

            foreach (ShippingProviderRuleControl ruleControl in panelMain.Controls)
            {
                ruleControl.UpdateDisplay(panelMain.Controls.Count - panelMain.Controls.IndexOf(ruleControl));
            }

            panelMain.Height = panelMain.Controls.Count == 0 ? 0 : panelMain.Controls.OfType<Control>().Max(c => c.Bottom);
            panelBottom.Top = panelMain.Bottom;

            Height = panelBottom.Bottom;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.DefaultType = (int) (ShipmentTypeCode) shipmentTypeCombo.SelectedValue;

            foreach (ShippingProviderRuleControl ruleControl in panelMain.Controls)
            {
                ruleControl.SaveSettings();
            }
        }

        /// <summary>
        /// Gets whether any rule filters are disabled
        /// </summary>
        public bool AreAnyRuleFiltersDisabled
        {
            get
            {
                return panelMain.Controls.OfType<ShippingProviderRuleControl>().Any(x => x.IsFilterDisabled);
            }
        }
    }
}
