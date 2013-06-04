using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Window to show users when the application is closing because it is in SINGLE_USER mode
    /// </summary>
    public partial class SingleUserModeDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SingleUserModeDlg()
        {
            InitializeComponent();
        }
    }
}
