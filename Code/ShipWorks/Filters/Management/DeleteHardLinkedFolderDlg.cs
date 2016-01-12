using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Linq;
using System.Data.SqlClient;
using Interapptive.Shared;
using ShipWorks.Data.Connection;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for confirming deletion of a hard-linked folder
    /// </summary>
    public partial class DeleteHardLinkedFolderDlg : Form
    {
        FilterNodeEntity folderNode;

        bool deleteAllLinks;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteHardLinkedFolderDlg(FilterNodeEntity folderNode)
        {
            InitializeComponent();

            if (folderNode == null)
            {
                throw new ArgumentNullException("folderNode");
            }

            if (!folderNode.Filter.IsFolder)
            {
                throw new ArgumentException("It is not a folder.", "folderNode");
            }

            if (!FilterHelper.IsFilterHardLinked(folderNode.Filter))
            {
                throw new ArgumentException("Not hardlinked", "folderNode");
            }

            this.folderNode = folderNode;

        }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<FilterNodeEntity> linkNodesToDelete = new List<FilterNodeEntity>();
            List<FilterNodeEntity> otherNodesToDelete = new List<FilterNodeEntity>();

            // Generate a list of all nodes to be deleted, if only the link is deleted
            foreach (FilterNodeEntity node in FilterHelper.GetNodesUsingSequence(folderNode.FilterSequence))
            {
                linkNodesToDelete.AddRange(FilterHelper.GetNodeAndDescendants(node));
            }

            // Generate the list of addtional nodes to delete if the filter is deleted completely, rather than just the link
            foreach (FilterSequenceEntity sequence in FilterHelper.GetSequencesUsingFilter(folderNode.Filter))
            {
                // Don't include the one from before
                if (sequence.FilterSequenceID == folderNode.FilterSequenceID)
                {
                    continue;
                }

                foreach (FilterNodeEntity node in FilterHelper.GetNodesUsingSequence(sequence))
                {
                    otherNodesToDelete.AddRange(FilterHelper.GetNodeAndDescendants(node));
                }
            }

            List<string> linkReasons = ObjectReferenceManager.GetReferenceReasons(linkNodesToDelete.Select(n => n.FilterNodeID).ToList());
            List<string> otherReasons = ObjectReferenceManager.GetReferenceReasons(otherNodesToDelete.Select(n => n.FilterNodeID).ToList());

            otherReasons = otherReasons.Except(linkReasons).ToList();

            if (linkReasons.Count == 0 && otherReasons.Count == 0)
            {
                panelUsages.Visible = false;
                Height -= panelUsages.Height;
            }
            else
            {
                if (otherReasons.Count > 0)
                {
                    if (linkReasons.Count > 0)
                    {
                        linkReasons.Add("");
                        linkReasons.Add("Additionally affected when choosing 'Delete Filter Completely':");
                    }
                    else
                    {
                        linkReasons.Add("Only affected when choosing 'Delete Filter Completely':");
                    }

                    linkReasons.AddRange(otherReasons);
                }

                usages.Lines = linkReasons.ToArray();
            }
        }

        /// <summary>
        /// Indiciates if the user chose to delete all links, or only the selected
        /// </summary>
        public bool DeleteAllLinks
        {
            get { return deleteAllLinks; }
        }

        /// <summary>
        /// Delete the selected link only
        /// </summary>
        private void OnDeleteSelected(object sender, EventArgs e)
        {
            deleteAllLinks = false;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Delete the entire filter
        /// </summary>
        private void OnDeleteAll(object sender, EventArgs e)
        {
            deleteAllLinks = true;
            DialogResult = DialogResult.OK;
        }
    }
}