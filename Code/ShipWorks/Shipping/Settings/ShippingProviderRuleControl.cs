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
using ShipWorks.Filters;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// An individual provider rule
    /// </summary>
    public partial class ShippingProviderRuleControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingProviderRuleControl));

        ShippingProviderRuleEntity rule;
        private long originalFilterNodeID;

        /// <summary>
        /// User has clicked the delete button on the rule line
        /// </summary>
        public event EventHandler DeleteClicked;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProviderRuleControl()
        {
            InitializeComponent();

            toolStipDelete.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// The rule that is loaded and being edited by the control
        /// </summary>
        public ShippingProviderRuleEntity Rule
        {
            get { return rule; }
        }

        /// <summary>
        /// Initialize the control to display settings for the given rule
        /// </summary>
        public void Initialize(ShippingProviderRuleEntity rule, List<ShipmentType> shipmentTypes)
        {
            this.rule = rule;

            shipmentTypeCombo.DisplayMember = "Key";
            shipmentTypeCombo.ValueMember = "Value";

            filterCombo.LoadLayouts(FilterTarget.Orders);

            originalFilterNodeID = rule.FilterNodeID;
            filterCombo.SelectedFilterNodeID = originalFilterNodeID;
            if (filterCombo.SelectedFilterNode == null)
            {
                filterCombo.SelectFirstNode();
            }

            UpdateActiveProviders(shipmentTypes);

            shipmentTypeCombo.SelectedValue = (ShipmentTypeCode) rule.ShipmentType;
            if (shipmentTypeCombo.SelectedIndex < 0)
            {
                shipmentTypeCombo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Update the display of the control based on the index within the parent
        /// </summary>
        public void UpdateDisplay(int index)
        {
            string text = (index == 1) ?
                "If the order is in" :
                "Otherwise if the order is in";

            labelOrder.Text = text;

            UpdateLayout();
        }

        /// <summary>
        /// The filter combo size changed
        /// </summary>
        private void OnFilterComboSizeChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        /// <summary>
        /// Update the layout and spacing of the controls
        /// </summary>
        private void UpdateLayout()
        {
            filterCombo.Left = labelOrder.Right;
            labelUse.Left = filterCombo.Right + 2;
            shipmentTypeCombo.Left = labelUse.Right;
        }

        /// <summary>
        /// User clicked the delete button
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
            {
                DeleteClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the list of available shipment types to choose from
        /// </summary>
        public void UpdateActiveProviders(List<ShipmentType> shipmentTypes)
        {
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
        }

        /// <summary>
        /// Save the settings in the control
        /// </summary>
        public void SaveSettings()
        {
            rule.FilterNodeID = filterCombo.SelectedFilterNodeID;
            rule.ShipmentType = (int) shipmentTypeCombo.SelectedValue;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                try
                {
                    ShippingProviderRuleManager.SaveRule(rule, adapter);
                    originalFilterNodeID = rule.FilterNodeID;
                }
                catch (ORMConcurrencyException ex)
                {
                    log.Error("Saving output group", ex);

                    // Skip this error.  It means that a group we are trying to save couldn't be saved b\c it was
                    // deleted somewhere else.  If that happens, for these settings, we just let it go.
                    ShippingProviderRuleManager.CheckForChangesNeeded();
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Gets whether the selected filter is disabled
        /// </summary>
        public bool IsFilterDisabled
        {
            get
            {
                return filterCombo.SelectedFilterNode != null &&
                    filterCombo.SelectedFilterNodeID != originalFilterNodeID &&
                    filterCombo.SelectedFilterNode.Filter.State != (int) FilterState.Enabled;
            }
        }
    }
}
