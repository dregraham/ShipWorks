using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Manual
{
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Manual, ExternallyOwned = false)]
    public class ManualStoreSetupControlHost : AddStoreWizardPage {}
}