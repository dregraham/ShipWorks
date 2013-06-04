using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.MessagesBoxes
{
    /// <summary>
    /// Window shown when closing an editor and there are unsaved changes
    /// </summary>
    public partial class UnsavedChangesDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnsavedChangesDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The message about what has unsaved changes
        /// </summary>
        public string Message
        {
            get { return labelMessage.Text; }
            set { labelMessage.Text = value; }
        }
    }
}
