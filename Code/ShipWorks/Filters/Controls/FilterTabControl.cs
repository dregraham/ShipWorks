using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Tab control for displaying multiple filter trees, tabbed.
    /// </summary>
    public partial class FilterTabControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterTabControl(FilterTree orderFilterTree, FilterTree customerFilterTree)
        {
            InitializeComponent();

            filterTabs.ImageList = new ImageList();
            filterTabs.ImageList.Images.Add(global::ShipWorks.Properties.Resources.filter);
            filterTabs.ImageList.Images.Add(global::ShipWorks.Properties.Resources.customer16);

            OrderFilterTree = orderFilterTree;
            CustomerFilterTree = customerFilterTree;

            orderFiltersTab.ImageIndex = 0;
            customerFiltersTab.ImageIndex = 1;

            filterTabs.SelectTab(0);
        }

        /// <summary>
        /// The filter tree of the currently selected tab
        /// </summary>
        public FilterTree ActiveFilterTree
        {
            get
            {
                if (filterTabs.SelectedIndex == 0)
                {
                    return orderFilterTree;
                }
                else
                {
                    return customerFilterTree;
                }
            }
        }

        /// <summary>
        /// The filter tree of the currently UNselected tab
        /// </summary>
        public FilterTree InActiveFilterTree
        {
            get
            {
                if (filterTabs.SelectedIndex == 0)
                {
                    return customerFilterTree;
                }
                else
                {
                    return orderFilterTree;
                }
            }
        }

        /// <summary>
        /// Get/Set for the order filter tree to use.
        /// </summary>
        public FilterTree OrderFilterTree
        {
            get
            {
                return orderFilterTree; 
            }
            set
            {
                orderFilterTree = value;

                orderFilterTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                orderFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                orderFilterTree.HotTrackNode = null;
                orderFilterTree.Location = new System.Drawing.Point(3, 3);
                orderFilterTree.Name = "orderFilterTree";
                orderFilterTree.Dock = DockStyle.Fill;
                orderFilterTree.TabIndex = 0;


                orderFiltersTab.Controls.Clear();
                orderFiltersTab.Controls.Add(orderFilterTree);
            }
        }

        /// <summary>
        /// Get/Set for the customer filter tree to use.
        /// </summary>
        public FilterTree CustomerFilterTree
        {
            get
            {
                return customerFilterTree;
            }
            set
            {
                customerFilterTree = value;

                customerFilterTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                customerFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                customerFilterTree.HotTrackNode = null;
                customerFilterTree.Location = new System.Drawing.Point(3, 3);
                customerFilterTree.Name = "customerFilterTree";
                customerFilterTree.Dock = DockStyle.Fill;
                customerFilterTree.TabIndex = 1;

                customerFiltersTab.Controls.Clear();
                customerFiltersTab.Controls.Add(customerFilterTree);
            }
        }

        /// <summary>
        /// Select the initial filter based on the given user settings
        /// </summary>
        public void SelectInitialFilter(UserSettingsEntity settings)
        {
            if (settings.LastFilterTargetSelected == (int) FilterTarget.Orders)
            {
                orderFilterTree.SelectInitialFilter(settings);
                filterTabs.SelectTab(orderFiltersTab);
            }
            else
            {
                customerFilterTree.SelectInitialFilter(settings);
                filterTabs.SelectTab(customerFiltersTab);
            }
        }

        /// <summary>
        /// Updates the filter tree when switching between tabs.
        /// </summary>
        private void OnFilterTabSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveFilterTree.SelectedFilterNodeID == 0)
            {
                ActiveFilterTree.SelectFirstNode();
            }
            else
            {
                // Need to get the grid to update when switching to the new tab.  
                // Couldn't find a better way, but there probably is one.
                long tmpSelectedFilterNodeID = ActiveFilterTree.SelectedFilterNodeID;
                ActiveFilterTree.SelectedFilterNodeID = 0;
                ActiveFilterTree.SelectedFilterNodeID = tmpSelectedFilterNodeID; 
            }
        }
    }
}
