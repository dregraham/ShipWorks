using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Platform;
using ShipWorks.Stores.Platforms.Shopify.DTOs;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Control for configuring non-credential settings for Shopify
    /// </summary>
    public partial class ShopifyStoreSettingsControl : StoreSettingsControlBase
    {
        private ShopifyStoreEntity shopifyStore;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopifyWebClient));

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

            LoadLocations(shopifyStore);

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

        /// <summary>
        /// Populates the fulfillment location dropdown with Shopify locations
        /// </summary>
        private void LoadLocations(ShopifyStoreEntity store)
        {
            List<ShopifyLocation> locations = new List<ShopifyLocation>();

            ShopifyLocation defaultLocation = new ShopifyLocation();
            defaultLocation.ID = 0;
            defaultLocation.Name = "Shop Default";

            locations.Add(defaultLocation);

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var webClient = lifetimeScope.Resolve<IShopifyWebClient>(TypedParameter.From(store), TypedParameter.From<IProgressReporter>(null));

                try
                {
                    locations.AddRange(webClient.GetLocations().Locations);
                }
                catch (ShopifyException ex)
                {
                    log.Error($"An error occurred retrieving the list of Shopify locations: {ex.Message}", ex);
                }
            }
        }
    }
}
