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
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// UserControl representing an individual rule line within a print output group
    /// </summary>
    public partial class PrintOutputRuleControl : UserControl, IPrintWithTemplates
    {
        private const long NoFilterSelectedID = long.MinValue;

        ShippingPrintOutputRuleEntity rule;
        private long originalFilterID;

        /// <summary>
        /// The delete button on this rule line has been clicked
        /// </summary>
        public event EventHandler DeleteClicked;

        // Indicates if always is selected, vs. a filter condition
        bool alwaysExecute = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintOutputRuleControl()
        {
            InitializeComponent();

            toolStripDelete.Renderer = new NoBorderToolStripRenderer();
            originalFilterID = NoFilterSelectedID;
        }

        /// <summary>
        /// Initialized the print output with the given rule 
        /// </summary>
        public void Initialize(ShippingPrintOutputRuleEntity rule)
        {
            this.rule = rule;

            filterCombo.LoadLayouts(FilterTarget.Shipments);
            templateCombo.LoadTemplates();

            filterCombo.SelectedFilterNodeID = rule.FilterNodeID;
            
            if (filterCombo.SelectedFilterNode == null)
            {
                filterCombo.SelectFirstNode();
            }
            else
            {
                originalFilterID = filterCombo.SelectedFilterNode.FilterID;
            }
            
            alwaysExecute = rule.FilterNodeID == ShippingPrintOutputManager.FilterNodeAlwaysID;

            templateCombo.SelectedTemplateID = rule.TemplateID;
            if (templateCombo.SelectedTemplate == null)
            {
                templateCombo.SelectFirstTemplate();
            }
        }

        /// <summary>
        /// The rule that is loaded
        /// </summary>
        public ShippingPrintOutputRuleEntity Rule
        {
            get { return rule; }
        }

        /// <summary>
        /// Update the display given the specified index within the parent
        /// </summary>
        public void UpdateDisplay()
        {
            int index = rule.ShippingPrintOutput.Rules.IndexOf(rule);
            bool isLast = index == rule.ShippingPrintOutput.Rules.Count - 1;

            // If there are no more then they can choose a condition, or always
            if (isLast)
            {
                labelShipment.Visible = (index > 0);
                labelShipment.Text = "Otherwise";

                linkAlwaysOption.Visible = true;
                linkAlwaysOption.Text = GetAlwaysOptionText(alwaysExecute);
                linkAlwaysOption.Left = (index > 0) ? labelShipment.Right - 4 : labelShipment.Left;

                filterCombo.Visible = !alwaysExecute;
                filterCombo.Left = linkAlwaysOption.Right + 2;

                labelPrint.Left = alwaysExecute ? linkAlwaysOption.Right - 3 : filterCombo.Right + 2;

                templateCombo.Left = labelPrint.Right;
            }
            // Its the first one, and there are others after it
            else
            {
                labelShipment.Visible = true;
                labelShipment.Text = (index == 0) ? "If the shipment is in" : "Otherwise if the shipment is in";

                linkAlwaysOption.Visible = false;
                alwaysExecute = false;

                filterCombo.Visible = true;
                filterCombo.Left = labelShipment.Right;

                labelPrint.Left = filterCombo.Right + 2;

                templateCombo.Left = labelPrint.Right;
            }
        }

        /// <summary>
        /// Clicking to change the selected "Always" option
        /// </summary>
        private void OnClickAlwaysOption(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(GetAlwaysOptionText(false), null, OnChangeAlwaysOption).Tag = false;
            contextMenu.Items.Add(GetAlwaysOptionText(true), null, OnChangeAlwaysOption).Tag = true;

            contextMenu.Show(linkAlwaysOption.Parent.PointToScreen(new Point(linkAlwaysOption.Left, linkAlwaysOption.Bottom)));
        }

        /// <summary>
        /// Change the selected "Always" option
        /// </summary>
        private void OnChangeAlwaysOption(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            bool option = (bool) item.Tag;

            alwaysExecute = option;

            UpdateDisplay();
        }

        /// <summary>
        /// Get the text to display for the "Always" option
        /// </summary>
        private string GetAlwaysOptionText(bool option)
        {
            return option ? "Always" : "If the shipment is in"; 
        }

        /// <summary>
        /// The size of the filter combo has changed
        /// </summary>
        private void OnFilterComboSizeChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        /// <summary>
        /// Delete has been clicked, notify any listeners
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
            {
                DeleteClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Save any unchanged settings
        /// </summary>
        public void SaveSettingsToEntity()
        {
            rule.FilterNodeID = alwaysExecute ? ShippingPrintOutputManager.FilterNodeAlwaysID : filterCombo.SelectedFilterNodeID;
            rule.TemplateID = templateCombo.SelectedTemplateID ?? 0;

            // Sync up the original filter ID with the saved filter ID to prevent a 
            // false positive in the HasFilterChanged property.
            originalFilterID = SelectedFilterNodeId;
        }

        /// <summary>
        /// Gets whether the selected filter is disabled
        /// </summary>
        public bool IsFilterDisabled
        {
            get
            {
                return filterCombo.IsSelectedFilterDisabled;
            }
        }

        /// <summary> 
        /// Gets a value indicating whether this the filter being used for this rule has changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the filter filter changed; otherwise, <c>false</c>.
        /// </value>
        public bool HasFilterChanged
        {
            get
            {
                return originalFilterID != SelectedFilterNodeId;
            }
        }

        /// <summary>
        /// Return all the template id's that the user has chosen to be used as templaets to print with
        /// </summary>
        IEnumerable<long> IPrintWithTemplates.TemplatesToPrintWith
        {
            get
            {
                return new long[] { templateCombo.SelectedTemplateID ?? 0 };
            }
        }

        /// <summary>
        /// Gets the id of the currently selected filter
        /// </summary>
        private long SelectedFilterNodeId
        {
            get
            {
                return filterCombo.SelectedFilterNode != null ? 
                    filterCombo.SelectedFilterNode.FilterID : 
                    NoFilterSelectedID;
            }
        }
    }
}
