using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Settings control for an Amazon MWS account
    /// </summary>
    [ToolboxItem(true)]
    public partial class AmazonMwsAccountSettingsControl : AccountSettingsControlBase
    {
        AmazonStoreEntity amazonStore;

        readonly Dictionary<string, List<AmazonMwsMarketplace>> marketplaceCache = new Dictionary<string, List<AmazonMwsMarketplace>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Should the save operation use the async version
        /// </summary>
        public override bool IsSaveAsync => true;

        /// <summary>
        /// Developer account number for access to Amazon API
        /// </summary>
        private string AccountNumber
        {
            get
            {
                switch (amazonStore.AmazonApiRegion)
                {
                    case "US":
                        return "1025-5115-6476";
                    case "CA":
                        return "1025-5115-6476";
                    case "MX":
                        return "1025-5115-6476";
                    default:
                        return "2814-9468-9452";
                }
            }
        }

        /// <summary>
        /// Url for accessing the Amazon developer portal
        /// </summary>
        private string DeveloperUrl
        {
            get
            {
                switch (amazonStore.AmazonApiRegion)
                {
                    case "US":
                        return "http://developer.amazonservices.com";
                    case "CA":
                        return "http://developer.amazonservices.ca";
                    case "MX":
                        return "http://developer.amazonservices.com";
                    default:
                        return "http://developer.amazonservices.co.uk";
                }
            }
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            LoadStore(store, false);
        }

        /// <summary>
        /// Load settings from the store entity
        /// </summary>
        public void LoadStore(StoreEntity store, bool onWizard)
        {
            if (onWizard)
            {
                title.Visible = false;
                form.Location = new Point(25, 0);
            }
            else
            {
                title.Visible = true;
                form.Location = new Point(25, 30);
            }

            // validate we get an AmazonStoreEntity
            amazonStore = MethodConditions.EnsureArgumentIsNotNull(store as AmazonStoreEntity, nameof(store));

            merchantID.Text = amazonStore.MerchantID;
            authToken.Text = amazonStore.AuthToken;
            marketplaceID.Text = amazonStore.MarketplaceID;

            mwsLink.Text = string.Format("{0}.", DeveloperUrl);
            accountNumber.Text = AccountNumber;

            base.LoadStore(store);
        }

        /// <summary>
        /// Save user-entered values back to the entity
        /// </summary>
        public override async Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            AmazonStoreEntity saveStore = MethodConditions.EnsureArgumentIsNotNull(store as AmazonStoreEntity, nameof(store));

            if (merchantID.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Your Seller ID is required.");
                return false;
            }

            if (authToken.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Your Auth Token is required.");
                return false;
            }

            if (marketplaceID.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Your Marketplace ID is required.");
                return false;
            }

            // save the values
            saveStore.MerchantID = merchantID.Text.Trim();
            saveStore.AuthToken = authToken.Text.Trim();
            saveStore.MarketplaceID = marketplaceID.Text.Trim();

            try
            {
                saveStore.DomainName = await GetStoreDomainName(saveStore.MarketplaceID).ConfigureAwait(true);

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var client = lifetimeScope.Resolve<AmazonMwsClient>(TypedParameter.From(saveStore));
                    await client.TestCredentials().ConfigureAwait(true);
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
        private async Task<string> GetStoreDomainName(string marketplaceId)
        {
            // Find the domain name of the marketplace provided to account for Amazon Canada store; default to an empty string - another
            // attempt will be made to populate it when a link is clicked (primarily so existing customers don't have to take explicit
            // action to have the domain name populated
            string storeDomainName = string.Empty;

            List<AmazonMwsMarketplace> marketplaces = await GetMarketplaces().ConfigureAwait(false);

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
        /// Clicking the find marketplaces link
        /// </summary>
        private async void OnClickFindMarketplaces(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(merchantID.Text))
            {
                MessageHelper.ShowMessage(this, "You must enter your Amazon Seller ID before ShipWorks can find your marketplaces.");
                return;
            }

            if (string.IsNullOrWhiteSpace(authToken.Text))
            {
                MessageHelper.ShowMessage(this, "You must enter your Auth Token before ShipWorks can find your marketplaces.");
                return;
            }

            List<AmazonMwsMarketplace> marketplaces;

            try
            {
                buttonChooseMarketplace.Enabled = false;
                marketplaces = await GetMarketplaces().ConfigureAwait(true);
                buttonChooseMarketplace.Enabled = true;
            }
            catch (AmazonException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return;
            }

            if (marketplaces.Count == 0)
            {
                MessageHelper.ShowMessage(this, "No marketplaces were found for the given Seller ID");
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

        /// <summary>
        /// Return the list of marketplaces for the amazon merchant
        /// </summary>
        private async Task<List<AmazonMwsMarketplace>> GetMarketplaces()
        {
            amazonStore.MerchantID = merchantID.Text;
            amazonStore.AuthToken = authToken.Text;

            List<AmazonMwsMarketplace> marketplaces;
            if (marketplaceCache.TryGetValue(GetCacheKey(), out marketplaces))
            {
                return marketplaces;
            }

            Cursor.Current = Cursors.WaitCursor;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var client = lifetimeScope.Resolve<AmazonMwsClient>(TypedParameter.From(amazonStore));
                marketplaces = await client.GetMarketplaces().ConfigureAwait(false);
            }

            marketplaceCache[GetCacheKey()] = marketplaces;

            return marketplaces;
        }

        /// <summary>
        /// Gets the cache key
        /// </summary>
        private string GetCacheKey()
        {
            return string.Format("{0}-{1}",
                merchantID.Text.Trim(),
                authToken.Text.Trim());
        }

        /// <summary>
        /// Open the MWS registration page
        /// </summary>
        private void OnMWSLinkClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl(DeveloperUrl, this);
        }

        /// <summary>
        /// Called when [copy account number click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCopyAccountNumberClick(object sender, EventArgs e)
        {
            Clipboard.SetText(AccountNumber);
        }
    }
}
