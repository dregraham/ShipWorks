using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Rakuten
{
    /// <summary>
    /// Store setup control host for Rakuten
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Rakuten, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class RakutenStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IRakutenStoreSetupControlViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenStoreSetupControlHost(IRakutenStoreSetupControlViewModel viewModel)
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
            viewModel.Load(GetStore<RakutenStoreEntity>());
        }

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<RakutenStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
