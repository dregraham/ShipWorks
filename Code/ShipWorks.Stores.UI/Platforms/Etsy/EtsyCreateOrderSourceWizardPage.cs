using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Etsy
{
    /// <summary>
    /// Wizard page to capture the order source id
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Etsy, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class EtsyCreateOrderSourceWizardPage : AddStoreWizardPage
    {
        private readonly IEtsyCreateOrderSourceViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyCreateOrderSourceWizardPage(IEtsyCreateOrderSourceViewModel viewModel)
        {
            this.viewModel = viewModel;
            Title = "Etsy Store Setup";
            Description = "Allow ShipWorks to access your Etsy store.";
            
            InitializeComponent();
        }

        /// <summary>
        /// Load the page when stepped into
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<EtsyStoreEntity>());
        }

        /// <summary>
        /// Save the page when finished
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<EtsyStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}