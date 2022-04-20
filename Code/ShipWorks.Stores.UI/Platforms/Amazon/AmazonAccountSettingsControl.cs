using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.UI.Wizard;


namespace ShipWorks.Stores.UI.Platforms.Amazon
{
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Amazon)]
    public partial class AmazonAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IAmazonAccountSettingsViewModel viewModel;

        public AmazonAccountSettingsControl(IAmazonAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();            
        }

        public override bool SaveToEntity(StoreEntity store)
        {
            // nothing to save
            return true;
        }

        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((AmazonStoreEntity) store);
        }


    }
}