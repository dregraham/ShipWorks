using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Overstock;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Overstock
{
    /// <summary>
    /// Store setup control host for Overstock
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Overstock, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class OverstockStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IOverstockStoreSetupControlViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockStoreSetupControlHost(IOverstockStoreSetupControlViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();

            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Called when stepping into this wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<OverstockStoreEntity>());
        }

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<OverstockStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
