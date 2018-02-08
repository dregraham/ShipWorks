using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.UI.Controls;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// A factory to create a control to be displayed on the last page of the AddStoreWizard
    /// </summary>
    [KeyedComponent(typeof(IStoreWizardFinishPageControlFactory), StoreTypeCode.Odbc)]
    public class OdbcWizardFinishPageControlFactory : IStoreWizardFinishPageControlFactory
    {
        /// <summary>
        /// Create a StoreWizardFinishPageControl or an OdbcWizardFinishPageControl (based on the
        /// OnDemand setting) to be displayed on the last page of the AddStoreWizard
        /// </summary>
        public Control Create(StoreEntity store)
        {
            OdbcStoreEntity odbcStore = (OdbcStoreEntity) store;

            if (odbcStore.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
            {
                return new OdbcWizardFinishPageControl();
            }

            return new StoreWizardFinishPageControl();
        }
    }
}
