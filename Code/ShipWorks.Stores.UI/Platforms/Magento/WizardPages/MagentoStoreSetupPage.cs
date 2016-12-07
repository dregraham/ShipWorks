using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
        private readonly IMessageHelper messageHelper;
        private readonly IMagentoWizardSettingsControlViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreSetupPage(IMagentoWizardSettingsControlViewModel viewModel, IMessageHelper messageHelper)
        {
            InitializeComponent();
            StepNext += OnStepNextMagentoStoreSetupPage;
            this.viewModel = viewModel;
            ((MagentoStoreSetupControl) elementHost.Child).DataContext = viewModel;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Save magento store. Show error and stay on page if there was a problem.
        /// </summary>
        private void OnStepNextMagentoStoreSetupPage(object sender, WizardStepEventArgs e)
        {
            GenericResult<MagentoStoreEntity> genericResult = viewModel.Save(GetStore<MagentoStoreEntity>());
            if (!genericResult.Success)
            {
                messageHelper.ShowError(genericResult.Message);
                e.NextPage = this;
            }
        }
    }
}
