using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Etsy
{
    /// <summary>
    /// Etsy Account Settings Control
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Etsy)]
    public partial class EtsyAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IEtsyAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyAccountSettingsControl(IEtsyAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();            
        }

        /// <summary>
        /// Nothing to save
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            // nothing to save, but underlying implementation throws
            return true;
        }

        /// <summary>
        /// Loads the store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((EtsyStoreEntity) store);
        }
    }
}