using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.UI.Utility;
using ShipWorks.Users;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// An individual provider rule
    /// </summary>
    public partial class ShippingProviderRuleControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingProviderRuleControl));
        private readonly long noFilterSelectedID = BuiltinFilter.GetTopLevelKey(FilterTarget.Orders);
        private ShippingProviderRuleEntity rule;
        private long originalFilterNodeID;
        private readonly IShippingProviderRuleManager shippingProviderRuleManager;
        private long originalFilterID;

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

            filterCombo.AllowMyFilters = false;

            toolStipDelete.Renderer = new NoBorderToolStripRenderer();
            originalFilterID = noFilterSelectedID;
            originalFilterNodeID = 0;
            shippingProviderRuleManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingProviderRuleManager>();
        }

        /// <summary>
        /// The rule that is loaded and being edited by the control
        /// </summary>
        public ShippingProviderRuleEntity Rule
        {
            get { return rule; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a my filter.
        /// </summary>
        public bool IsMyFilter => filterCombo.IsSelectedFilterMyFilter;

        /// <summary>
        /// Gets the selected filter name
        /// </summary>
        public string SelectedFilterName => filterCombo.SelectedFilterNode?.Filter?.Name;
		
        /// <summary>
        /// Initialize the control to display settings for the given rule
        /// </summary>
        public void Initialize(ShippingProviderRuleEntity rule, List<ShipmentType> shipmentTypes)
        {
            this.rule = rule;

            shipmentTypeCombo.DisplayMember = "Key";
            shipmentTypeCombo.ValueMember = "Value";
            filterCombo.AllowMyFilters = Rule.FilterNodeID != 0 && FilterHelper.IsMyFilter(Rule.FilterNodeID);

            filterCombo.LoadLayouts(FilterTarget.Orders);

            originalFilterNodeID = rule.FilterNodeID;
            filterCombo.SelectedFilterNodeID = originalFilterNodeID;
            if (filterCombo.SelectedFilterNode == null)
            {
                filterCombo.SelectFirstNode();
            }
            else
            {
                originalFilterID = filterCombo.SelectedFilterNode.FilterID;
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
        /// Return a list of validation errors
        /// </summary>
        public string ValidationErrors
        {
            get
            {
                // If there was an original filter, check it to see if it belongs to the current user.  If not, return error message.
                if (originalFilterNodeID != filterCombo.SelectedFilterNodeID)
                {
                    FilterLayoutEntity originalLayout = FilterLayoutContext.Current.GetNodeLayout(originalFilterNodeID);
                    if (originalLayout == null)
                    {
                        // The original filter node was another user's, so return the account msg.
                        return $"A rule was using a My Filter that is not available to this user account. " +
                               $"Shipping Provider Rules can no longer use a My filter.  " +
                               $"This rule has been changed to use the \"None\" provider.";
                    }
                }

                FilterLayoutEntity layout = FilterLayoutContext.Current.GetNodeLayout(filterCombo.SelectedFilterNodeID);
                if (layout?.UserID != null)
                {
                    // If the selected filter has a user ID that is NOT the current user.
                    if (layout.UserID != UserSession.User.UserID)
                    {
                        return $"A rule was using a My Filter that is not available to this user account. " +
                               $"Shipping Provider Rules can no longer use a My filter.  " +
                               $"This rule has been changed to use the \"None\" provider.";
                    }
                 
                    // If the selected filter has a user ID that IS the current user.
                    return $"A Rule is using a My Filter, '{filterCombo.SelectedFilterNode.Filter.Name}'. Shipping Provider Rules can no longer use a My Filter. Any changes to this rule will not be saved.";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Save the settings in the control
        /// </summary>
        public void SaveSettings()
        {
            // If there was an original filter, check it to see if it belongs to the current user.  If not, change to All and None provider.
            if (originalFilterNodeID != filterCombo.SelectedFilterNodeID)
            {
                FilterLayoutEntity originalLayout = FilterLayoutContext.Current.GetNodeLayout(originalFilterNodeID);
                if (originalLayout == null)
                {
                    SaveRule(noFilterSelectedID, (int) ShipmentTypeCode.None);
                    return;
                }
            }

            FilterLayoutEntity layout = FilterLayoutContext.Current.GetNodeLayout(filterCombo.SelectedFilterNode);
            if (layout?.UserID != null)
            {
                // If the selected filter has a user ID that is NOT the current user, don't save.
                if (layout.UserID != UserSession.User.UserID)
                {
                    SaveRule(noFilterSelectedID, (int) ShipmentTypeCode.None);
                }

                // If the selected filter has a user ID that IS the current user, don't save, just return.
                return;
            }
         
            // If the selected filter does not have a user ID, save it.
            SaveRule(filterCombo.SelectedFilterNodeID, (int) shipmentTypeCombo.SelectedValue);
        }

        /// <summary>
        /// Save the shipping rule with the given shipping profile id
        /// </summary>
        private void SaveRule(long filterNodeID, int shipmentType)
        {
            rule.FilterNodeID = filterNodeID;
            rule.ShipmentType = shipmentType;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                try
                {
                    shippingProviderRuleManager.SaveRule(rule, adapter);
                    originalFilterNodeID = rule.FilterNodeID;
                }
                catch (ORMConcurrencyException ex)
                {
                    log.Error("Saving output group", ex);

                    // Skip this error.  It means that a group we are trying to save couldn't be saved b\c it was
                    // deleted somewhere else.  If that happens, for these settings, we just let it go.
                    shippingProviderRuleManager.CheckForChangesNeeded();
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
