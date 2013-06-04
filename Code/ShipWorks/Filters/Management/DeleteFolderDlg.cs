using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for confirming deletion of a soft linked folder
    /// </summary>
    public partial class DeleteFolderDlg : Form
    {
        FilterEntity folder;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteFolderDlg(FilterEntity folder)
        {
            InitializeComponent();

            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            this.folder = folder;

            if (!folder.IsFolder)
            {
                throw new InvalidOperationException("folder must represent a folder.");
            }

            if (FilterHelper.IsFilterHardLinked(folder))
            {
                throw new InvalidOperationException("This window is only for folder's that are not linked.");
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<FilterNodeEntity> nodesToDelete = new List<FilterNodeEntity>();

            // Generate a list of all nodes to be deleted
            foreach (FilterNodeEntity node in FilterHelper.GetNodesUsingFilter(folder))
            {
                nodesToDelete.AddRange(FilterHelper.GetNodeAndDescendants(node));
            }

            List<string> reasons = ObjectReferenceManager.GetReferenceReasons(nodesToDelete.Select(n => n.FilterNodeID).ToList());

            if (reasons.Count == 0)
            {
                panelUsages.Visible = false;
                Height -= panelUsages.Height;
            }
            else
            {
                usages.Lines = reasons.ToArray();

                // See if it should be singular
                if (nodesToDelete.Count == 1)
                {
                    labelInUse.Text = "The item being deleted is used by:";
                }
            }
        }
    }
}