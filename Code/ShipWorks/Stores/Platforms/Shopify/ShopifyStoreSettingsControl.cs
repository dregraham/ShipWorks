using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Control for configuring non-credential settings for Shopify
    /// </summary>
    public partial class ShopifyStoreSettingsControl : StoreSettingsControlBase
    {
        ShopifyStoreEntity ShopifyStore;

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
            ShopifyStore = (ShopifyStoreEntity)store;

            requestedShippingOptions.SelectedValue = (ShopifyRequestedShippingField)ShopifyStore.ShopifyRequestedShippingOption;
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ShopifyStore = (ShopifyStoreEntity) store;

            ShopifyStore.ShopifyRequestedShippingOption = (int)requestedShippingOptions.SelectedValue;

            return true;
        }
    }
}
