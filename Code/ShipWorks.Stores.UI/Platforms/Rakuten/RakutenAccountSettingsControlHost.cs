using System.Threading.Tasks;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Rakuten;

namespace ShipWorks.Stores.UI.Platforms.Rakuten
{
    /// <summary>
    /// Account settings page for Rakuten
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Rakuten)]
    public partial class RakutenAccountSettingsControlHost : AccountSettingsControlBase
    {
        private readonly IRakutenStoreSetupControlViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RakutenAccountSettingsControlHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="messageHelper"></param>
        public RakutenAccountSettingsControlHost(IRakutenStoreSetupControlViewModel viewModel, IMessageHelper messageHelper)
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
            viewModel.Load(store as RakutenStoreEntity);
        }

        /// <summary>
        /// Should the save operation use the async version
        /// </summary>
        public override bool IsSaveAsync => true;

        /// <summary>
        /// Save the data into the StoreEntity. Nothing is saved to the database.
        /// </summary>
        public override async Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            return await viewModel.Save(store as RakutenStoreEntity).ConfigureAwait(false);
        }
    }
}
