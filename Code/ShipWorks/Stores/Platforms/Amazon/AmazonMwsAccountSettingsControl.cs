using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Stores.Platforms.Amazon
{
    [ToolboxItem(true)]
    public partial class AmazonMwsAccountSettingsControl : AccountSettingsControlBase
    {
        AmazonStoreEntity amazonStore;

        Dictionary<string, List<AmazonMwsMarketplace>> marketplaceCache = new Dictionary<string, List<AmazonMwsMarketplace>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load settings from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            // validate we get an AmazonStoreEntity
            this.amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new ArgumentException("AmazonStoreEntity expected.", "store"); 
            }

            merchantID.Text = amazonStore.MerchantID;
            marketplaceID.Text = amazonStore.MarketplaceID;

            base.LoadStore(store);
        }

        /// <summary>
        /// Save user-entered values back to the entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            AmazonStoreEntity saveStore = store as AmazonStoreEntity;
            if (saveStore == null)
            {
                throw new ArgumentException("AmazonStoreEntity expected.", "store"); 
            }

            if (merchantID.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Your Merchant ID is required.");
                return false;
            }

            if (marketplaceID.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Your Marketplace ID is required.");
                return false;
            }

            // save the values
            saveStore.MerchantID = merchantID.Text.Trim();
            saveStore.MarketplaceID = marketplaceID.Text.Trim();
            saveStore.DomainName = GetStoreDomainName(saveStore.MarketplaceID);

            try
            {
                using (AmazonMwsClient client = new AmazonMwsClient(saveStore))
                {
                    client.TestCredentials();
                }

                return true;
            }
            catch (AmazonException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Get the domain name of the marketplace
        /// </summary>
        private string GetStoreDomainName(string marketplaceId)
        {
            // Find the domain name of the marketplace provided to account for Amazon Canada store; default to an empty string - another
            // attempt will be made to populate it when a link is clicked (primarily so existing customers don't have to take explicit
            // action to have the domain name populated
            string storeDomainName = string.Empty;

            List<AmazonMwsMarketplace> marketplaces = GetMarketplaces();

            if (marketplaces != null)
            {
                AmazonMwsMarketplace marketplace = marketplaces.FirstOrDefault(m => m.MarketplaceID.ToUpperInvariant() == marketplaceId.ToUpperInvariant());
                storeDomainName = marketplace == null || string.IsNullOrWhiteSpace(marketplace.DomainName) ? string.Empty : marketplace.DomainName;

                if (!string.IsNullOrWhiteSpace(storeDomainName))
                {
                    // We're just interested in the domain name without the "www." (e.g. amazon.com instead of www.amazon.com)
                    Uri url = new UriBuilder(storeDomainName).Uri;
                    storeDomainName = url.Host.Replace("www.", string.Empty);
                }
            }

            return storeDomainName;
        }

        /// <summary>
        /// Open the instruction dialog
        /// </summary>
        private void OnGetMerchantID(object sender, EventArgs e)
        {
            using (AuthorizationInstructionsDlg dlg = new AuthorizationInstructionsDlg(amazonStore.AmazonApiRegion))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    merchantID.Text = dlg.MerchantID;
                }
            }
        }

        /// <summary>
        /// Clicking the find marketplaces link
        /// </summary>
        private void OnClickFindMarketplaces(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(merchantID.Text))
            {
                MessageHelper.ShowMessage(this, "You must enter your Amazon Merchant ID before ShipWorks can find your marketplaces.");
                return;
            }

            List<AmazonMwsMarketplace> marketplaces = GetMarketplaces();

            if (marketplaces != null)
            {
                if (marketplaces.Count == 0)
                {
                    MessageHelper.ShowMessage(this, "No marketplaces were found for the given Merchant ID");
                    return;
                }

                using (AmazonMwsMarketplaceSelectionDlg dlg = new AmazonMwsMarketplaceSelectionDlg(marketplaces, marketplaceID.Text))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        marketplaceID.Text = dlg.SelectedMarketplace.MarketplaceID;
                    }
                }
            }
        }

        /// <summary>
        /// Return the list of marketplaces for the amazon merchant
        /// </summary>
        private List<AmazonMwsMarketplace> GetMarketplaces()
        {
            List<AmazonMwsMarketplace> marketplaces;
            if (marketplaceCache.TryGetValue(merchantID.Text, out marketplaces))
            {
                return marketplaces;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                using (AmazonMwsClient client = new AmazonMwsClient(amazonStore))
                {
                    marketplaces = client.GetMarketplaces(merchantID.Text);
                }
            }
            catch (AmazonException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return null;
            }

            marketplaceCache[merchantID.Text] = marketplaces;

            return marketplaces;
        }
    }
}
