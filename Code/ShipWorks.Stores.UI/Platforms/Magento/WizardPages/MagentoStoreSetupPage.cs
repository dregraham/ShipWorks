using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Magento.WizardPages
{
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Magento)]
    public partial class MagentoStoreSetupPage : AddStoreWizardPage
    {
        public MagentoStoreSetupPage()
        {
            InitializeComponent();
        }
    }
}
