using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Users;
using System.Collections;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Help
{
    /// <summary>
    /// Window for submitting a help request to interapptive
    /// </summary>
    public partial class SubmitHelpRequestDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubmitHelpRequestDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadStores();
        }

        /// <summary>
        /// Load the stores UI
        /// </summary>
        private void LoadStores()
        {
            ArrayList stores = new ArrayList();

            if (UserSession.IsLoggedOn)
            {
                comboStores.DisplayMember = "Display";
                comboStores.ValueMember = "Value";

                // Add the stores
                stores.AddRange(StoreManager.GetAllStores().Select(s =>
                    new { Display = s.StoreName, StoreID = s.StoreID }).ToArray());

                if (stores.Count > 1)
                {
                    stores.Add(new { Display = "All / Not Applicable", StoreID = (long) -1 });
                }

                comboStores.DataSource = stores;
                comboStores.SelectedIndex = 0;
            }

            if (stores.Count <= 1)
            {
                panelStores.Visible = false;
                Height -= panelStores.Height;
            }
        }

        /// <summary>
        /// Change the selected store
        /// </summary>
        private void OnChangeSelectedStore(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Submit the web request
        /// </summary>
        private void OnSubmit(object sender, EventArgs e)
        {

        }
    }
}
