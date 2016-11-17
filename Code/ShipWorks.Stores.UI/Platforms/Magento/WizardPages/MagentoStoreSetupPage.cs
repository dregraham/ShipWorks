using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Magento.WizardPages
{
    /// <summary>
    /// Wizard page for getting Magento store settings from user
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Magento)]
    public partial class MagentoStoreSetupPage : AddStoreWizardPage
    {
        private IMessageHelper messageHelper;
        private IMagentoWizardSettingsControlViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreSetupPage(IMagentoWizardSettingsControlViewModel viewModel, IMessageHelper messageHelper)
        {
            InitializeComponent();
            StepNext += OnStepNextMagentoStoreSetupPage;
            this.viewModel = viewModel;
            ((MagentoWizardSettingsControl) elementHost.Child).DataContext = viewModel;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Save magento store. Show error and stay on page if there was a problem.
        /// </summary>
        private void OnStepNextMagentoStoreSetupPage(object sender, WizardStepEventArgs e)
        {
            try
            {
                viewModel.Save(GetStore<MagentoStoreEntity>());
            }
            catch (MagentoException ex)
            {
                messageHelper.ShowError(ex.Message);
                e.NextPage = this;
            }
        }
    }
}
