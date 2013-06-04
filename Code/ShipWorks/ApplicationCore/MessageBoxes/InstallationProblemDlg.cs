using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window for displaying InstallatioException information to the user
    /// </summary>
    public partial class InstallationProblemDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InstallationProblemDlg(string errorMessage)
        {
            InitializeComponent();

            labelProblem.Text = errorMessage;
        }

        /// <summary>
        /// Clicked the support link
        /// </summary>
        private void OnSupportLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/support", this);
        }
    }
}
