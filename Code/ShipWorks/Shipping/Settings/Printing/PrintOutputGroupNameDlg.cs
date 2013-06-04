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

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// Window for entering the name of a print output group
    /// </summary>
    public partial class PrintOutputGroupNameDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintOutputGroupNameDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            name.SelectAll();
        }

        /// <summary>
        /// The current value in the name box
        /// </summary>
        public string PrintOutputName
        {
            get { return name.Text; }
            set { name.Text = value; }
        }

        /// <summary>
        /// Close the window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a name.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
