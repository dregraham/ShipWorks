using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Brightpearl
{
    /// <summary>
    /// Add Store Wizard page for configuring BrightPearl credentials
    /// </summary>
    public partial class BrightpearlAddStoreWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrightpearlAddStoreWizardPage"/> class.
        /// </summary>
        public BrightpearlAddStoreWizardPage()
        {
            InitializeComponent();
            
            accountSettingsControl.LoadControl();

            BrightpearlStoreType store = (BrightpearlStoreType) StoreTypeManager.GetType(StoreTypeCode.Brightpearl);

            helpLink.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// User is navigating away from this page, so save it's values
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            GenericModuleStoreEntity store = GetStore<GenericModuleStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericModuleConfigurationException ex)
            {
                string message = String.Format("The ShipWorks module returned invalid configuration information.  Please contact the module developer with the following information.\n\n{0}", ex.Message);
                MessageHelper.ShowError(this, message);

                e.NextPage = this;
                return;
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
