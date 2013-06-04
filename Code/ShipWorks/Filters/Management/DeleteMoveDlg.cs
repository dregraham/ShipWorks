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
    public partial class DeleteMoveDlg : Form
    {
        List<string> reasons;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteMoveDlg(List<string> reasons, bool isFolder)
        {
            InitializeComponent();

            if (reasons == null)
            {
                throw new ArgumentNullException("reasons");
            }

            if (reasons.Count == 0)
            {
                throw new ArgumentException("If there aren't any to delete you don't need this window.");
            }

            this.reasons = reasons;

            label1.Text = string.Format(label1.Text, isFolder ? "folder" : "filter");
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            usages.Lines = reasons.ToArray();
        }
    }
}