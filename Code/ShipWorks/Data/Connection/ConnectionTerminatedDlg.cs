using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Window that is shown when the connection is lost with no hope of getting it back.
    /// </summary>
    public partial class ConnectionTerminatedDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionTerminatedDlg()
        {
            InitializeComponent();
        }
    }
}