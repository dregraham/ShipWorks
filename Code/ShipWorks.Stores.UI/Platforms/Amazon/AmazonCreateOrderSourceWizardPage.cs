using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Amazon
{
    /// <summary>
    /// Wizard page to capture the order source id
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Amazon, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class AmazonCreateOrderSourceWizardPage : AddStoreWizardPage
    {
        private readonly IAmazonCreateOrderSourceViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateOrderSourceWizardPage(IAmazonCreateOrderSourceViewModel viewModel)
        {
            this.viewModel = viewModel;
            Title = "Amazon Store Setup";
            Description = "Allow ShipWorks to access your Amazon store.";
            
            InitializeComponent();
        }

        /// <summary>
        /// Load the page when stepped into
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<AmazonStoreEntity>());
        }

        /// <summary>
        /// Save the page when finished
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<AmazonStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}