using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Window for updating an eBay token that is expired or about to expire
    /// </summary>
    public partial class EbayTokenUpdateDlg : Form
    {
        EbayStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayTokenUpdateDlg(EbayStoreEntity store)
        {
            InitializeComponent();

            this.store = store;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            tokenManageControl.InitializeForStore(store);
        }

        /// <summary>
        /// The token has been imported
        /// </summary>
        private void OnTokenImported(object sender, EventArgs e)
        {
            tokenManageControl.Enabled = false;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(store);
            }
        }
    }
}
