using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Jet;

namespace ShipWorks.Stores.UI.Platforms.Jet
{
    /// <summary>
    /// Account settings page for Jet
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Jet)]
    public partial class JetAccountSettingsControlHost : AccountSettingsControlBase
    {
        private readonly IJetStoreSetupControlViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAccountSettingsControlHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="messageHelper"></param>
        public JetAccountSettingsControlHost(IJetStoreSetupControlViewModel viewModel, IMessageHelper messageHelper)
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
            viewModel.Load(store as JetStoreEntity);
        }

        /// <summary>
        /// Save the data into the StoreEntity. Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save(store as JetStoreEntity);
        }
    }
}
