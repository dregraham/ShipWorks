using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreSetupControlHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public WalmartStoreSetupControlHost(IWalmartStoreSetupControlViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Next was clicked
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            bool canMoveNext = viewModel.Save(GetStore<WalmartStoreEntity>());

            if (!canMoveNext)
            {
                e.NextPage = this;
            }
        }
    }
}
