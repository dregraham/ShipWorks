using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
    /// <summary>
    /// Wizard page to capture the order source id
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Shopify, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class ShopifyCreateOrderSourceWizardPage : AddStoreWizardPage
    {
        private readonly IShopifyCreateOrderSourceViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateOrderSourceWizardPage(IShopifyCreateOrderSourceViewModel viewModel)
        {
            this.viewModel = viewModel;
            Title = "Shopify Store Setup";
            Description = "Allow ShipWorks to access your Shopify store.";
            
            InitializeComponent();
        }

        /// <summary>
        /// Load the page when stepped into
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<ShopifyStoreEntity>());
        }

        /// <summary>
        /// Save the page when finished
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<ShopifyStoreEntity>()).Result)
            {
                e.NextPage = this;
            }
        }
    }
}