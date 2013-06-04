using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window to make a user confirm that they understand the severity of deleting a store
    /// </summary>
    public partial class StoreConfirmDeleteDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreConfirmDeleteDlg(string storeName)
        {
            InitializeComponent();

            labelStore.Text = string.Format(labelStore.Text, storeName);
        }

        /// <summary>
        /// Confirm the delete
        /// </summary>
        private void OnConfirm(object sender, EventArgs e)
        {
            delete.Enabled = checkBoxConfirm.Checked;
        }
    }
}
