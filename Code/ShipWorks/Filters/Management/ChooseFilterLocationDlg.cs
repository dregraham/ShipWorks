using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using ShipWorks.Filters.Management;
using ShipWorks.Filters.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for allowing a user to select where a filter is going to go
    /// </summary>
    public partial class ChooseFilterLocationDlg : Form
    {
        // Event raised when the user is saving their selection
        public event CancelEventHandler Saving;

        // The node that's having its location chosen
        FilterNodeEntity subjectNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChooseFilterLocationDlg(string action, FilterNodeEntity subjectNode, FolderExpansionState initialState)
        {
            InitializeComponent();

            if (subjectNode == null)
            {
                throw new ArgumentNullException("subjectNode");
            }

            if (initialState == null)
            {
                throw new ArgumentNullException("initialState");
            }

            this.subjectNode = subjectNode;

            labelInstructions.Text = string.Format(labelInstructions.Text, action, subjectNode.Filter.IsFolder ? "folder" : "filter");
            nodeIcon.Image = FilterHelper.GetFilterImage(subjectNode);
            labelNodeName.Text = subjectNode.Filter.Name;

            this.Text = string.Format("{0} {1}", action, subjectNode.Filter.IsFolder ? "Folder" : "Filter");

            // Limit to my filters if permissions not enabled
            if (!UserSession.Security.HasPermission(PermissionType.ManageFilters))
            {
                filterTree.FilterScope = FilterScope.MyFilters;
            }

            filterTree.LoadLayouts((FilterTarget) subjectNode.Filter.FilterTarget);
            filterTree.ApplyFolderState(initialState);

            // Select the root
            filterTree.SelectFirstNode();
        }

        /// <summary>
        /// The node that is the reason this window is open
        /// </summary>
        public FilterNodeEntity SubjectNode
        {
            get
            {
                return subjectNode;
            }
        }

        /// <summary>
        /// The folder that was selected by the user
        /// </summary>
        public FilterNodeEntity SelectedFolder
        {
            get
            {
                return filterTree.SelectedFilterNode;
            }
            set
            {
                filterTree.SelectedFilterNode = value;
            }
        }

        /// <summary>
        /// User has ok'd the close of the window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (filterTree.SelectedFilterNode == null)
            {
                MessageHelper.ShowMessage(this, "Please select a folder.");
                return;
            }

            if (Saving != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                Saving(this, args);

                // Cancel the closing of the window
                if (args.Cancel)
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}