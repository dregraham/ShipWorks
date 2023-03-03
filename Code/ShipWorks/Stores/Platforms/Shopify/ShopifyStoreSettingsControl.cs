using System;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Control for configuring non-credential settings for Shopify
    /// </summary>
    public partial class ShopifyStoreSettingsControl : StoreSettingsControlBase
    {
        private ShopifyStoreEntity shopifyStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            shopifyStore = (ShopifyStoreEntity) store;

            shopifyNotifyCustomer.Checked = shopifyStore.ShopifyNotifyCustomer;
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            shopifyStore = (ShopifyStoreEntity) store;

            shopifyStore.ShopifyNotifyCustomer = shopifyNotifyCustomer.Checked;

            return true;
        }

        /// <summary>
        /// Update the store settings in platform
        /// </summary>
        public override async Task<bool> SaveToPlatform(StoreEntity store)
        {
            ShopifyStoreEntity storeEntity = store as ShopifyStoreEntity;
            if (storeEntity.ShopifyNotifyCustomer != shopifyNotifyCustomer.Checked)
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var orderSourceClient = lifetimeScope.Resolve<IHubOrderSourceClient>();
                    try
                    {
                        await orderSourceClient.UpdateShopifyNotifyCustomer(store.OrderSourceID, shopifyNotifyCustomer.Checked).ConfigureAwait(false);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        var messageHelper = lifetimeScope.Resolve<IMessageHelper>();
                        var loggerFactory = lifetimeScope.Resolve<Func<Type, ILog>>();
                        var logger = loggerFactory(typeof(ShopifyStoreSettingsControl));

                        logger.Error("An error occured updating the shopify store settings in platform", ex);
                        messageHelper.ShowError("Failed to update settings. Please try again.");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
