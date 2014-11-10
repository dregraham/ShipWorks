using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Confirmation of disabling filter. Lists anything the filter migt be associated with.
    /// </summary>
    public partial class DisableLinkedFilterDlg : Form
    {
        private readonly FilterNodeEntity filterNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public DisableLinkedFilterDlg(FilterNodeEntity filterNode)
        {
            InitializeComponent();

            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            if (filterNode.Filter.IsFolder)
            {
                throw new ArgumentException(@"It is a folder.", "filterNode");
            }

            this.filterNode = filterNode;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<string> linkReasons = new FilterNodeReferenceRepository().Find(filterNode);

            if (!linkReasons.Any())
            {
                panelUsages.Visible = false;
                Height -= panelUsages.Height;
            }
            else
            {
                usages.Lines = linkReasons.ToArray();
            }
        }

        

        /// <summary>
        /// Disable Selected
        /// </summary>
        private void OnDisableSelected(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}