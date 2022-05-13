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

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Store account settings control for Etsy
    /// </summary>
    public partial class EtsyAccountSettingsControl : AccountSettingsControlBase
    {
        EtsyStoreEntity etsyStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fires at first use. LoadsStore and loads token information.
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            etsyStore = store as EtsyStoreEntity;

            if (etsyStore == null)
            {
                throw new ArgumentException("Invalid store sent to EtsyManageTokenAccountSettings", "store");
            }

            base.LoadStore(store);

            tokenControl.LoadStore(store);

            LoadTokenInfo();
        }

        /// <summary>
        /// Handles TokenImported event
        /// </summary>
        void OnTokenImported(object sender, EventArgs e)
        {
            LoadTokenInfo();
        }

        /// <summary>
        /// Loads Token information into TokenInfo textbox.
        /// </summary>
        private void LoadTokenInfo()
        {
            tokenInfoBox.Text = string.Format("Authorized to store '{0}' with user '{1}'",
                etsyStore.EtsyLoginName,
                etsyStore.EtsyStoreName);
        }

        /// <summary>
        /// Handles LingImportToken Click
        /// </summary>
        private void OnLinkImportTokenClicked(object sender, EventArgs e)
        {
            tokenControl.ImportToken();
        }

        /// <summary>
        /// Handles LinkExportToken Click
        /// </summary>
        private void OnLinkExportTokenClicked(object sender, EventArgs e)
        {
            tokenControl.ExportToken();
        }

        /// <summary>
        /// Save the settings to the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            if (!tokenControl.HasUserUpdatedToken)
            {
                //User didn't enter a new token just return true so they can exit
                return true;
            }

            tokenControl.VerifyToken();
            return tokenControl.IsTokenValid;
        }
    }
}