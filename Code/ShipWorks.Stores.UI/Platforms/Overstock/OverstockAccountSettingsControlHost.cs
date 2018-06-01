using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Overstock;

namespace ShipWorks.Stores.UI.Platforms.Overstock
{
    /// <summary>
    /// Account settings page for Overstock
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Overstock)]
    public partial class OverstockAccountSettingsControlHost : AccountSettingsControlBase
    {
        private readonly IOverstockStoreSetupControlViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverstockAccountSettingsControlHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="messageHelper"></param>
        public OverstockAccountSettingsControlHost(IOverstockStoreSetupControlViewModel viewModel, IMessageHelper messageHelper)
        {
            this.viewModel = viewModel;
            InitializeComponent();
            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load(store as OverstockStoreEntity);
        }

        /// <summary>
        /// Save the data into the StoreEntity. Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save(store as OverstockStoreEntity);
        }
    }
}
