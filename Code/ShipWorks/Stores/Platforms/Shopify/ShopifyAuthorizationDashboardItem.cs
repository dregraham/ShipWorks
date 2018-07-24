using System;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Dashboard item prompting users to upgrade their Shopify token
    /// </summary>
    public class ShopifyAuthorizationDashboardItem : DashboardStoreItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public ShopifyAuthorizationDashboardItem(ShopifyStoreEntity store) :
            base(store,
                "ShopifyToken",
                ShipWorks.Properties.Resources.warning16,
                "Your Shopify token needs additional permissions.",
                new DashboardActionMethod("[link]Update Now[/link]",
                (s, e) => OnUpdateToken(s, store)))
        {

        }

        /// <summary>
        /// Handle when the token is requested to be updated
        /// </summary>
        private static void OnUpdateToken(Control owner, ShopifyStoreEntity store)
        {
            using (ShopifyCreateTokenWizard wizard = new ShopifyCreateTokenWizard())
            {
                wizard.Store = store;

                if (wizard.ShowDialog(owner) == DialogResult.OK)
                {
                    store.SaveFields("");

                    store.ShopifyShopUrlName = wizard.ShopUrlName;
                    store.ShopifyAccessToken = wizard.ShopAccessToken;

                    try
                    {
                        ShopifyWebClient webClient = new ShopifyWebClient(store, null);
                        webClient.RetrieveShopInformation();

                        StoreManager.SaveStore(store);

                        (owner as DashboardBar).Dismiss();
                    }
                    catch (Exception ex)
                    {
                        store.RollbackFields("");

                        throw WebHelper.TranslateWebException(ex, typeof(ShopifyException));
                    }
                }
            }
        }
    }
}