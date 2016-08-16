using System.Windows.Forms;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.SellerVantage.WizardPages
{
    /// <summary>
    /// Add Store Wizard page for configuring GenericStore credentials
    /// </summary>
    public partial class SellerVantageAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerVantageAccountPage()
        {
            InitializeComponent();

            SellerVantageStoreType store = (SellerVantageStoreType)StoreTypeManager.GetType(StoreTypeCode.SellerVantage);

            helpLink.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// User is navigating away from this page, so save it's values
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            GenericModuleStoreEntity store = GetStore<GenericModuleStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = this;
                return;
            }
        }
    }
}
