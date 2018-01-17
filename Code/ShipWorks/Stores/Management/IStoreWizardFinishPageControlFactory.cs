using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// An interface to a factory to create a StoreWizardFinishPageControl to be displayed 
    /// on the last page of the AddStoreWizard
    /// </summary>
    public interface IStoreWizardFinishPageControlFactory
    {
        /// <summary>
        /// Create a control to be displayed on the last page of the AddStoreWizard
        /// </summary>
        Control Create(StoreEntity store);
    }
}