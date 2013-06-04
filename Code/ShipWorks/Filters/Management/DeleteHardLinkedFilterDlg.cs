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
using ShipWorks.Data.Connection;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Determines the scope of deleting a filter.  As in, if all links should be deleted, or just the selected link.
    /// </summary>
    public partial class DeleteHardLinkedFilterDlg : Form
    {
        FilterNodeEntity filterNode;

        bool deleteAllLinks;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteHardLinkedFilterDlg(FilterNodeEntity filterNode)
        {
            InitializeComponent();

            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            if (filterNode.Filter.IsFolder)
            {
                throw new ArgumentException("It is a folder.", "filterNode");
            }

            if (!FilterHelper.IsFilterHardLinked(filterNode.Filter))
            {
                throw new ArgumentException("Not hardlinked", "filterNode");
            }

            this.filterNode = filterNode;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<FilterNodeEntity> linkNodesToDelete = new List<FilterNodeEntity>();
            List<FilterNodeEntity> otherNodesToDelete = new List<FilterNodeEntity>();

            // Generate a list of all nodes to be deleted, if only the link is deleted
            linkNodesToDelete.AddRange(FilterHelper.GetNodesUsingSequence(filterNode.FilterSequence));

            // Generate the list of addtional nodes to delete if the filter is deleted completely, rather than just the link
            foreach (FilterSequenceEntity sequence in FilterHelper.GetSequencesUsingFilter(filterNode.Filter))
            {
                // Don't include the one from before
                if (sequence.FilterSequenceID == filterNode.FilterSequenceID)
                {
                    continue;
                }

                otherNodesToDelete.AddRange(FilterHelper.GetNodesUsingSequence(sequence));
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