using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Manual.Controls
{
    /// <summary>
    /// A factory to create a control to be displayed on the last page of the AddStoreWizard
    /// </summary>
    [KeyedComponent(typeof(IStoreWizardFinishPageControlFactory), StoreTypeCode.Manual)]
    public class ManualWizardFinishPageControlFactory : IStoreWizardFinishPageControlFactory
    {
        /// <summary>
        /// Create a ManualWizardFinishPageControl to be displayed on the last page of the AddStoreWizard
        /// </summary>
        public Control Create(StoreEntity store) =>
            new ManualWizardFinishPageControl();
    }
}
