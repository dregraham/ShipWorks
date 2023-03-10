using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
    /// <summary>
    /// Shopify Account Settings Control
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Shopify)]
    public partial class ShopifyAccountSettingsMigrationControl : AccountSettingsControlBase
    {
        private readonly IShopifyMigrateOrderSourceViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAccountSettingsMigrationControl(IShopifyMigrateOrderSourceViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
        }

        public override bool IsSaveAsync => true;

        public async override Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            return await viewModel.Save((ShopifyStoreEntity) store);
        }

        /// <summary>
        /// Loads the store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((ShopifyStoreEntity) store);
        }
    }
}