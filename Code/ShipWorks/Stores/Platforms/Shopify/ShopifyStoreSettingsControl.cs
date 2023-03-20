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
    }
}
