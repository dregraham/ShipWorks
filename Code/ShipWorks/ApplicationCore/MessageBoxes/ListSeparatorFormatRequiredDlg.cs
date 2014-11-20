using System;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window for displaying a message to the user that US-English date format is required
    /// </summary>
    public partial class ListSeparatorFormatRequiredDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ListSeparatorFormatRequiredDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clicked the support link
        /// </summary>
        private void OnClickSupportForum(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/support", this);
        }
    }
}
