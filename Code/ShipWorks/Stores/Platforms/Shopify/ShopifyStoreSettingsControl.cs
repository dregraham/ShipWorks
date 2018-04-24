using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Shopify.Enums;

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

            EnumHelper.BindComboBox<ShopifyRequestedShippingField>(requestedShippingOptions);
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            shopifyStore = (ShopifyStoreEntity) store;

            requestedShippingOptions.SelectedValue = (ShopifyRequestedShippingField) shopifyStore.ShopifyRequestedShippingOption;
            shopifyNotifyCustomer.Checked = shopifyStore.ShopifyNotifyCustomer;
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            shopifyStore = (ShopifyStoreEntity) store;

            shopifyStore.ShopifyRequestedShippingOption = (int) requestedShippingOptions.SelectedValue;
            shopifyStore.ShopifyNotifyCustomer = shopifyNotifyCustomer.Checked;

            return true;
        }
    }
}
