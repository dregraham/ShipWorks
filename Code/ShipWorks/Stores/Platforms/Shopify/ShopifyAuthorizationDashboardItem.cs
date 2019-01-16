using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
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
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IShopifyCreateTokenViewModel>()
                    .CreateToken()
                    .Bind(x => SaveTokenToStore(store, x))
                    .Do(_ => (owner as DashboardBar).Dismiss())
                    .OnFailure(ex => lifetimeScope.Resolve<IMessageHelper>().ShowError("Could not update token", ex));

            }
        }

        /// <summary>
        /// Save the token to store
        /// </summary>
        private static Result SaveTokenToStore(ShopifyStoreEntity store, (string name, string token) result)
        {
            var (name, token) = result;

            store.SaveFields("");

            store.ShopifyShopUrlName = name;
            store.ShopifyAccessToken = token;

            try
            {
                ShopifyWebClient webClient = new ShopifyWebClient(store, null);
                webClient.RetrieveShopInformation();

                StoreManager.SaveStore(store);

                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                store.RollbackFields("");

                return Result.FromError(WebHelper.TranslateWebException(ex, typeof(ShopifyException)));
            }
        }
    }
}