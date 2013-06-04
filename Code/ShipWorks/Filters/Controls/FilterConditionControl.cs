using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using Microsoft.XmlDiffPatch;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content;
using Interapptive.Shared.UI;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Control for editing the condition of a filter or folder
    /// </summary>
    public partial class FilterConditionControl : UserControl
    {
        FilterEntity filter;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterConditionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI for the given filter
        /// </summary>
        public void LoadFilter(FilterEntity filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.filter = filter;

            // Load the definition editor
            if (filter.Definition == null)
            {
                conditionEditor.LoadDefinition(new FilterDefinition((FilterTarget) filter.FilterTarget));
            }
            else
            {
                conditionEditor.LoadDefinition(new FilterDefinition(filter.Definition));
            }

            // Folders
            if (filter.IsFolder)
            {
                // One of our builtin folders
                if (FilterHelper.IsBuiltin(filter))
                {
                    useFolderCondition.Checked = false;
                }
                else
                {
                    conditionEditor.ShowEditor = filter.Definition != null;
                    useFolderCondition.Checked = conditionEditor.ShowEditor;
                }
            }
            // Filters
            else
            {
                panelFolderCondition.Visible = false;
            }
        }

        /// <summary>
        /// Changing whether the selected folder uses a condition
        /// </summary>
        private void OnChangeUseFolderCondition(object sender, EventArgs e)
        {
            conditionEditor.ShowEditor = useFolderCondition.Checked;
        }

        /// <summary>
        /// Save the current state of the definition configuration to the filter
        /// </summary>
        public bool SaveDefinitionToFilter()
        {
            // If its a folder not using a condition, the definition is now null
            if (filter.IsFolder && !useFolderCondition.Checked)
            {
                if (filter.Definition != null)
                {
                    filter.Definition = null;
                }
            }
            else
            {
                // Validate and get the definition
                if (!conditionEditor.SaveDefinition())
                {
                    return false;
                }

                filter.Definition = conditionEditor.FilterDefinition.GetXml();
            }

            return true;
        }
    }
}
