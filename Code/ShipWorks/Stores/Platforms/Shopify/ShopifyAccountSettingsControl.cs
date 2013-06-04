using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Account settings control for Shopify
    /// </summary>
    public partial class ShopifyAccountSettingsControl : AccountSettingsControlBase
    {
        ShopifyStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load data from the Shopify store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            this.store = (ShopifyStoreEntity) store;

            createTokenControl.InitializeForStore(this.store);

            UpdateDisplay();
        }

        /// <summary>
        /// Update what we are displaying to the user
        /// </summary>
        private void UpdateDisplay()
        {
            shopDisplayName.Text = store.ShopifyShopDisplayName;
        }

        /// <summary>
        /// A new token has been created
        /// </summary>
        private void OnTokenCreated(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        /// <summary>
        /// Export a token
        /// </summary>
        private void OnExportToken(object sender, EventArgs e)
        {
            ShopifyTokenUtility.ExportTokenFile(store, this);
        }

        /// <summary>
        /// Import a token
        /// </summary>
        private void OnImportToken(object sender, EventArgs e)
        {
            if (ShopifyTokenUtility.ImportTokenFile(store, this))
            {
                UpdateDisplay();
            }
        }
    }
}
