using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Window for allowing the user to enter a custom comment for updating an online status
    /// </summary>
    public partial class NetworkSolutionsOnlineStatusCommentDlg : Form
    {
        NetworkSolutionsStatusCodeProvider statusCodes;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOnlineStatusCommentDlg(NetworkSolutionsStatusCodeProvider statusCodes)
        {
            InitializeComponent();

            this.statusCodes = statusCodes;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            status.DisplayMember = "Display";
            status.ValueMember = "Code";
            status.DataSource = statusCodes.CodeValues.Select(c => new { Display = statusCodes[c], Code = c }).ToList();

            if (status.Items.Count > 0)
            {
                status.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// The code the user has selected
        /// </summary>
        public long Code
        {
            get
            {
                if (status.SelectedIndex >= 0)
                {
                    return (long)status.SelectedValue;
                }

                return 0;
            }
        }

        /// <summary>
        /// The user entered comments
        /// </summary>
        public string Comments
        {
            get
            {
                return comment.Text;
            }
        }
    }
}
