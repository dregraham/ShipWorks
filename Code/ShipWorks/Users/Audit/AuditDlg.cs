using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Window for viewing audits for the entire system
    /// </summary>
    public partial class AuditDlg : Form
    {
        static Guid gridSettingsKey = new Guid("{FDAFC7C7-7461-4bca-BC54-A23B1F41BB05}");

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            auditControl.Initialize(gridSettingsKey, null);
        }
    }
}
