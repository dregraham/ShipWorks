using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using System.Threading.Tasks;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    /// <summary>
    /// Control for editing BigCommerce account details
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.BigCommerce, ExternallyOwned = true)]
    public partial class BigCommerceAccountSettingsControl : AccountSettingsControlBase
    {
        BigCommerceAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountSettingsControl(BigCommerceAccountSettingsViewModel viewModel) : this()
        {
            SetViewModel(viewModel);
        }

        /// <summary>
        /// Saving is async
        /// </summary>
        public override bool IsSaveAsync => true;

        /// <summary>
        /// Set the view model on the control
        /// </summary>
        public void SetViewModel(BigCommerceAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            ((BigCommerceAccountSettings) accountSettingsElementHost.Child).DataContext = viewModel;
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            viewModel.LoadStore(store as IBigCommerceStoreEntity);
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        public async override Task<bool> SaveToEntityAsync(StoreEntity store) =>
            await viewModel.SaveToEntity(store as BigCommerceStoreEntity);
    }
}
