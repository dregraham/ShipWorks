using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Controls
{
    /// <summary>
    /// A factory to create a StoreWizardFinishPageControl to be displayed on the last page of the AddStoreWizard
    /// </summary>
    [Component]
    public class StoreWizardFinishPageControlFactory: IStoreWizardFinishPageControlFactory
    {
        /// <summary>
        /// Create a StoreWizardFinishPageControl to be displayed on the last page of the AddStoreWizard
        /// </summary>
        public Control Create(StoreEntity store)
        {
            return new StoreWizardFinishPageControl();
        }
    }
}
