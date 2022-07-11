using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Amazon
{
    /// <summary>
    /// Amazon Account Settings Control
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Amazon)]
    public partial class AmazonAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IAmazonAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAccountSettingsControl(IAmazonAccountSettingsViewModel viewModel)
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
            viewModel.Load((AmazonStoreEntity) store);
        }
    }
}