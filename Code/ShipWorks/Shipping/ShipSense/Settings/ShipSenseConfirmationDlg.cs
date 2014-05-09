using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseConfirmationDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseConfirmationDlg"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ShipSenseConfirmationDlg(string description)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to reload the knowledge base.
        /// </summary>
        public bool IsReloadRequested { get; set; }
    }
}
