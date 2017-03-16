using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    /// <summary>
    /// Element host for the WPF WalmartStoreSetupControl
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Walmart, ExternallyOwned = true)]
    public partial class WalmartStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IWalmartStoreSetupControlViewModel viewModel;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreSetupControlHost"/> class.
        /// </summary>
        public WalmartStoreSetupControlHost(IWalmartStoreSetupControlViewModel viewModel, IMessageHelper messageHelper)
        {
            this.viewModel = viewModel;
            this.messageHelper = messageHelper;
            InitializeComponent();
            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Next was clicked
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            bool canMoveNext = false;

            try
            {
                viewModel.Save(GetStore<WalmartStoreEntity>());
                canMoveNext = true;
            }
            catch (WalmartException ex)
            {
                messageHelper.ShowError(this, $"Error connecting to Walmart:\n{ex.Message}");
            }

            if (!canMoveNext)
            {
                e.NextPage = this;
            }
        }

        /// <summary>
        /// Called when stepping into this wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<WalmartStoreEntity>());
        }
    }
}
